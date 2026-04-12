import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginCredentials {
  identifier: string;
  password: string;
}

export interface AuthResponse {
  id: string;
  token: string;
  username: string;
  email: string;
  role: string;
}

export interface AuthSession {
  token: string;
  username: string;
  email: string;
  userId: string;
  role: string;
}

export interface CreateUserPayload {
  fullName: string;
  username: string;
  email: string;
  password: string;
  role: number;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl = `${environment.apiUrl}/auth`;
  private tokenKey = 'access_token';
  private roleKey = 'user_role';
  private sessionKey = 'auth_session';
  private guestKey = 'guest_session';

  constructor(private http: HttpClient) {}

  loginUser(credentials: LoginCredentials): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, credentials);
  }

  loginCustomer(credentials: LoginCredentials): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/customer/login`, credentials);
  }

  register(data: {
    fullName: string;
    username: string;
    email: string;
    password: string;
  }): Observable<{ message: string }> {
    return this.createUser({ ...data, role: 3 });
  }

  createUser(data: CreateUserPayload): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/users`, data);
  }

  saveSession(response: AuthResponse): AuthSession {
    const session: AuthSession = {
      token: response.token,
      username: response.username,
      email: response.email,
      userId: response.id,
      role: response.role,
    };

    if (this.hasStorage()) {
      localStorage.setItem(this.tokenKey, session.token);
      localStorage.setItem(this.roleKey, session.role);
      localStorage.setItem(this.sessionKey, JSON.stringify(session));
      localStorage.removeItem(this.guestKey);
    }

    return session;
  }

  startGuestSession(): void {
    if (this.hasStorage()) {
      localStorage.setItem(this.guestKey, 'true');
      localStorage.removeItem(this.tokenKey);
      localStorage.removeItem(this.roleKey);
      localStorage.removeItem(this.sessionKey);
    }
  }

  getSession(): AuthSession | null {
    if (!this.hasStorage()) {
      return null;
    }

    const raw = localStorage.getItem(this.sessionKey);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AuthSession;
    } catch {
      this.clearSession();
      return null;
    }
  }

  hasRole(...roles: string[]): boolean {
    const session = this.getSession();
    if (!session) {
      return false;
    }

    return roles.some(role => role.toLowerCase() === session.role.toLowerCase());
  }

  isGuestSession(): boolean {
    return this.hasStorage() && localStorage.getItem(this.guestKey) === 'true';
  }

  logout(): void {
    this.clearSession();
  }

  isLoggedIn(): boolean {
    return this.hasStorage() && !!localStorage.getItem(this.tokenKey);
  }

  private clearSession(): void {
    if (!this.hasStorage()) {
      return;
    }

    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.roleKey);
    localStorage.removeItem(this.sessionKey);
    localStorage.removeItem(this.guestKey);
  }

  private hasStorage(): boolean {
    return typeof localStorage !== 'undefined';
  }
}
