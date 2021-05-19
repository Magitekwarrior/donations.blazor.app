using donations.blazor.app.Data.Config;
using donations.blazor.app.Data.Services;
using donations.blazor.app.Data.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System;

namespace donations.blazor.app
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;

      var loggerConfiguration = new LoggerConfiguration()
          .MinimumLevel.Is(LogEventLevel.Information)
          .Enrich.FromLogContext()
          .Enrich.WithExceptionDetails()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
          .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
          .MinimumLevel.Override("System", LogEventLevel.Warning);

      Log.Logger = loggerConfiguration.CreateLogger();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddRazorPages();
      services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);

      services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
      services.Configure<ServicesEndpoints>(Configuration.GetSection("ServicesEndpoints"));
      services.AddHttpClient("Donations", x => { x.BaseAddress = new Uri("http://localhost:5000"); });

      services.AddTransient<IPayfastService, PayfastService>();

      services.AddCors();

      services.AddMudServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Error");
      }

      app.UseCors(Configuration);

      app.UseStaticFiles();

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapBlazorHub();
        endpoints.MapFallbackToPage("/_Host");
      });
    }
  }
}
