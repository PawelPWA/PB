using NSubstitute;
using SimpleCarCatalogue.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SimpleCarCatalogue.UnitTests
{
    public class CarValidatorTests
    {
        private IValidator<Car> _carValidator;
        private List<Producer> _producers = new List<Producer>
            {
                new Producer
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Alfa Romeo",
                },
                 new Producer
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "audi",
                },
                new Producer
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Name = "vw",
                },
        };

        public CarValidatorTests()
        {
            var producerService = Substitute.For<IProducerService>();
            producerService.GetAll().Returns(_producers);

            _carValidator = new CarValidator(producerService);
        }

        [Theory]
        [InlineData("Giulia", 2015, "Alfa Romeo")]
        [InlineData("Stelvio", 2020, "Alfa Romeo")]
        [InlineData("GT", 1900, "Alfa Romeo")]
        public void Validate_ReturnsTrue_IfModelIsValid(string name, int year, string producerName)
        {
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.True(result);
        }

        [Theory]
        [InlineData("Giulia", 2015, "Alfa Romeo")]
        public void Validate_ClearsErrorMessages_IfModelIsValid(string name, int year, string producerName)
        {
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null, 2015, "Alfa Romeo")]
        public void Validate_ReturnsFalse_IfNameIsNull(string name, int year, string producerName)
        {
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.False(result);
        }

        [Theory]
        [InlineData("", 2015, "Alfa Romeo")]
        public void Validate_ReturnsFalse_IfNameHasLessThen1Char(string name, int year, string producerName)
        {
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.False(result);
        }

        [Theory]
        [InlineData(257, 2015, "Alfa Romeo")]
        public void Validate_ReturnsFalse_IfNameHasMoreThen256Chars(int numberOfChars, int year, string producerName)
        {
            var name = string.Join("", Enumerable.Range(0, numberOfChars).Select(n => "n"));
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.False(result);
        }

        [Theory]
        [InlineData("Giulia", 1899, "Alfa Romeo")]
        public void Validate_ReturnsFalse_IfYearIsLowerThen1900(string name, int year, string producerName)
        {
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.False(result);
        }

        [Theory]
        [InlineData("Giulia", 2021, "Alfa Romeo")]
        public void Validate_ReturnsFalse_IfYearIsBiggetThen2020(string name, int year, string producerName)
        {
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = GetProducerId(producerName),
            };

            var result = _carValidator.Validate(car);

            Assert.False(result);
        }

        [Theory]
        [InlineData("Giulia", 2021)]
        public void Validate_ReturnsFalse_IfProducerIdIsOutOfRange(string name, int year)
        {
            var outOfRangeId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var car = new Car
            {
                Name = name,
                Year = year,
                ProducerId = outOfRangeId,
            };

            var result = _carValidator.Validate(car);

            Assert.False(result);
        }

        [Theory]
        [InlineData(null, "Name can not be null")]
        [InlineData("", "Name lenght has to bo in [1, 255] range")]
        public void Validate_SetMessage_IfNameIsInvalid(string name, string expectedMessage)
        {
            var car = new Car
            {
                Name = name,
                Year = 2015,
                ProducerId = GetProducerId("Alfa Romeo"),
            };

            var result = _carValidator.Validate(car);

            Assert.Equal(expectedMessage, _carValidator.ValidationMessages.First());
        }

        [Theory]
        [InlineData(1899, "Year must be greater than 1900")]
        [InlineData(3000, "Year must be lower than 2020")]
        public void Validate_SetMessage_IfYearIsInvalid(int year, string expectedMessage)
        {
            var car = new Car
            {
                Name = "Giulia",
                Year = year,
                ProducerId = GetProducerId("Alfa Romeo"),
            };

            var result = _carValidator.Validate(car);

            Assert.Equal(expectedMessage, _carValidator.ValidationMessages.First());
        }

        [Fact]
        public void Validate_SetMessage_IfProducerIdIsInvalid()
        {
            var outOfRangeId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            var car = new Car
            {
                Name = "Giulia",
                Year = 2015,
                ProducerId = outOfRangeId,
            };

            var result = _carValidator.Validate(car);
            const string expectedMessage = "ProducerId is out of range";
            Assert.Equal(expectedMessage, _carValidator.ValidationMessages.First());
        }

        private Guid GetProducerId(string name)
        {
            return _producers.First(n => n.Name == name).Id;
        }
    }
}
