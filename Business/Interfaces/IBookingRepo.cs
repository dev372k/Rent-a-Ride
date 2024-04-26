using Business.DTOs;
using Data.Commons;

namespace Business.Interfaces
{
    public interface IBookingRepo
    {
        GetBookingDTO Add(CreateBookingDTO dto);
        IQueryable<GetBookingDTO> Get(int userId);
        void Update(int id, enPaymentStatus status, string paymentIntent);
        void Delete(int id);
        (int, int, string) Count();
        (int, int, double) UserBookingCount(int userId);
    }
}
