using Domain;
using Persistence.EF_Repository;
using Persistence.Interfaces;
using Scalar.AspNetCore;


namespace XLITTE_AuthorizationService
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
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v0.0.1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "xlitteAuthorizationServiceAPI",
                    Version = "v0.0.1"
                });
            });


            builder.Services
                .AddSingleton<ApplicationContext>()
                .AddSingleton<UsersRepository>()
                .AddSingleton<ClientApplicationsRepository>();


            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://127.0.0.1:8000")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "/openapi/{documentName}.json";
                });
                app.MapScalarApiReference(opt =>
                {
                    opt
                        .WithTitle("AuthorizationServiceAPI");
                });
            }

            

            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");

            app.MapControllers();

            app.Run();
        }
    }
}
