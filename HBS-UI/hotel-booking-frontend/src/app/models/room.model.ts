export interface Room {
  id: string;
  roomNumber: string;
  pricePerNight: number;
  status: string;
  roomTypeName: string;
  maxGuests: number;
  hotelName: string;
  primaryImageUrl?: string;
}

export interface RoomFilter {
  minPrice?: number;
  maxPrice?: number;
  roomTypeId?: string;
  minGuests?: number;
}

export interface RoomAvailability {
  date: string;
  isAvailable: boolean;
  priceOverride?: number;
}

export interface CreateRoom {
  hotelId: string;
  roomTypeId: string;
  roomNumber: string;
  pricePerNight: number;
  status: string;
}
