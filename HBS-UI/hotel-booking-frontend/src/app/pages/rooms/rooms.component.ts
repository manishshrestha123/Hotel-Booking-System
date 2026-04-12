import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RoomService } from '../../services/room.service';
import { Room, RoomFilter } from '../../models/room.model';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.scss']
})
export class RoomsComponent implements OnInit {
  rooms: Room[] = [];
  filteredRooms: Room[] = [];
  isLoading = true;
  error: string | null = null;

  filter: RoomFilter = {
    minPrice: undefined,
    maxPrice: undefined,
    minGuests: undefined
  };

  selectedRoomType = '';
  showFilterPanel = false;

  roomTypeOptions = [
    { label: 'All Types', value: '' },
    { label: 'Standard', value: 'Standard' },
    { label: 'Deluxe', value: 'Deluxe' },
    { label: 'Suite', value: 'Suite' },
    { label: 'Executive', value: 'Executive' },
  ];

  sortOrder: 'asc' | 'desc' | '' = '';

  constructor(private roomService: RoomService, private router: Router) {}

  ngOnInit(): void {
    this.loadRooms();
  }

  loadRooms(): void {
    this.isLoading = true;
    this.error = null;
    this.roomService.getAllRooms().subscribe({
      next: (data) => {
        this.rooms = data;
        this.filteredRooms = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.error = 'Failed to load rooms. Please try again.';
        this.isLoading = false;
        // Use mock data for demo purposes if API unavailable
        this.rooms = this.getMockRooms();
        this.filteredRooms = this.rooms;
      }
    });
  }

  applyFilter(): void {
    this.isLoading = true;
    this.error = null;
    const filterPayload: RoomFilter = {};
    if (this.filter.minPrice) filterPayload.minPrice = this.filter.minPrice;
    if (this.filter.maxPrice) filterPayload.maxPrice = this.filter.maxPrice;
    if (this.filter.minGuests) filterPayload.minGuests = this.filter.minGuests;

    this.roomService.filterRooms(filterPayload).subscribe({
      next: (data) => {
        this.rooms = data;
        this.applyLocalSort(data);
        this.isLoading = false;
        this.showFilterPanel = false;
      },
      error: () => {
        this.isLoading = false;
        // fallback: local filter on mock
        this.applyLocalFilter();
        this.showFilterPanel = false;
      }
    });
  }

  applyLocalFilter(): void {
    let result = [...this.rooms];
    if (this.filter.minPrice) result = result.filter(r => r.pricePerNight >= this.filter.minPrice!);
    if (this.filter.maxPrice) result = result.filter(r => r.pricePerNight <= this.filter.maxPrice!);
    if (this.filter.minGuests) result = result.filter(r => r.maxGuests >= this.filter.minGuests!);
    if (this.selectedRoomType) result = result.filter(r => r.roomTypeName === this.selectedRoomType);
    this.applyLocalSort(result);
  }

  applyLocalSort(data: Room[]): void {
    if (this.sortOrder === 'asc') {
      this.filteredRooms = [...data].sort((a, b) => a.pricePerNight - b.pricePerNight);
    } else if (this.sortOrder === 'desc') {
      this.filteredRooms = [...data].sort((a, b) => b.pricePerNight - a.pricePerNight);
    } else {
      this.filteredRooms = data;
    }
  }

  resetFilter(): void {
    this.filter = { minPrice: undefined, maxPrice: undefined, minGuests: undefined };
    this.selectedRoomType = '';
    this.sortOrder = '';
    this.filteredRooms = [...this.rooms];
  }

  onSortChange(): void {
    this.applyLocalSort([...this.filteredRooms]);
  }

  viewRoom(id: string): void {
    this.router.navigate(['/rooms', id]);
  }

  bookRoom(room: Room): void {
    this.router.navigate(['/bookings/create'], { queryParams: { roomId: room.id } });
  }

  getStatusClass(status: string): string {
    switch (status?.toLowerCase()) {
      case 'available': return 'status-available';
      case 'occupied': return 'status-occupied';
      case 'maintenance': return 'status-maintenance';
      default: return 'status-unknown';
    }
  }

  getRoomImage(room: Room): string {
    if (room.primaryImageUrl) return room.primaryImageUrl;
    const images: Record<string, string> = {
      'Standard': 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=600&q=80',
      'Deluxe': 'https://images.unsplash.com/photo-1591088398332-8a7791972843?w=600&q=80',
      'Suite': 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=600&q=80',
      'Executive': 'https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=600&q=80',
    };
    return images[room.roomTypeName] || 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=600&q=80';
  }

  getMockRooms(): Room[] {
    return [
      { id: '1', roomNumber: '101', pricePerNight: 120, status: 'Available', roomTypeName: 'Standard', maxGuests: 2, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '2', roomNumber: '201', pricePerNight: 180, status: 'Available', roomTypeName: 'Deluxe', maxGuests: 3, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '3', roomNumber: '301', pricePerNight: 280, status: 'Occupied', roomTypeName: 'Suite', maxGuests: 4, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '4', roomNumber: '401', pricePerNight: 350, status: 'Available', roomTypeName: 'Executive', maxGuests: 2, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '5', roomNumber: '102', pricePerNight: 115, status: 'Available', roomTypeName: 'Standard', maxGuests: 2, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
      { id: '6', roomNumber: '202', pricePerNight: 195, status: 'Maintenance', roomTypeName: 'Deluxe', maxGuests: 3, hotelName: 'PrimeStay Hotel', primaryImageUrl: '' },
    ];
  }
}
