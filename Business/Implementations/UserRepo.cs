using Business.DTOs;
using Data;
using Data.Entities;
using Data.Helpers;

namespace Business.Implementations
{
    public class UserRepo : IUserRepo
    {
        private ApplicationDBContext _context;

        public UserRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public void Add(RegisterDTO dto)
        {
            var userExist = Get(dto.Email);
            if (userExist != null)
                throw new Exception("User already exist with this email");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                Country = dto.Country,
                Password = SecurityHelper.GenerateHash(dto.Password)
            };
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public GetUserDTO Get(string email)
        {
            GetUserDTO? user = _context.Users.Where(_ => _.Email.ToLower().Equals(email.ToLower())).Select(_ => new GetUserDTO
            {
                Id = _.Id,
                Email = _.Email,
                Name = _.Name,
                Password = _.Password,
                Role = _.Role
            }).FirstOrDefault();
            return user;
        }

        public void Update(UpdateUserDTO dto)
        {
            var user = _context.Users.FirstOrDefault(_ => _.Id == dto.Id);
            if (user != null)
            {
                user.Name = dto.Name;
                user.Address = dto.Address;
                user.City = dto.City;
                user.Country = dto.Country;

                _context.Users.Update(user);
                _context.SaveChanges();
            }

        }
    }
}
