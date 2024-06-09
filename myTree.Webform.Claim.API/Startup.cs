using CI.TMS.Claim.API.Extensions;
using myTree.MicroService.Platform;
using System.Globalization;
using CI.TMS.Claim.API.Helper;

namespace CI.TMS.Claim.API
{
    public class Startup
    {
        public IWebHostEnvironment HostEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
            Variable.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            services.AddHttpContextAccessor();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureAppServices();
            services.AddControllersWithViews();
            services.AddSwaggerAuth(Configuration);
            services.AddControllerWithJsonOption();
            services.AddJWTHandler(Configuration);
            services.AddHttpClient();
            services.AddSwaggerGen();
            services.AddAuthorizationIdentityServer(Configuration);
            services.AddBasicHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration["SwaggerUI:RouteURL"], Configuration["SwaggerUI:ApiName"]);
            });
            app.UseHealthChecks();
            app.UseGlobalExceptionHandler(log, Configuration["Middleware:ErrorHandling:ErrorPage"], true);
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers();//.RequireAuthorization("ApiScope");
                    //.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
