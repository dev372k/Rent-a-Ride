using Business.DTOs;
using Data.Commons;

namespace Business.Interfaces
{
    public interface IVehicleRepo
    {
        GetVehicleDTO Get(int id);
        IQueryable<GetVehicleDTO> Get(int vehicleType, string location, decimal price);
        Task Add(CreateVehicleDTO createVehicleDTO);
        void Delete(int id);
    }
}
