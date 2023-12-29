using DataverseWebApis.Services;

namespace DataverseWebApis
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
            services.AddMvc();
            services.AddCors(options =>
            {
                //options.AddPolicy(name: "authenticate",
                //                  builder => {
                //                      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                //                  });
            });
                
            services.AddHttpContextAccessor();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IAccountService, AccountsService>();
        }




    }
}
