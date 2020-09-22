using NSubstitute;
using SimpleCarCatalogue.Gui.Controllers;
using SimpleCarCatalogue.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleCarCatalogue.Gui.UnitTests
{
 public   class CarsControllerTests
    {
        private readonly CarsController _controller;
        private readonly ICarContext _carContext;
        private readonly ICarService _carService;

        public CarsControllerTests()
        {
            _carContext = Substitute.For<ICarContext>();
            _carService = Substitute.For<ICarService>();

            _controller = new CarsController(_carContext, _carService) ;
        }

        [Fact]
        public void Get_ReturnsAllCars()
        {
            var carsList = new List<Car>();
            _carService.GetAll().Returns(carsList);

            var returnedCarsList = _controller.Get();

            Assert.Equal(carsList, returnedCarsList);
        }

        [Fact]
        public void Create_ExecutesCreateMethod()
        {
            var car = new Car();

           _controller.Create(car);

            _carService.Received().Create(car);
        }

        [Fact]
        public void Edit_ExecutesEditMethod()
        {
            var car = new Car();

            _controller.Edit(car.Id, car);

            _carService.Received().Edit(car);
        }

        [Fact]
        public void Delete_ExecutesEditMethod()
        {
            var id = new Guid();

            _controller.Delete(id);

            _carService.Received().Delete(id);
        }
    }
}
