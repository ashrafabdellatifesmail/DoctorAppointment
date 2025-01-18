
using DoctorAppointment.Shared.Models;
using MediatR;

namespace DoctorAppointment.AppointmentBooking.Application.Commands
{
    public class BookAppointmentCommand:IRequest<AppointmentDto>
    {
        public BookAppointmentCommand(Guid slotId, Guid patientId, string patientName)
        {
            SlotId = slotId;
            PatientId = patientId;
            PatientName = patientName;
        }

        public Guid SlotId { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
    }
      
   
}
