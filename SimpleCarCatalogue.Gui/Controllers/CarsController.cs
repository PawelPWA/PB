using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleCarCatalogue.Gui.ViewModels;
using SimpleCarCatalogue.Repositories;

namespace SimpleCarCatalogue.Gui.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarContext _context;
        private readonly ICarService _carService;

        public CarsController(ICarContext context, ICarService carService)
        {
            _context = context;
            _carService = carService;
        }

        // GET: Cars
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/Get
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            return _carService.GetAll(); ;
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            var carSetViewModel = new CarSetViewModel
            {
                Producers = _context.Producers.ToList(),
                Car = new Car(),
            };
            return View(carSetViewModel);
        }

        // POST: Cars/Create  
        [HttpPost]
        public IActionResult Create([FromForm] Car car)
        {
            _carService.Create(car);
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Edit/5
        public IActionResult Edit(Guid id)
        {
            var car = _carService.Get(id);

            var vm = new CarSetViewModel
            {
                Car = car,
                Id = car.Id,
                Producers = _context.Producers.ToList(),
            };
            return View(vm);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Car car)
        {
            _carService.Edit(car);

            return RedirectToAction(nameof(Index));
        }

        // DELETE: Cars/Delete/5
        [HttpDelete("Cars/Delete/{id}")]
        public void Delete(Guid id)
        {
            _carService.Delete(id);
        }
    }
}
