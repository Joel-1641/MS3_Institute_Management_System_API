using MSS1.DTOs.RequestDTOs;

namespace MSS1.Interfaces
{
    public interface IPaymentService
    {
        Task ProcessPaymentAsync(PaymentDTO paymentRequest);
        Task<decimal> GetTotalDueAmountAsync(int courseId);
        Task NotifyOverduePaymentsAsync();
        //Task ProcessPaymentAsync(PaymentDTO paymentRequest);


    }

}
