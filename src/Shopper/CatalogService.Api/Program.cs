using CatalogService.Domain;
using CatalogService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

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

app.MapControllers();

app.Run();
