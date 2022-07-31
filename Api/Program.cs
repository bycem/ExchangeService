using System;
using System.ComponentModel.DataAnnotations;
using Api;
using Api.Common;
using Api.Dtos;
using Application;
using Application.Commands.CalculateAmountForTargetCurrencies;
using Application.Common;
using Application.Queries.GetExchangeRateListByFilter;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using ExternalServices.Repositories;
using FluentValidation.AspNetCore;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var settings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();

builder.Services.AddSingleton<IExchangeTransactionRepository, ExchangeTransactionRepository>();

builder.Services.AddHttpClient<IExchangeProvider, CurrencyDataExchangeProviderImpl>("CurrencyData",(sp, http) =>
{
    http.BaseAddress = new Uri(settings.CurrencyDataApiUrl);
    http.DefaultRequestHeaders.Add("apiKey", settings.CurrencyDataApiKey);
});

builder.Services.AddHttpClient<IExchangeProvider, FixerExchangeProviderImpl>("Fixer", (sp, http) =>
{
    http.BaseAddress = new Uri(settings.FixerApiUrl);
    http.DefaultRequestHeaders.Add("apiKey", settings.FixerApiKey);
});

builder.Services.AddScoped<IExchangeProviderOrchestrator, ExchangeProviderOrchestratorImpl>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(GetCurrencyExchangeRateListQuery).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
builder.Services.AddFluentValidation(conf =>
{
    conf.RegisterValidatorsFromAssembly(typeof(GetCurrencyExchangeRateListQuery).Assembly);
    conf.AutomaticValidationEnabled = true;
});

builder.Services.AddResponseCaching(x => x.MaximumBodySize = 1024);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseResponseCaching();
app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] {"Accept-Encoding"};

    await next();
});

app.MapGet("/exchange/rate/source/{sourceCurrencyCode}", async (
    [FromServices] IMediator mediator,
    [Required] string sourceCurrencyCode,
    [Required] string targetCurrencyCodes) =>
{
    var response =
        await mediator.Send(new GetCurrencyExchangeRateListQuery(sourceCurrencyCode, targetCurrencyCodes.Split(",")));

    return response;
});

app.MapPost("/exchange/source/{sourceCurrencyCode}", async (
    [FromServices] IMediator mediator,
    [Required] string sourceCurrencyCode,
    [FromBody] ExchangeListInput input) =>
{
    var request =
        new CalculateAmountForTargetCurrenciesCommand(sourceCurrencyCode, input.Amount, input.TargetCurrencies);
    var response = await mediator.Send(request);

    return response;
});

app.MapGet("/exchange/list", async ([FromServices] IMediator mediator,
    Guid? transactionId,
    DateTime? startDate,
    DateTime? endDate) =>
{
    var result = await mediator.Send(new GetExchangeRateListByFilterQuery(startDate, endDate, transactionId));
    return result;
});


app.Run();