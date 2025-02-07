using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.Data
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAllCars();
        Car? GetCarById(int id);
        void AddCar(Car car);
        void UpdateCar(Car car);
        void DeleteCar(Car car);
    }
}
