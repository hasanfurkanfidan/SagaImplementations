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
                x.AddConsumer<PaymentCompletedEventConsumer>();
                x.AddConsumer<PaymentFailedEventConsumer>();
                x.AddConsumer<StockNotReservedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ReceiveEndpoint(RabbitMqSettings.OrderPaymentCompletedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(RabbitMqSettings.OrderPaymentFailedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(RabbitMqSettings.OrderStockNotReservedEventQueueName, e =>
                    {
                        e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
                    });

                    cfg.Host(Configuration.GetConnectionString("RabbitMQ"));
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
