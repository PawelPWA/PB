using System;
using System.Collections.Generic;

namespace SimpleCarCatalogue
{
    public interface ICarService
    {
        List<Car> GetAll();
        void Create(Car car);
        Car Get(Guid id);
        void Delete(Guid id);
        void Edit(Car car);
    }
}
