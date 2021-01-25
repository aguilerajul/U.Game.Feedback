using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using U.Game.Feedback.Api.Extensions;
using U.Game.Feedback.Api.Validators;
using U.Game.Feedback.Domain.Contracts;
using U.Game.Feedback.Domain.Entities;
using U.Game.Feedback.Repository;
using U.Game.Feedback.Repository.Implementations;

namespace U.Game.Feedback.Api
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
            services.AddControllers();

            services.AddMvc(op =>
            {
                op.Filters.Add(new ValidationFilter());
            })
            .AddFluentValidation(fvo =>
            {
                fvo.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "U.Game.Feedback.Api",
                        Version = "v1",
                        Description = "U Game Feedback",
                        Contact = new OpenApiContact
                        {
                            Name = "Julio Aguilera",
                            Email = "aguilera.jul@gmail.com",
                            Url = new System.Uri("https://www.linkedin.com/in/julio-c%C3%A9sar-aguilera-tovar-43940919/")
                        }
                    });
            });

            services.AddSingleton(provider => this.Configuration);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            AddRepositoryCollection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(swg =>
            {
                swg.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback API");
            });

            app.ConfigureExceptionHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddRepositoryCollection(IServiceCollection services)
        {
            services.AddDbContext<RepositoryDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Game.Feedback.DataBase")));

            services.AddTransient<IRepositoryBase<User>, UserRepository>();
            services.AddTransient<IRepositoryBase<UserFeedback>, UserFeedbackRepository>();
        }
    }
}
