using DoctorAppointment.Shared.Models;

namespace DoctorAppointment.DoctorManagement.Application.Ports.Input
{
    public interface IAppointmentManagementUseCase
    {
        Task<IEnumerable<AppointmentDto>> GetUpcomingAppointments();
        Task<AppointmentDto> MarkAppointmentAsCompleted(Guid appointmentId);
        Task<AppointmentDto> CancelAppointment(Guid appointmentId);
    }
}
