using Business.DTOs;
using Data.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ITransactionRepo
    {
        int Add(CreateTransactionDTO dto);
        void Update(int id, enPaymentStatus status);

    }
}
