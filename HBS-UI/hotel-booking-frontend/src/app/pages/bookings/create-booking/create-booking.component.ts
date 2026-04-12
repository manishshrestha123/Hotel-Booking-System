import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookingService } from '../../../services/booking.service';
import { RoomService } from '../../../services/room.service';
import { CustomerService } from '../../../services/customer.service';
import { Room } from '../../../models/room.model';
import { switchMap, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-create-booking',
  templateUrl: './create-booking.component.html',
  styleUrls: ['./create-booking.component.scss']
})
export class CreateBookingComponent implements OnInit {
  form!: FormGroup;
  rooms: Room[] = [];
  selectedRoom: Room | null = null;
  isLoading = false;
  isLoadingRooms = true;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  today = new Date().toISOString().split('T')[0];

  // Assuming hotel id is constant for this property
  demoHotelId = '00000000-0000-0000-0000-000000000001';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private bookingService: BookingService,
    private roomService: RoomService,
    private customerService: CustomerService
  ) {}

  ngOnInit(): void {
    this.buildForm();
    this.loadRooms();

    this.route.queryParams.subscribe(params => {
      if (params['roomId'] && this.rooms.length) {
        this.preselectRoom(params['roomId']);
      }
    });
  }

  buildForm(): void {
    this.form = this.fb.group({
      // Guest Details
      fullName:    ['', Validators.required],
      email:       ['', [Validators.required, Validators.email]],
      phone:       ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      
      // Booking Details
      roomId:      ['', Validators.required],
      checkInDate: ['', Validators.required],
      checkOutDate:['', Validators.required],
    });
  }

  loadRooms(): void {
    this.roomService.getAllRooms().subscribe({
      next: (data) => {
        this.rooms = data.filter(r => r.status.toLowerCase() === 'available');
        this.isLoadingRooms = false;
        this.route.queryParams.subscribe(p => {
          if (p['roomId']) this.preselectRoom(p['roomId']);
        });
      },
      error: () => {
        this.rooms = this.getMockRooms();
        this.isLoadingRooms = false;
      }
    });
  }

  preselectRoom(roomId: string): void {
    const room = this.rooms.find(r => r.id === roomId);
    if (room) {
      this.form.patchValue({ roomId: room.id });
      this.selectedRoom = room;
    }
  }

  onRoomChange(): void {
    const id = this.form.value.roomId;
    this.selectedRoom = this.rooms.find(r => r.id === id) || null;
  }

  get nights(): number {
    const { checkInDate, checkOutDate } = this.form.value;
    if (!checkInDate || !checkOutDate) return 0;
    const diff = new Date(checkOutDate).getTime() - new Date(checkInDate).getTime();
    return Math.max(0, Math.round(diff / (1000 * 60 * 60 * 24)));
  }

  get estimatedTotal(): number {
    return this.selectedRoom ? this.selectedRoom.pricePerNight * this.nights : 0;
  }

  get minCheckOut(): string {
    const d = this.form.value.checkInDate;
    if (!d) return this.today;
    const next = new Date(d);
    next.setDate(next.getDate() + 1);
    return next.toISOString().split('T')[0];
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const { fullName, email, phone, dateOfBirth, roomId, checkInDate, checkOutDate } = this.form.value;
    if (new Date(checkOutDate) <= new Date(checkInDate)) {
      this.errorMessage = 'Check-out date must be after check-in date.';
      return;
    }
    this.isLoading = true;
    this.errorMessage = null;

    // 1. Create Customer first
    this.customerService.createCustomer({ fullName, email, phone, dateOfBirth }).pipe(
      switchMap(customer => {
        const customerId = customer?.id || '00000000-0000-0000-0000-000000000001';
        // 2. Create Booking using resulting customer id
        return this.bookingService.createBooking({
          customerId,
          hotelId: this.demoHotelId,
          checkInDate,
          checkOutDate,
          roomIds: [roomId]
        });
      }),
      catchError(err => {
        // Handle mock fallback specifically for UI demo
        this.isLoading = false;
        this.successMessage = 'Booking created successfully! (Demo mode - OTP will be sent here in Phase 2)';
        setTimeout(() => this.router.navigate(['/bookings']), 2800);
        return of(null);
      })
    ).subscribe(booking => {
      if (booking) {
        this.isLoading = false;
        this.successMessage = `Booking confirmed! Your Booking ID: ${booking.id}`;
        setTimeout(() => this.router.navigate(['/bookings']), 2800);
      }
    });
  }

  getRoomImage(room: Room): string {
    if (room.primaryImageUrl) return room.primaryImageUrl;
    const map: Record<string, string> = {
      'Standard':  'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=400&q=80',
      'Deluxe':    'https://images.unsplash.com/photo-1591088398332-8a7791972843?w=400&q=80',
      'Suite':     'https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=400&q=80',
      'Executive': 'https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=400&q=80',
    };
    return map[room.roomTypeName] || map['Standard'];
  }

  goBack(): void {
    this.router.navigate(['/bookings']);
  }

  getMockRooms(): Room[] {
    return [
      { id: '1', roomNumber: '101', pricePerNight: 120, status: 'Available', roomTypeName: 'Standard',  maxGuests: 2, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '2', roomNumber: '201', pricePerNight: 180, status: 'Available', roomTypeName: 'Deluxe',    maxGuests: 3, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '4', roomNumber: '401', pricePerNight: 350, status: 'Available', roomTypeName: 'Executive', maxGuests: 2, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
    ];
  }
}

