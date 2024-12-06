using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ITDbContext _context;

        public PaymentRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalDueAmountAsync(int courseId)
        {
            return await _context.Payments
                .Where(p => p.CourseId == courseId)
                .SumAsync(p => p.DueAmount);
        }

        public async Task<List<Payment>> GetOverduePaymentsAsync(DateTime currentDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == "Overdue" && p.DueAmount > 0 && p.Course.CourseEndDate <= currentDate)
                .ToListAsync();
        }
        public async Task<List<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.ToListAsync(); // Ensure the 'await' keyword is used
        }
        public async Task<Payment> GetPaymentAsync(int studentId, int courseId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.CourseId == courseId);
        }
    }

}
