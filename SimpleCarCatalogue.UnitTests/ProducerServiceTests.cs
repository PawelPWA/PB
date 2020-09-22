using Microsoft.EntityFrameworkCore;
using NSubstitute;
using SimpleCarCatalogue.Repositories;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SimpleCarCatalogue.UnitTests
{
    public class ProducerServiceTests
    {
        private readonly ProducerService _producerService;
        private readonly ICarContext _carContext;

        public ProducerServiceTests()
        {
            _carContext = Substitute.For<ICarContext>();

            _producerService = new ProducerService(_carContext);
        }

        [Fact]
        public void GetAll_ReturnsAllProducers()
        {
            var producersList = new List<Producer>
            {
                new Producer
                {
                   Name = "Alfa Romeo"
                },
                new Producer
                {
                   Name = "audi"
                }
            };

            var mockSet = ToDbSet(producersList);

            _carContext.Producers.Returns(mockSet);

            var allCars = _producerService.GetAll();

            var expected = producersList;
            Assert.Equal(expected, allCars);
        }

        private DbSet<Producer> ToDbSet(List<Producer> producersList)
        {
            var mockProducers = producersList.AsQueryable();

            var mockSet = Substitute.For<DbSet<Producer>, IQueryable<Producer>>();
            ((IQueryable<Producer>)mockSet).Provider.Returns(mockProducers.Provider);
            ((IQueryable<Producer>)mockSet).Expression.Returns(mockProducers.Expression);
            ((IQueryable<Producer>)mockSet).ElementType.Returns(mockProducers.ElementType);
            ((IQueryable<Producer>)mockSet).GetEnumerator().Returns(mockProducers.GetEnumerator());

            return mockSet;
        }
    }
}
