using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleCarCatalogue.Gui.Controllers;
using SimpleCarCatalogue.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleCarCatalogue.Gui.IntegrationTests
{
    public class CarControllerTests : IDisposable
    {
        CarContext _context;

        HttpClient _client;

        public CarControllerTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<CarContext>();

            builder.UseSqlServer($"Server=.\\SqlExpress;Database=CarCatalogue;Trusted_Connection=True;")
                    .UseInternalServiceProvider(serviceProvider);

            _context = new CarContext(builder.Options);
            _context.Database.Migrate();

            InitDb();
            BuildApplication();
        }

        [Fact]
        public async Task Get_ReturnsAllCars()
        {
            var cars = await GetCars();

            Assert.Equal(3, cars.Count);
        }

        [Fact]
        public async Task Create_AddsNewCarToDb()
        {
            var producerId = _context.Producers.First(n => n.Name == "Alfa Romeo").Id;

            var cars = await GetCars();
            var initialNoCars = cars.Count();

            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("Name", "147"));
            formVariables.Add(new KeyValuePair<string, string>("Year", "2003"));
            formVariables.Add(new KeyValuePair<string, string>("ProducerId", producerId.ToString()));

            var formContent = new FormUrlEncodedContent(formVariables);

            var result = await _client.PostAsync("cars/create", formContent);

            cars = await GetCars();

            Assert.Equal(initialNoCars + 1, cars.Count);
        }

        [Fact]
        public async Task Edit_UpdatesCarInDb()
        {
            var producerId = _context.Producers.First(n => n.Name == "Alfa Romeo").Id;
            var cars = await GetCars();
            var carToEdit = cars.First();
            carToEdit.Description = "Modified description";
            

            var formVariables = new List<KeyValuePair<string, string>>();
            formVariables.Add(new KeyValuePair<string, string>("Id", carToEdit.Id.ToString()));
            formVariables.Add(new KeyValuePair<string, string>("Name", carToEdit.Name));
            formVariables.Add(new KeyValuePair<string, string>("Year", carToEdit.Year.ToString()));
            formVariables.Add(new KeyValuePair<string, string>("ProducerId", carToEdit.ProducerId.ToString()));
            formVariables.Add(new KeyValuePair<string, string>("Description", carToEdit.Description));

            var formContent = new FormUrlEncodedContent(formVariables);

            var result = await _client.PostAsync("cars/edit", formContent);

            cars = await GetCars();
            var editedCar = cars.First(n => n.Id == carToEdit.Id);
            Assert.Equal("Modified description", editedCar.Description);
        }

        [Fact]
        public async Task Delete_DeletesCar()
        {
            var cars = await GetCars();
            var beginCarsCount = cars.Count();

            await _client.DeleteAsync($"cars/Delete/{cars.First().Id}");

            cars = await GetCars();

            Assert.Equal(beginCarsCount - 1, cars.Count);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

        private void InitDb()
        {
            _context.Cars.Add(new Car
            {
                Name = "Giulia",
                Year = 2015,
                ProducerId = _context.Producers.First(n => n.Name == "Alfa Romeo").Id
            });
            _context.Cars.Add(new Car
            {
                Name = "Stelvio",
                Year = 2017,
                ProducerId = _context.Producers.First(n => n.Name == "Alfa Romeo").Id
            });
            _context.Cars.Add(new Car
            {
                Name = "GT",
                Year = 2007,
                ProducerId = _context.Producers.First(n => n.Name == "Alfa Romeo").Id
            });
            _context.SaveChanges();
        }

        private void BuildApplication()
        {
            var server = new TestServer(new WebHostBuilder()
                  .UseEnvironment("Testing")
                  .UseConfiguration(new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      //.AddJsonFile("appsettings.Testing.json")
                      .Build()
                  )
                  .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        private async Task<List<Car>> GetCars()
        {
            var response = await _client.GetAsync("Cars/Get");
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Car>>(json);
        }
    }
}
