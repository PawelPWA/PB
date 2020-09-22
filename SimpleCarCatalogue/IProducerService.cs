using Microsoft.EntityFrameworkCore;
using SimpleCarCatalogue.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCarCatalogue
{
    public interface IProducerService
    {
        List<Producer> GetAll();
    }
}
