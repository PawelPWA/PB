using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SimpleCarCatalogue.Repositories;
using SimpleCarCatalogue.Validators;

namespace SimpleCarCatalogue.Gui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<CarContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CarsDatabase"))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddTransient<ICarContext, CarContext>();
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IProducerService, ProducerService>();
            services.AddTransient<IValidator<Car>, CarValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Errors/Error");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new
                    {
                        controller = "Cars",
                        action = "Index"
                    });
            });
        }
    }
}
