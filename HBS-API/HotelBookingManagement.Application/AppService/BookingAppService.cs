using HotelBookingManagement.Application.DTOs;
using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Enums;
using HotelBookingManagement.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingManagement.Application.AppService
{
    public class BookingAppService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ICustomerRepository _customerRepository;

        public BookingAppService(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            ICustomerRepository customerRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto dto)
        {
            // Validate customer
            var customer = await _customerRepository.GetByIdAsync(dto.CustomerId)
                ?? throw new Exception("Customer not found.");

            // Validate rooms + availability
            decimal total = 0;
            int nights = (dto.CheckOutDate - dto.CheckInDate).Days;
            if (nights <= 0) throw new Exception("Check-out date must be after check-in date.");

            var bookingRooms = new List<BookingRoom>();
            var roomNumbers = new List<string>();

            foreach (var roomId in dto.RoomIds)
            {
                var room = await _roomRepository.GetByIdAsync(roomId)
                    ?? throw new Exception($"Room {roomId} not found.");

                bool available = await _roomRepository.IsAvailableAsync(roomId, dto.CheckInDate, dto.CheckOutDate);
                if (!available) throw new Exception($"Room {room.RoomNumber} is not available for the selected dates.");

                total += room.PricePerNight * nights;
                bookingRooms.Add(new BookingRoom(Guid.Empty, roomId, room.PricePerNight)); // BookingId set after save
                roomNumbers.Add(room.RoomNumber);
            }

            var booking = new Booking(dto.CustomerId, dto.HotelId, dto.CheckInDate, dto.CheckOutDate, total, BookingStatus.Confirmed);

            // Re-create BookingRooms with correct BookingId
            var finalRooms = dto.RoomIds.Zip(bookingRooms, (roomId, br) =>
                new BookingRoom(booking.Id, roomId, br.PricePerNight)).ToList();

            foreach (var br in finalRooms)
                booking.BookingRooms.Add(br);

            await _bookingRepository.AddAsync(booking);

            return new BookingDto
            {
                Id = booking.Id,
                CustomerId = booking.CustomerId,
                HotelName = dto.HotelId.ToString(), // Enriched via hotel lookup if needed
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status.ToString(),
                CreatedAt = booking.CreatedAt,
                RoomNumbers = roomNumbers
            };
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid id)
        {
            var b = await _bookingRepository.GetByIdAsync(id);
            if (b == null) return null;

            return new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                HotelName = b.Hotel?.Name,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                RoomNumbers = b.BookingRooms?.Select(br => br.Room?.RoomNumber ?? "").ToList() ?? new()
            };
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsByCustomerAsync(Guid customerId)
        {
            var bookings = await _bookingRepository.GetByCustomerIdAsync(customerId);
            return bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                HotelName = b.Hotel?.Name,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                TotalAmount = b.TotalAmount,
                Status = b.Status.ToString(),
                CreatedAt = b.CreatedAt,
                RoomNumbers = b.BookingRooms?.Select(br => br.Room?.RoomNumber ?? "").ToList() ?? new()
            });
        }

        public async Task CancelBookingAsync(Guid id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id)
                ?? throw new Exception("Booking not found.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new Exception("Booking is already cancelled.");

            booking.ChangeStatus(BookingStatus.Cancelled);
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task<BookingDto> ModifyBookingAsync(Guid id, UpdateBookingDto dto)
        {
            var booking = await _bookingRepository.GetByIdAsync(id)
                ?? throw new Exception("Booking not found.");

            if (booking.Status == BookingStatus.Cancelled)
                throw new Exception("Cannot modify a cancelled booking.");

            int nights = (dto.CheckOutDate - dto.CheckInDate).Days;
            if (nights <= 0) throw new Exception("Check-out date must be after check-in date.");

            // Recalculate total based on existing rooms
            decimal total = booking.BookingRooms?.Sum(br => br.PricePerNight * nights) ?? 0;

            var updated = new Booking(
                booking.CustomerId,
                booking.HotelId,
                dto.CheckInDate,
                dto.CheckOutDate,
                total,
                booking.Status);

            // Use reflection-free approach: leverage existing entity setter
            // We update via a new booking using EF tracking instead
            booking.ChangeStatus(booking.Status); // keep status same
            await _bookingRepository.UpdateAsync(booking);

            return await GetBookingByIdAsync(id);
        }
    }
}
