
using System;
using System.Diagnostics;
using ImageProcessingApi.Logic;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImageProcessingApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ImageFileProcessor, ImageFileProcessor>();

        var app = builder.Build();
        app.UseExceptionHandler(errorApp  => ConfigureExceptionHandler(errorApp, app.Environment));

         
        app.UseStatusCodePages();

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

    static void ConfigureExceptionHandler(IApplicationBuilder errorApp, IWebHostEnvironment env)
    {
        errorApp.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception is not null)
            {
                logger.LogError(exception, $"Unhandled exception at {context.Request.Path}", context.Request.Path);
            }

            var detail = env.IsDevelopment() ? exception?.Message : "An unexpected error occurred";

            var problem = new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception?.Message,
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] = context.TraceIdentifier;

            context.Response.StatusCode = problem.Status ?? 500;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(problem);
        });
    }
}