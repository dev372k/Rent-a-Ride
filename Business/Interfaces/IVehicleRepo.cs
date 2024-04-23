using Business.DTOs;

namespace Business.Interfaces
{
    public interface IVehicleRepo
    {
        GetVehicleDTO Get(int id);
        List<GetVehicleDTO> Get(string query);
    }
}
