using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SimpleCarCatalogue.Exceptions;
using SimpleCarCatalogue.Repositories;
using SimpleCarCatalogue.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SimpleCarCatalogue.UnitTests
{
    public class CarServiceTests
    {
        private readonly CarService _carService;
        private readonly IValidator<Car> _validator;
        private readonly ICarContext _carContext;

        public CarServiceTests()
        {
            _validator = Substitute.For<IValidator<Car>>();
            _carContext = Substitute.For<ICarContext>();

            _carService = new CarService(_carContext, _validator);
        }

        [Fact]
        public void GetAll_ReturnsAllCarsOrderedByYear()
        {
            var carsList = new List<Car>
            {
                new Car
                {
                    Year = 2000,
                },
                new Car
                {
                    Year = 1900,
                },
                new Car
                {
                    Year = 1995,
                }
            };

            var mockSet = ToDbSet(carsList);

            _carContext.Cars.Returns(mockSet);

            var allCars = _carService.GetAll();

            var expected = new List<Car>
            {
                carsList[1], carsList[2], carsList[0]
            };

            Assert.Equal(expected, allCars);
        }

        [Fact]
        public void Create_ExectuesSaveChangesMethod_IfParameterIsValid()
        {
            var car = new Car
            {
                Name = "Giulia",
                Year = 2015,
                ProducerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            };

            _validator.Validate(car).Returns(true);
            _carService.Create(car);

            _carContext.Received().SaveChanges();
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_IfParameterIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _carService.Create(null));
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_IfParameterIsInvalid()
        {
            var car = new Car
            {
            };

            _validator.Validate(car).Returns(false);
            Assert.Throws<ArgumentException>(() => _carService.Create(car));
        }

        [Fact]
        public void Get_ThrowsObjectNotExistsException_IfIdIsInvalid()
        {
            var mockSet = ToDbSet(new List<Car>());
            _carContext.Cars.Returns(mockSet);

            var outOfRangeId = Guid.Parse("99999999-9999-9999-9999-999999999999");

            Assert.Throws<ObjectNotExistsException>(() => _carService.Get(outOfRangeId));
        }

        [Fact]
        public void Get_ReturnsItem_IfIdIsCorrect()
        {
            var carsList = new List<Car>
            {
                new Car
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Year = 2000,
                },
                new Car
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Year = 1900,
                },
                new Car
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Year = 1995,
                }
            };

            var mockSet = ToDbSet(carsList);
            _carContext.Cars.Returns(mockSet);

            var car = _carService.Get(Guid.Parse("00000000-0000-0000-0000-000000000002"));

            var expected = carsList[1];

            Assert.Equal(expected, car);
        }

        [Fact]
        public void Edit_ThrowsObjectNotExistsException_IfIdIsInvalid()
        {
            var mockSet = ToDbSet(new List<Car>());
            _carContext.Cars.Returns(mockSet);

            var outOfRangeId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var car = new Car
            {
                Id = outOfRangeId
            };
            Assert.Throws<ObjectNotExistsException>(() => _carService.Edit(car));
        }

        [Fact]
        public void Edit_ThrowsArgumentException_IfParameterIsInvalid()
        {
            var car = new Car
            {
            };
            _validator.Validate(car).Returns(false);

            var mockSet = ToDbSet(new List<Car>());
            _carContext.Cars.Returns(mockSet);

            Assert.Throws<ObjectNotExistsException>(() => _carService.Edit(car));
        }

        private DbSet<Car> ToDbSet(List<Car> carsList)
        {
            var mockCars = carsList.AsQueryable();

            var mockSet = Substitute.For<DbSet<Car>, IQueryable<Car>>();
            ((IQueryable<Car>)mockSet).Provider.Returns(mockCars.Provider);
            ((IQueryable<Car>)mockSet).Expression.Returns(mockCars.Expression);
            ((IQueryable<Car>)mockSet).ElementType.Returns(mockCars.ElementType);
            ((IQueryable<Car>)mockSet).GetEnumerator().Returns(mockCars.GetEnumerator());

            return mockSet;
        }
    }
}
