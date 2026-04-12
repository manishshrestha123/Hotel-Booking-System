import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Customer, CreateCustomer } from '../models/customer.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private baseUrl = `${environment.apiUrl}/customers`;

  constructor(private http: HttpClient) {}

  createCustomer(dto: CreateCustomer): Observable<Customer> {
    return this.http.post<Customer>(this.baseUrl, dto);
  }

  getCustomerById(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/${id}`);
  }
}
