using Microsoft.EntityFrameworkCore;
using SimpleCarCatalogue.Exceptions;
using SimpleCarCatalogue.Repositories;
using SimpleCarCatalogue.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCarCatalogue
{
    public class CarService : ICarService
    {
        private readonly ICarContext _carContext;
        private IValidator<Car> _carValidator;

        public CarService(ICarContext carContext, IValidator<Car> carValidator)
        {
            _carContext = carContext;
            _carValidator = carValidator;
        }

        public List<Car> GetAll()
        {
            return _carContext.Cars.Include(u => u.Producer).OrderBy(n => n.Year).ToList();
        }

        public void Create(Car car)
        {
            if (car == null)
                throw new ArgumentNullException(nameof(car));

            var isValid = _carValidator.Validate(car);
            if (!isValid)
                throw new ArgumentException(string.Join(',', _carValidator.ValidationMessages));

            _carContext.Cars.Add(car);
            _carContext.SaveChanges();
        }

        public Car Get(Guid id)
        {
            var car = _carContext.Cars.FirstOrDefault(n=>n.Id == id);
            if (car == null)
                throw new ObjectNotExistsException("Car does not exists");

            return car;
        }

        public void Delete(Guid id)
        {
            var car = Get(id);

            _carContext.Cars.Remove(car);
            _carContext.SaveChanges();
        }

        public void Edit(Car car)
        {
            Get(car.Id);

            var isValid = _carValidator.Validate(car);
            if (!isValid)
                throw new ArgumentException(string.Join(',', _carValidator.ValidationMessages));

            _carContext.Cars.Update(car);
            _carContext.SaveChanges();
        }
    }
}
