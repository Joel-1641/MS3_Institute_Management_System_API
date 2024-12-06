using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task<decimal> GetTotalDueAmountAsync(int courseId);
        Task<List<Payment>> GetOverduePaymentsAsync(DateTime currentDate);
        Task<List<Payment>> GetPaymentsAsync();
        Task<Payment> GetPaymentAsync(int studentId, int courseId);


    }


}
