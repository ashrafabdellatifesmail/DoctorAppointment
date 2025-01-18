using DoctorAppointment.DoctorAvailability.Interfaces;
using DoctorAppointment.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorAppointment.DoctorAvailability.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlotController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SlotDto>>> GetAllSlots()
        {
            var slots = await _slotService.GetAllSlots();
            return Ok(slots);
        }

        [HttpPost]
        public async Task<ActionResult<SlotDto>> AddSlot(SlotDto slotDto)
        {
            var result = await _slotService.AddSlot(slotDto);
            return CreatedAtAction(nameof(GetAllSlots), new { id = result.Id }, result);
        }
    }
}
