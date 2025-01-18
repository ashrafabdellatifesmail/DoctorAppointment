using DoctorAppointment.DoctorAvailability.Interfaces;
using DoctorAppointment.DoctorAvailability.Models;
using DoctorAppointment.DoctorAvailability.Repositories;
using DoctorAppointment.Shared.Constants;
using DoctorAppointment.Shared.Models;

namespace DoctorAppointment.DoctorAvailability.Services
{
    public class SlotService : ISlotService
    {
        private readonly ISlotRepository _repository;

        public SlotService(ISlotRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SlotDto>> GetAllSlots()
        {
            var slots = await _repository.GetAllSlots();
            return slots.Select(s => new SlotDto
            {
                Id = s.Id,
                Time = s.Time,
                DoctorId = s.DoctorId,
                DoctorName = s.DoctorName,
                IsReserved = s.IsReserved,
                Cost = s.Cost
            });
        }

        public async Task<SlotDto> AddSlot(SlotDto slotDto)
        {
            var slot = new Slot
            {
                Id = Guid.NewGuid(),
                Time = slotDto.Time,
                DoctorId = SystemConstants.DefaultDoctorId,
                DoctorName = SystemConstants.DefaultDoctorName,
                IsReserved = false,
                Cost = slotDto.Cost
            };

            var result = await _repository.AddSlot(slot);
            return new SlotDto
            {
                Id = result.Id,
                Time = result.Time,
                DoctorId = result.DoctorId,
                DoctorName = result.DoctorName,
                IsReserved = result.IsReserved,
                Cost = result.Cost
            };
        }
    }
}
