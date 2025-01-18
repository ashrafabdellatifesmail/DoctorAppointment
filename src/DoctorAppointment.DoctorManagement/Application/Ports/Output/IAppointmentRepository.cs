
using DoctorAppointment.DoctorManagement.Domain;

namespace DoctorAppointment.DoctorManagement.Application.Ports.Output
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetUpcomingAppointments();
        Task<Appointment> GetById(Guid id);
        Task<Appointment> Update(Appointment appointment);
    }
}
