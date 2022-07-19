using Sabio.Models;
using Sabio.Models.Domain.Automobiles;
using Sabio.Models.Requests.Automobiles;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IAutomobileService
    {
        Automobile GetCar(int id);
        List<Automobile> GetAllCars();
        int AddCar(AutomobileAddRequest model, int currentUser);

        Paged<Automobile> GetAllPagination(int pageIndex, int pageSize);

        Paged<Automobile> SearchPagination(int pageIndex, int pageSize, string query);

        public void Update(AutomobileUpdateRequest model);
    }
}