using Business.DTOs;

namespace Business.Interfaces
{
    public interface IVehicleRepo
    {
        GetVehicleDTO Get(int id);
        IQueryable<GetVehicleDTO> Get(int vehicleType, string location, decimal price);
    }
}
