using DoctorAppointment.DoctorAvailability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointment.DoctorAvailability.Repositories
{
    public interface ISlotRepository
    {
        Task<IEnumerable<Slot>> GetAllSlots();
        Task<IEnumerable<Slot>> GetAvailableSlots();
        Task<Slot> AddSlot(Slot slot);
        Task<Slot> UpdateSlot(Slot slot);
        Task<Slot> GetSlotById(Guid id);
    }
}
