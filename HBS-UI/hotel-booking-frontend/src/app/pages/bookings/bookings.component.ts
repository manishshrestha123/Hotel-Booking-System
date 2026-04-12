import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookingService } from '../../services/booking.service';
import { Booking } from '../../models/booking.model';

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

  // For demo – in real app get from auth token
  customerId = 'demo-customer-id';

  today = new Date().toISOString().split('T')[0];
  cancellingId: string | null = null;
  modifyingBooking: Booking | null = null;
  modifyCheckIn = '';
  modifyCheckOut = '';

  constructor(private bookingService: BookingService, private router: Router) {}

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.isLoading = true;
    this.error = null;
    this.bookingService.getBookingsByCustomer(this.customerId).subscribe({
      next: (data) => {
        this.bookings = data;
        this.isLoading = false;
      },
      error: () => {
        this.bookings = this.getMockBookings();
        this.isLoading = false;
        this.error = 'Could not reach server. Showing demo bookings.';
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
      error: () => {
        this.cancellingId = null;
        // demo fallback
        this.bookings = this.bookings.map(b =>
          b.id === id ? { ...b, status: 'Cancelled' } : b
        );
        this.showSuccess('Booking cancelled (demo mode).');
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
      error: () => {
        this.modifyingBooking = null;
        this.showSuccess('Booking updated (demo mode).');
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

  getMockBookings(): Booking[] {
    return [
      {
        id: 'bk-001', customerId: this.customerId, hotelName: 'PrimeStay Hotel',
        checkInDate: '2026-05-10T00:00:00', checkOutDate: '2026-05-15T00:00:00',
        totalAmount: 900, status: 'Confirmed', createdAt: '2026-04-12T10:00:00',
        roomNumbers: ['201']
      },
      {
        id: 'bk-002', customerId: this.customerId, hotelName: 'PrimeStay Hotel',
        checkInDate: '2026-06-01T00:00:00', checkOutDate: '2026-06-03T00:00:00',
        totalAmount: 560, status: 'Confirmed', createdAt: '2026-04-10T10:00:00',
        roomNumbers: ['101', '102']
      },
      {
        id: 'bk-003', customerId: this.customerId, hotelName: 'PrimeStay Hotel',
        checkInDate: '2026-03-01T00:00:00', checkOutDate: '2026-03-03T00:00:00',
        totalAmount: 360, status: 'Cancelled', createdAt: '2026-02-20T08:00:00',
        roomNumbers: ['301']
      }
    ];
  }
}
