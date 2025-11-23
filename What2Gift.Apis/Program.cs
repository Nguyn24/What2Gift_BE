using System.Text.Json.Serialization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using What2Gift.Apis.Extensions;
using What2Gift.Application;
using What2Gift.Infrastructure;

namespace What2Gift.Apis;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Configuration.AddEnvironmentVariables();
        
        builder.Services.AddSwaggerGenWithAuth(); 
        
        builder.Services
            .AddApplication()
            .AddPresentation()
            .AddInfrastructure(builder.Configuration);
        
        builder.Services
            .AddControllers(options =>
            {
                // Enable DateOnly query parsing with formats: dd-MM-yyyy and yyyy-MM-dd
                options.ModelBinderProviders.Insert(0, new ModelBinding.DateOnlyModelBinderProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                // Add custom DateTime converters for Vietnam timezone (GMT+7) with AM/PM format
                options.JsonSerializerOptions.Converters.Add(new Extensions.DateTimeJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new Extensions.NullableDateTimeJsonConverter());
            });
        
        var app = builder.Build();

        app.UseSwaggerWithUi();  

        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseCors("AllowLocalAndProdFE");
        app.UseRequestContextLogging();
        app.UseExceptionHandler();
        app.UseHttpsRedirection();
        
        // Static files should be before routing
        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                // Cache static files for 1 year
                ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=31536000");
            }
        });
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.ApplyMigrations();  // chạy EF Core migration khi khởi động
        app.MapControllers();

        app.Run();
    }
}