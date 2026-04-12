import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Booking, CreateBooking, UpdateBooking } from '../models/booking.model';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private baseUrl = `${environment.apiUrl}/bookings`;

  constructor(private http: HttpClient) {}

  createBooking(dto: CreateBooking): Observable<Booking> {
    return this.http.post<Booking>(this.baseUrl, dto);
  }

  getBookingById(id: string): Observable<Booking> {
    return this.http.get<Booking>(`${this.baseUrl}/${id}`);
  }

  getBookingsByCustomer(customerId: string): Observable<Booking[]> {
    return this.http.get<Booking[]>(`${this.baseUrl}/customer/${customerId}`);
  }

  cancelBooking(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  modifyBooking(id: string, dto: UpdateBooking): Observable<Booking> {
    return this.http.put<Booking>(`${this.baseUrl}/${id}`, dto);
  }
}
