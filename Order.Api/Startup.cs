using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Order.Api.Consumers;
using Order.Api.Models;
using Shared.Constants;

namespace Order.Api
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
                x.AddConsumer<OrderRequestCompletedEventConsumer>();
                x.AddConsumer<OrderStockNotReservedEventConsumer>();
                x.AddConsumer<PaymentFailedRequestEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration.GetConnectionString("RabbitMQ"));

                    cfg.ReceiveEndpoint(RabbitMqSettings.OrderRequestCompletedEventtQueueName, x =>
                    {
                        x.ConfigureConsumer<OrderRequestCompletedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(RabbitMqSettings.OrderStockNotReservedRequestQueueName, x =>
                    {
                        x.ConfigureConsumer<OrderStockNotReservedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(RabbitMqSettings.OrderPaymentFailedRequestQueue, x =>
                    {
                        x.ConfigureConsumer<PaymentFailedRequestEventConsumer>(context);
                    });
                });
            });

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlCon"));
            });

            services.AddMassTransitHostedService();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order.Api v1"));
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
