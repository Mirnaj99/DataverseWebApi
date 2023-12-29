using DataverseWebApis.Services;

namespace DataverseWebApis
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
          
            var services = builder.Services;

            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "authenticate",
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                                  });
            });

            services.AddHttpContextAccessor();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IAccountService, AccountsService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}