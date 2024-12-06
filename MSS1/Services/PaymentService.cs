using MSS1.DTOs.RequestDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly INotificationService _notificationService;

        public PaymentService(IPaymentRepository paymentRepository, INotificationService notificationService)
        {
            _paymentRepository = paymentRepository;
            _notificationService = notificationService;
        }

        public async Task ProcessPaymentAsync(PaymentDTO paymentRequest)
        {
            var payment = await _paymentRepository.GetPaymentAsync(paymentRequest.StudentId, paymentRequest.CourseId);

            if (payment == null)
            {
                // New payment
                payment = new Payment
                {
                    StudentId = paymentRequest.StudentId,
                    CourseId = paymentRequest.CourseId,
                    AmountPaid = paymentRequest.AmountPaid,
                    TotalFee = paymentRequest.TotalFee,
                    DueAmount = paymentRequest.TotalFee - paymentRequest.AmountPaid,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = paymentRequest.PaymentMethod,
                    PaymentStatus = paymentRequest.AmountPaid >= paymentRequest.TotalFee ? "Success" : "Pending"
                };

                await _paymentRepository.AddPaymentAsync(payment);
            }
            else
            {
                // Installment payment
                payment.AmountPaid += paymentRequest.AmountPaid;
                payment.DueAmount = payment.TotalFee - payment.AmountPaid;
                payment.PaymentStatus = payment.DueAmount > 0 ? "Pending" : "Success";

                await _paymentRepository.UpdatePaymentAsync(payment);
            }
        }

        public async Task<decimal> GetTotalDueAmountAsync(int courseId)
        {
            return await _paymentRepository.GetTotalDueAmountAsync(courseId);
        }

        public async Task NotifyOverduePaymentsAsync()
        {
            var overduePayments = await _paymentRepository.GetOverduePaymentsAsync(DateTime.Now);

            foreach (var payment in overduePayments)
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    StudentId = payment.StudentId,
                    Message = $"Your payment for course {payment.Course.CourseName} is overdue. Please settle the amount of {payment.DueAmount}."
                });
            }
        }
    }

}
