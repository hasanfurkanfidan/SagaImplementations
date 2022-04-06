using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Constants;
using StockApi.Consumers;
using StockApi.Models;

namespace StockApi
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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCreatedEventConsumer>();
                x.AddConsumer<PaymentFailedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint(RabbitMqSettings.StockOrderCreatedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint(RabbitMqSettings.StockPaymentFailedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
                    });
                    cfg.Host(Configuration.GetConnectionString("RabbitMQ"));

                });
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("StockDb");
            });

            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockApi v1"));
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
