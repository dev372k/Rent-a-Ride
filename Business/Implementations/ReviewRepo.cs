using Business.DTOs;
using Business.Interfaces;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Implementations
{
    public class ReviewRepo : IReviewRepo
    {
        private ApplicationDBContext _context;

        public ReviewRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Add(int bookingId, AddReviewDTO dto)
        {
            _context.Reviews.Add(new Data.Entities.Review
            {
                BookingId = bookingId,
                Comment = dto.Comment,
                Rating = dto.Rating,
            });
            _context.SaveChanges();
        }

        public List<GetReviewDTO> Get(int bookingId)
        {
            return _context.Reviews.Include(_ => _.Booking).Where(_ => _.BookingId == bookingId).Select(_ => new GetReviewDTO
            {     
                User = _.Booking.User,
                Comment = _.Comment,
                Rating = _.Rating,
            }).ToList();
        }

        public List<GetReviewDTO> Get()
        {
            return _context.Reviews.Include(_ => _.Booking).Select(_ => new GetReviewDTO
            {
                User = _.Booking.User,
                Comment = _.Comment,
                Rating = _.Rating,
            }).ToList();
        }
    }
}
