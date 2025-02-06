using TSFiler.API.Extensions;
using TSFiler.API.Middlewares;
using Serilog;

namespace TSFiler.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) =>
            loggerConfiguration
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("logs/ts-filer.log", rollingInterval: RollingInterval.Day)
        );

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureServices(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseCors("AllowAllOrigins");
        app.MapControllers();

        app.Run();
    }
}
