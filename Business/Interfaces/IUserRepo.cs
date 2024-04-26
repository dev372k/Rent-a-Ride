using Business.DTOs;
using Business.Interfaces;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public interface IUserRepo
    {
        GetUserDTO Get(string email);
        void Add(RegisterDTO dto);
        void Update(UpdateUserDTO dto);
        IQueryable<GetUserDTO> Get();
        void UpdateStatus(int userId, bool status);

        (int, int) Count();
    }
}
