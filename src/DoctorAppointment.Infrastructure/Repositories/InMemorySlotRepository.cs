using DoctorAppointment.DoctorAvailability.Models;
using DoctorAppointment.DoctorAvailability.Repositories;

namespace DoctorAppointment.Infrastructure.Repositories
{
    public class InMemorySlotRepository : ISlotRepository
    {
        private readonly List<Slot> _slots = new();

        public async Task<IEnumerable<Slot>> GetAllSlots()
        {
            return await Task.FromResult(_slots);
        }

        public async Task<IEnumerable<Slot>> GetAvailableSlots()
        {
            return await Task.FromResult(_slots.Where(s => !s.IsReserved));
        }

        public async Task<Slot> AddSlot(Slot slot)
        {
            _slots.Add(slot);
            return await Task.FromResult(slot);
        }

        public async Task<Slot> UpdateSlot(Slot slot)
        {
            var existingSlot = _slots.FirstOrDefault(s => s.Id == slot.Id);
            if (existingSlot != null)
            {
                _slots.Remove(existingSlot);
                _slots.Add(slot);
            }
            return await Task.FromResult(slot);
        }

        public async Task<Slot> GetSlotById(Guid id)
        {
            return await Task.FromResult(_slots.FirstOrDefault(s => s.Id == id));
        }
    }
}
