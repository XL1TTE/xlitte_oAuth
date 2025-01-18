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
                .AddSingleton<IEntityRepository<User>, UsersRepository>()
                .AddSingleton<ClientApplicationsRepository>();

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


            app.MapControllers();

            app.Run();
        }
    }
}
