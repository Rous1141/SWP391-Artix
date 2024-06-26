﻿using backend.Entities;
using backend.Service;

namespace backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        //Add Controller 
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // Đăng ký dịch vụ IVnPayService với lớp VnPayService
        builder.Services.AddTransient<IVnPayService, VnPayService>();
        // Thêm đăng ký IMemoryCache vào IServiceCollection
        builder.Services.AddMemoryCache();
        builder.Services.AddDbContext<ApplicationDbContext>();
        builder.Services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.AddDebug();
            // Thêm các cấu hình logging khác nếu cần
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1"));
        }

        app.UseCors("AllowOrigin");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}