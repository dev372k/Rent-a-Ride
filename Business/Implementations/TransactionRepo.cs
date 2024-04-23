using Business.DTOs;
using Business.Interfaces;
using Data;
using Data.Commons;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public class TransactionRepo : ITransactionRepo
    {
        private ApplicationDBContext _context;

        public TransactionRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public int Add(CreateTransactionDTO dto)
        {
            var transaction = new Transaction
            {
                Amount = dto.Amount,
                BookingId = dto.BookingId,
                UserId = dto.UserId,
                Status = enPaymentStatus.Unpaid
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return transaction.Id;
        }

        public void Update(int id, enPaymentStatus status)
        {
            var transaction = _context.Transactions.FirstOrDefault(_ => _.Id == id);
            if (transaction != null)
            {
                transaction.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
