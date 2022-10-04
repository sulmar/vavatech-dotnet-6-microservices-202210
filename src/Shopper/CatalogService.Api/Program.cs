using CatalogService.Domain;
using CatalogService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));

    })
    .AddXmlSerializerFormatters();
    ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddSingleton<IMessageService, FakeMessageService>();

builder.Services.AddResponseCompression(options =>
{    
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

// dotnet add package MediatR.Extensions.Autofac.DependencyInjection
builder.Services.AddMediatR(typeof(Program));

//if (builder.Environment.IsDevelopment())
//{
//    
//    builder.Services.AddSingleton<IProductRepository, DbProductRepository>();
//}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

app.Run();
