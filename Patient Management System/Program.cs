using Microsoft.EntityFrameworkCore;
using Patient_Management_System.Data;
using Serilog;

namespace Patient_Management_System;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Host.UseSerilog((context, config) =>
            config.ReadFrom.Configuration(context.Configuration));
        
        builder.Services.AddControllers();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddDbContext<PatientContext>(options => 
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
            )
        );
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.Run();
    }
}