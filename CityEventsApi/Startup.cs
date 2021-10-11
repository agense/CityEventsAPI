using Business.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Data.Repos;
using Data;
using EventsApi.Mappers;
using Data.Interfaces;
using Data.Mappers;
using Business;
using Business.Services;
using Business.Interfaces.Log;
using EventsApi.Interfaces;
using EventsApi.Validators;

namespace EventsApi
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
            //DB Config
            services.AddDbContext<EventsContext>();

            //Dependency Injection
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IEventsRepository, EventsRepository>();

            services.AddTransient<EventsApi.Interfaces.IEventRequestMapper, EventRequestMapper>();
            services.AddTransient<EventsApi.Interfaces.IEventResponseMapper, EventResponseMapper>();
            services.AddTransient<EventsApi.Interfaces.ICategoryRequestMapper, CategoryRequestMapper>();
            services.AddTransient<EventsApi.Interfaces.ICategoryResponseMapper, CategoryResponseMapper>();
            services.AddTransient<EventsApi.Interfaces.ICategoryInEventRequestMapper, CategoryInEventRequestMapper>();

            services.AddTransient<ILog, FileLog>();
            services.AddTransient<IErrorLog, ErrorLog>();

            services.AddTransient<IEventRequestValidator, EventRequestValidator>();
            services.AddTransient<IDateValidator, DateValidator>();
            
            services.AddTransient<ICategoryEntityMapper, CategoryEntityMapper>();
            services.AddTransient<ICategoryEntityWithEventsMapper, CategoryEntityWithEventsMapper>();
            services.AddTransient<IEventEntityMapper, EventEntityMapper>();

            //Options
            services.Configure<LoggingOptions>(Configuration.GetSection("FileLog"));

            //Cors Configurations
            services.AddCors(options => options.AddDefaultPolicy(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            ));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "City Events Api", 
                    Version = "v1",
                    Description = "An API for creating and viewing events happenning in the city."
                
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EventsContext context)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                context.Database.EnsureCreated();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventsApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
