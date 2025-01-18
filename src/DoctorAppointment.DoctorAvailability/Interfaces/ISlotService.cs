using DoctorAppointment.Shared.Models;

namespace DoctorAppointment.DoctorAvailability.Interfaces
{
    public interface ISlotService
    {
        Task<IEnumerable<SlotDto>> GetAllSlots();
        Task<SlotDto> AddSlot(SlotDto slotDto);
    }
}
