using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using MassTransit.Definition;
using PS.Notification.Api.Consumers;
using Microsoft.Extensions.Configuration;
using PS.Notification.Abstractions.Settings;
using PS.Notification.Api.Settings;
using PS.Notification.Api.Interfaces;
using PS.Notification.Api.Services;

namespace PS.Notification.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            var rabbitMqSettings = Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();
            services.Configure<MailSettings>(options => Configuration.GetSection("MailSettings").Bind(options));
            services.AddScoped<IEMailService, SmtpMailService>();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<MailConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, configurator =>
                    {
                        configurator.Username(rabbitMqSettings.UserName);
                        configurator.Password(rabbitMqSettings.Password);
                    });

                    cfg.ClearMessageDeserializers();
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance);

                    cfg.ReceiveEndpoint("ps-mail-queue", e =>
                        {
                            e.ConfigureConsumer<MailConsumer>(context);
                        });
                });
            });



            services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("PS Notification API!");
                });
            });
        }
    }
}
