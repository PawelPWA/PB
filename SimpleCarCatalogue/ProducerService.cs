using Microsoft.EntityFrameworkCore;
using SimpleCarCatalogue.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCarCatalogue
{
    public class ProducerService: IProducerService
    {
        private readonly ICarContext _carContext;

        public ProducerService(ICarContext carContext)
        {
            _carContext = carContext;
        }

        public List<Producer> GetAll()
        {
            return _carContext.Producers.ToList();
        }
    }
}
