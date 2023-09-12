using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api
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

            //Registering our ShopOnlineDbContext for dependency injection
            builder.Services.AddDbContextPool<ShopOnlineDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnectionOne")));


            //Registering for DI
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            //Registering for DI
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            /////Configuring policy to allow blazor component to access resources
            app.UseCors(policy =>
            policy.WithOrigins("http://localhost:7277", "https://localhost:7277")
            .AllowAnyMethod()
            .WithHeaders(HeaderNames.ContentType)
            );

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}