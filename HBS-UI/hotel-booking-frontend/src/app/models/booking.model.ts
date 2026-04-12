export interface Booking {
  id: string;
  customerId: string;
  hotelName: string;
  checkInDate: string;
  checkOutDate: string;
  totalAmount: number;
  status: string;
  createdAt: string;
  roomNumbers: string[];
}

export interface CreateBooking {
  customerId: string;
  hotelId: string;
  checkInDate: string;
  checkOutDate: string;
  roomIds: string[];
}

export interface UpdateBooking {
  checkInDate: string;
  checkOutDate: string;
}
