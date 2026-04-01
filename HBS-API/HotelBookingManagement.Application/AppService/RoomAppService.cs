using HotelBookingManagement.Application.DTOs;
using HotelBookingManagement.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingManagement.Application.AppService
{
    public class RoomAppService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomAppService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                Status = r.Status.ToString(),
                RoomTypeName = r.RoomType?.Name,
                MaxGuests = r.RoomType?.MaxGuests ?? 0,
                HotelName = r.Hotel?.Name,
                PrimaryImageUrl = r.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
            });
        }

        public async Task<RoomDto?> GetRoomByIdAsync(Guid id)
        {
            var r = await _roomRepository.GetByIdAsync(id);
            if (r == null) return null;

            return new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                Status = r.Status.ToString(),
                RoomTypeName = r.RoomType?.Name,
                MaxGuests = r.RoomType?.MaxGuests ?? 0,
                HotelName = r.Hotel?.Name,
                PrimaryImageUrl = r.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
            };
        }

        public async Task<IEnumerable<RoomDto>> FilterRoomsAsync(RoomFilterDto filter)
        {
            var rooms = await _roomRepository.FilterAsync(filter.MinPrice, filter.MaxPrice, filter.RoomTypeId, filter.MinGuests);
            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                PricePerNight = r.PricePerNight,
                Status = r.Status.ToString(),
                RoomTypeName = r.RoomType?.Name,
                MaxGuests = r.RoomType?.MaxGuests ?? 0,
                HotelName = r.Hotel?.Name,
                PrimaryImageUrl = r.Images?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
            });
        }

        public async Task<IEnumerable<RoomAvailabilityDto>> CheckAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            var availabilities = await _roomRepository.GetAvailabilityAsync(roomId, checkIn, checkOut);
            return availabilities.Select(a => new RoomAvailabilityDto
            {
                Date = a.Date,
                IsAvailable = a.IsAvailable,
                PriceOverride = a.PriceOverride
            });
        }

        public async Task<RoomDto> CreateRoomAsync(CreateRoomDto dto)
        {
            var room = new HotelBookingManagement.Domain.Entities.Room(
                dto.HotelId,
                dto.RoomTypeId,
                dto.RoomNumber,
                dto.PricePerNight,
                dto.Status
            );

            await _roomRepository.AddAsync(room);

            return new RoomDto
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                PricePerNight = room.PricePerNight,
                Status = room.Status.ToString()
            };
        }
    }
}
