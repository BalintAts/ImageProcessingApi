
using System;
using ImageProcessingApi.Logic;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        builder.Services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                var exception = context.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception is not null)
                {
                    context.ProblemDetails.Detail = exception.Message;
                }
            };
        });

        builder.Services.AddScoped<ImageFileProcessor, ImageFileProcessor>();

        var app = builder.Build();

        app.UseExceptionHandler();

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
}