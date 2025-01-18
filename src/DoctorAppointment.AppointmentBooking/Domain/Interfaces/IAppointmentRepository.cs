

using DoctorAppointment.AppointmentBooking.Domain.Entities;

namespace DoctorAppointment.AppointmentBooking.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> Create(Appointment appointment);
    }
}
