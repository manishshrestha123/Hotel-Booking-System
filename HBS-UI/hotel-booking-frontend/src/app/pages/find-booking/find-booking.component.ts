import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-find-booking',
  templateUrl: './find-booking.component.html',
  styleUrls: ['./find-booking.component.scss'],
  standalone: false
})
export class FindBookingComponent {
  searchQuery = '';
  isLoading = false;
  errorMessage: string | null = null;
  foundBookings: any[] = [];

  constructor(private http: HttpClient, private router: Router) {}

  onSearch() {
    if (!this.searchQuery.trim()) {
      this.errorMessage = 'Please enter an Email or Booking ID.';
      return;
    }
    
    this.isLoading = true;
    this.errorMessage = null;
    this.foundBookings = [];

    this.http.get<any[]>(`${environment.apiUrl}/bookings/find?identifier=${encodeURIComponent(this.searchQuery.trim())}`)
      .subscribe({
        next: (data) => {
          this.isLoading = false;
          if (data && data.length > 0) {
            this.foundBookings = data;
          } else {
            this.errorMessage = 'No bookings found for the provided information.';
          }
        },
        error: (err) => {
          this.isLoading = false;
          // Dummy data for demo purpose if API not running yet
          this.foundBookings = [{
            id: 'demo-b83c-1234',
            hotelName: 'PrimeStay Hotel',
            checkInDate: new Date().toISOString(),
            checkOutDate: new Date(Date.now() + 86400000).toISOString(),
            totalAmount: 450,
            status: 'Confirmed'
          }];
          this.errorMessage = 'Simulated Backend Reply: Showing Demo booking.';
        }
      });
  }

  getBadgeClass(status: string): string {
    const s = status.toLowerCase();
    if (s === 'confirmed') return 'badge-success';
    if (s === 'cancelled') return 'badge-danger';
    return 'badge-warning';
  }
}
