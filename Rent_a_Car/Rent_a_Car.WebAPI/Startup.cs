using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Rent_a_Car.WebAPI.Database;
//using Rent_a_Car.WebAPI.Filters;
using Rent_a_Car.WebAPI.Interface;
using Rent_a_Car.WebAPI.Service;
using RentaCar.Data.Models;
using RentaCar.Data.Requests;
using RentaCar.Data.Requests.Booking;
using RentaCar.Data.Requests.Branch;
using RentaCar.Data.Requests.City;
using RentaCar.Data.Requests.Comments;
using RentaCar.Data.Requests.Customer;
using RentaCar.Data.Requests.Fuel_type;
using RentaCar.Data.Requests.Manufacturer;
using RentaCar.Data.Requests.Rating;
using RentaCar.Data.Requests.Vehicle;
using RentaCar.Data.Requests.VehicleModel;
using RentaCar.Data.Requests.VehicleType;
using RentACar.WebAPI.Interface;
using RentACar.WebAPI.Security;
using RentACar.WebAPI.Service;

namespace Rent_a_Car.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc(x => x.Filters.Add<ErrorFilter>());         // Filter for errors

            services.AddControllers();

            services.AddDbContext<RentaCarContext>(c => c.UseSqlServer(Configuration.GetConnectionString("RentaCarCS")));       // Connection string

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RentACar API", Version = "v1" });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);


            services.AddAutoMapper(typeof(Startup));                //Automapper configuration

            #region Dependency injection

            services.AddScoped<IService<FuelTypeRequest,object>,BaseService<FuelTypeRequest, object,Database.FuelType>>();
            services.AddScoped<IService<VehicleTypeRequest, object>, BaseService<VehicleTypeRequest, object, Database.VehicleType>>();
            services.AddScoped<IService<ManufacturerRequest, object>, BaseService<ManufacturerRequest, object,Database.Manufacturer>>();
            services.AddScoped<IService<VehicleModelRequest, VehicleModelSearch>, VehicleModelService>();
            services.AddScoped<IService<CityRequest,CitySearchRequest>,CityService>();


            services.AddScoped<ICRUDService<BranchRequest,BranchSearchRequest,BranchUpsert,BranchUpsert> , BranchService>();
            services.AddScoped<ICRUDService<VehicleRequest, VehicleSearchRequest, VehicleUpsert, VehicleUpsert>, VehicleService>();
            services.AddScoped<ICRUDService<BookingRequest, BookingSearchRequest, BookingUpsert, BookingUpsert>,BookingService>();
            services.AddScoped<ICRUDService<RatingRequest,RatingSearchRequest,RatingUpsert,RatingUpsert >, RatingService > ();
            

            services.AddScoped<ICRUDService<MComment, CommentSearchRequest, CommentUpsert, CommentUpsert>,CommentService>();

            services.AddScoped<ICustomerService,CustomerService>();
            
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
