using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCarCatalogue.Gui.ViewModels
{
    public class CarSetViewModel
    {
        public Guid? Id { get; set; }
        public List<Producer> Producers { get; set; }
        public Car Car { get; set; }
    }
}
