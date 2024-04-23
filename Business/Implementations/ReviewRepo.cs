using Business.DTOs;
using Business.Interfaces;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Implementations
{
    public class ReviewRepo : IReviewRepo
    {
        private ApplicationDBContext _context;

        public ReviewRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Add(int userId, AddReviewDTO dto)
        {
            _context.Reviews.Add(new Data.Entities.Review
            {
                UserId = userId,
                Comment = dto.Comment,
                Rating = dto.Rating,
            });
            _context.SaveChanges();
        }

        public List<GetReviewDTO> Get(int userId)
        {
            return _context.Reviews.Include(_ => _.User).Where(_ => _.UserId == userId).Select(_ => new GetReviewDTO
            {     
                User = _.User,
                Comment = _.Comment,
                Rating = _.Rating,
            }).ToList();
        }

        public List<GetReviewDTO> Get()
        {
            return _context.Reviews.Include(_ => _.User).Select(_ => new GetReviewDTO
            {
                User = _.User,
                Comment = _.Comment,
                Rating = _.Rating,
            }).ToList();
        }
    }
}
