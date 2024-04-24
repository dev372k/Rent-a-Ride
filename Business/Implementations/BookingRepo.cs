using Business.DTOs;
using Business.Interfaces;
using Data;
using Data.Commons;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Implementations
{
    public class BookingRepo : IBookingRepo
    {
        private ApplicationDBContext _context;

        public BookingRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public GetBookingDTO Add(CreateBookingDTO dto)
        {
            var booking = new Booking
            {
                From = dto.From,
                To = dto.To,
                Purpose = dto.Purpose,
                UserId = dto.UserId,
                VehicleId = dto.VehicleId,
                Status = enPaymentStatus.Unpaid,
                PaymentIntent = "no payment intent"
            };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return new GetBookingDTO
            {
                Id = booking.Id,
                From = dto.From,
                To = dto.To,
                Purpose = dto.Purpose,
            };
        }

        public void Delete(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(_ => _.Id == id);
            if (booking != null)
            {
                booking.IsDeleted = true;
                _context.SaveChanges();
            }
        }

        public IQueryable<GetBookingDTO> Get(int userId)
        {
            var bookings = _context.Bookings.Include(_ => _.User).Include(_ => _.Vehicle).Where(_ => (userId !=0 ? _.UserId == userId : true)).Select(_ => new GetBookingDTO
            {
                Id = _.Id,
                From = _.From,
                To = _.To,
                Purpose = _.Purpose,
                User = new GetUserDTO
                {
                    Name = _.User.Name,
                    Email = _.User.Email,
                },
                Vehicle = new GetVehicleDTO
                {
                    Name = _.Vehicle.Name,
                    Image = _.Vehicle.Image,
                    Model = _.Vehicle.Model,
                    Location = _.Vehicle.Location,
                    Price = _.Vehicle.Price,
                    Type = _.Vehicle.Type,
                    Year = _.Vehicle.Year,
                    Color = _.Vehicle.Color,
                },
                IsReview = _context.Reviews.Any(_ => _.BookingId == _.Id) ? false : true,
                Status = ((enPaymentStatus)_.Status).ToString()
            });

            return bookings;
        }

        public void Update(int id, enPaymentStatus status, string paymentIntent)
        {
            var booking = _context.Bookings.FirstOrDefault(_ => _.Id == id);
            if (booking != null)
            {
                booking.PaymentIntent = paymentIntent;
                booking.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
