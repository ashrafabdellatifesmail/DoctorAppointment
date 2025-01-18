using DoctorAppointment.Shared.Models;

namespace DoctorAppointment.Shared.Interfaces
{
    public interface INotificationService
    {
        Task SendAppointmentConfirmation(AppointmentDto appointment, SlotDto slot);
    }
}
