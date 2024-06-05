using myTree.MicroService.Platform;
using myTree.Webform.Claim.API;
using Serilog;
using Serilog.Formatting.Compact;

namespace CI.TMS.Claim.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                  .MinimumLevel.Information()
                  .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Routing.EndpointMiddleware", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Authorization.DefaultAuthorizationService", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectHandler", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Serilog.AspNetCore.RequestLoggingMiddleware", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationHandler", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Cors.Infrastructure.CorsService", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.PageActionInvoker", Serilog.Events.LogEventLevel.Error)
                  .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.NewtonsoftJson.NewtonsoftJsonResultExecutor", Serilog.Events.LogEventLevel.Error)
                  .Enrich.FromLogContext()
                  .WriteTo.File(new RenderedCompactJsonFormatter(), path: "logs/log.json", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
                  .CreateLogger();
                try
                {
                    CreateHostBuilder(args).Build().Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Host terminated unexpectedly");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLogging() //<-- THIS WAS MISSING HERE
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
