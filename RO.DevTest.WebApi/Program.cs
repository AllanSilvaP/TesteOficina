using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application;
using RO.DevTest.Application.Interfaces;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence;
using RO.DevTest.Persistence.IoC;
using RO.DevTest.Persistence.Repositories;

namespace RO.DevTest.WebApi;

public class Program {
    public static void Main(string[] args) {

        var builder = WebApplication.CreateBuilder(args);
        //CONFIG DB
        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        //REPOSITORYS
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ISaleRepository, SaleRepository>();
        builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();

        //DEFAULT
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.InjectPersistenceDependencies()
            .InjectInfrastructureDependencies();

        // Add Mediatr to program
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly
            );
        });

        //Console.WriteLine("🔒 Connection: " + builder.Configuration.GetConnectionString("DefaultConnection"));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if(app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
