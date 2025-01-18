using DoctorAppointment.AppointmentBooking.Application.Commands;
using DoctorAppointment.DoctorAvailability.Repositories;
using DoctorAppointment.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.AppointmentBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISlotRepository _slotRepository;

        public BookingController(IMediator mediator, ISlotRepository slotRepository)
        {
            _mediator = mediator;
            _slotRepository = slotRepository;
        }

        [HttpGet("available-slots")]
        public async Task<ActionResult<IEnumerable<SlotDto>>> GetAvailableSlots()
        {
            var slots = await _slotRepository.GetAvailableSlots();
            return Ok(slots);
        }

        [HttpPost("book")]
        public async Task<ActionResult<AppointmentDto>> BookAppointment(BookAppointmentCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
