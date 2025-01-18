
using DoctorAppointment.AppointmentBooking.Application.Commands;
using DoctorAppointment.AppointmentBooking.Domain.Entities;
using DoctorAppointment.DoctorAvailability.Repositories;
using DoctorAppointment.Shared.Interfaces;
using DoctorAppointment.Shared.Models;
using MediatR;

namespace DoctorAppointment.AppointmentBooking.Application.Handlers
{
    public class BookAppointmentHandler : IRequestHandler<BookAppointmentCommand, AppointmentDto>
    {
        private readonly Domain.Interfaces.IAppointmentRepository _appointmentRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly INotificationService _notificationService;

        public BookAppointmentHandler(
            Domain.Interfaces.IAppointmentRepository appointmentRepository,
            ISlotRepository slotRepository,
            INotificationService notificationService)
        {
            _appointmentRepository = appointmentRepository;
            _slotRepository = slotRepository;
            _notificationService = notificationService;
        }

        public async Task<AppointmentDto> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Check if slot is available
            var slot = await _slotRepository.GetSlotById(request.SlotId);
            if (slot == null || slot.IsReserved)
            {
                throw new ApplicationException("Slot is not available");
            }

            // Create and save appointment
            var appointment = Appointment.Create(
                request.SlotId,
                request.PatientId,
                request.PatientName
            );

            var savedAppointment = await _appointmentRepository.Create(appointment);

            // Update slot status
            slot.IsReserved = true;
            await _slotRepository.UpdateSlot(slot);

            // Send notification
            var appointmentDto = new AppointmentDto
            {
                Id = savedAppointment.Id,
                SlotId = savedAppointment.SlotId,
                PatientId = savedAppointment.PatientId,
                PatientName = savedAppointment.PatientName,
                ReservedAt = savedAppointment.ReservedAt,
                Status = savedAppointment.Status
            };

            await _notificationService.SendAppointmentConfirmation(appointmentDto, new SlotDto
            {
                Id = slot.Id,
                Time = slot.Time,
                DoctorId = slot.DoctorId,
                DoctorName = slot.DoctorName,
                Cost = slot.Cost,
                IsReserved = slot.IsReserved
            });

            return appointmentDto;
        }
    }
}
