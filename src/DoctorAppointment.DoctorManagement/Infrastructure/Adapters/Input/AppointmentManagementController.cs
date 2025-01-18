using DoctorAppointment.DoctorManagement.Application.Ports.Input;
using DoctorAppointment.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.DoctorManagement.Infrastructure.Adapters.Input
{
    [ApiController]
    [Route("api/doctor/appointments")]
    public class AppointmentManagementController : ControllerBase
    {
        private readonly IAppointmentManagementUseCase _appointmentManagement;

        public AppointmentManagementController(IAppointmentManagementUseCase appointmentManagement)
        {
            _appointmentManagement = appointmentManagement;
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetUpcomingAppointments()
        {
            var appointments = await _appointmentManagement.GetUpcomingAppointments();
            return Ok(appointments);
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult<AppointmentDto>> MarkAsCompleted(Guid id)
        {
            var result = await _appointmentManagement.MarkAppointmentAsCompleted(id);
            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult<AppointmentDto>> CancelAppointment(Guid id)
        {
            var result = await _appointmentManagement.CancelAppointment(id);
            return Ok(result);
        }
    }
}
