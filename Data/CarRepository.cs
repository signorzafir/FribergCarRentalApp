using FribergCarRentalApp.Models;

namespace FribergCarRentalApp.Data
{
    public class CarRepository : ICarRepository
    {
        private readonly RentalAppDbContext rentalAppDbContext;

        public CarRepository(RentalAppDbContext rentalAppDbContext)
        {
            this.rentalAppDbContext = rentalAppDbContext;
        }
        public void AddCar(Car car)
        {
            rentalAppDbContext.Cars.Add(car);
            rentalAppDbContext.SaveChanges();
        }

        public void DeleteCar(Car car)
        {
            rentalAppDbContext.Cars.Remove(car);
            rentalAppDbContext.SaveChanges();
        }


        public Car? GetCarById(int id)
        {
             return rentalAppDbContext.Cars.Find(id);            
        }

        public void UpdateCar(Car car)
        {
            rentalAppDbContext.Cars.Update(car);
            rentalAppDbContext.SaveChanges();
        }

        IQueryable<Car> ICarRepository.GetAllCars()
        {
            return rentalAppDbContext.Cars.OrderBy(c => c.Make).ThenBy(c=>c.Model);
        }
    }
}
