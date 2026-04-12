import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-bookings',
  templateUrl: './bookings.component.html',
  styleUrls: ['./bookings.component.scss']
})
export class BookingsComponent implements OnInit {
  bookings: Booking[] = [];
  isLoading = true;
  error: string | null = null;
  successMessage: string | null = null;
  customerEmail: string | null = null;

  today = new Date().toISOString().split('T')[0];
  cancellingId: string | null = null;
  modifyingBooking: Booking | null = null;
  modifyCheckIn = '';
  modifyCheckOut = '';

  constructor(
    private bookingService: BookingService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.customerEmail = this.authService.getSession()?.email ?? null;
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.error = null;

    if (!this.customerEmail) {
      this.bookings = [];
      this.isLoading = false;
      this.error = 'Please log in with a customer account to view your bookings.';
      return;
    }

    this.bookingService.findBookings(this.customerEmail).subscribe({
      next: (data) => {
        this.bookings = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.bookings = [];
        this.isLoading = false;
        this.error = err.error?.message || 'Could not load your bookings from the server.';
      }
    });
  }

  goCreate(): void {
    this.router.navigate(['/bookings/create']);
  }

  cancelBooking(id: string): void {
    if (!confirm('Are you sure you want to cancel this booking?')) return;
    this.cancellingId = id;
    this.bookingService.cancelBooking(id).subscribe({
      next: () => {
        this.bookings = this.bookings.map(b =>
          b.id === id ? { ...b, status: 'Cancelled' } : b
        );
        this.cancellingId = null;
        this.showSuccess('Booking cancelled successfully.');
      },
      error: (err) => {
        this.cancellingId = null;
        this.error = err.error?.message || 'Could not cancel the booking.';
      }
    });
  }

  openModify(booking: Booking): void {
    this.modifyingBooking = booking;
    this.modifyCheckIn = booking.checkInDate.substring(0, 10);
    this.modifyCheckOut = booking.checkOutDate.substring(0, 10);
  }

  submitModify(): void {
    if (!this.modifyingBooking) return;

    this.bookingService.modifyBooking(this.modifyingBooking.id, {
      checkInDate: this.modifyCheckIn,
      checkOutDate: this.modifyCheckOut
    }).subscribe({
      next: (updated) => {
        this.bookings = this.bookings.map(b => b.id === updated.id ? updated : b);
        this.modifyingBooking = null;
        this.showSuccess('Booking updated successfully!');
      },
      error: (err) => {
        this.modifyingBooking = null;
        this.error = err.error?.message || 'Could not update the booking.';
      }
    });
  }

  cancelModify(): void {
    this.modifyingBooking = null;
  }

  showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => (this.successMessage = null), 3500);
  }

  getStatusClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'confirmed': return 'status-confirmed';
      case 'cancelled': return 'status-cancelled';
      case 'pending': return 'status-pending';
      default: return 'status-unknown';
    }
  }

  getNights(checkIn: string, checkOut: string): number {
    const diff = new Date(checkOut).getTime() - new Date(checkIn).getTime();
    return Math.max(1, Math.round(diff / (1000 * 60 * 60 * 24)));
  }
}
