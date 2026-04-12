import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Room, RoomFilter, RoomAvailability, CreateRoom } from '../models/room.model';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  private baseUrl = `${environment.apiUrl}/rooms`;

  constructor(private http: HttpClient) {}

  getAllRooms(): Observable<Room[]> {
    return this.http.get<Room[]>(this.baseUrl);
  }

  getRoomById(id: string): Observable<Room> {
    return this.http.get<Room>(`${this.baseUrl}/${id}`);
  }

  filterRooms(filter: RoomFilter): Observable<Room[]> {
    let params = new HttpParams();
    if (filter.minPrice != null) params = params.set('minPrice', filter.minPrice);
    if (filter.maxPrice != null) params = params.set('maxPrice', filter.maxPrice);
    if (filter.roomTypeId) params = params.set('roomTypeId', filter.roomTypeId);
    if (filter.minGuests != null) params = params.set('minGuests', filter.minGuests);
    return this.http.get<Room[]>(`${this.baseUrl}/filter`, { params });
  }

  checkAvailability(id: string, checkIn: string, checkOut: string): Observable<RoomAvailability[]> {
    const params = new HttpParams()
      .set('checkIn', checkIn)
      .set('checkOut', checkOut);
    return this.http.get<RoomAvailability[]>(`${this.baseUrl}/${id}/availability`, { params });
  }

  createRoom(room: CreateRoom): Observable<Room> {
    return this.http.post<Room>(this.baseUrl, room);
  }
}
