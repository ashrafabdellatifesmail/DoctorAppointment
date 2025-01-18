using DoctorAppointment.Shared.Interfaces;
using DoctorAppointment.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.AppointmentConfirmation
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationRequest request)
        {
            await _notificationService.SendAppointmentConfirmation(request.Appointment, request.Slot);
            return Ok();
        }
    }

    public class ResendConfirmationRequest
    {
        public AppointmentDto Appointment { get; set; }
        public SlotDto Slot { get; set; }
    }
}
