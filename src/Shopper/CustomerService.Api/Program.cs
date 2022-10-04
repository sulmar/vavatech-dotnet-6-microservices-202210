


using CustomerService.Api;
using CustomerService.Domain;
using CustomerService.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();


builder.Services.AddHealthChecks()
        .AddCheck("Ping", () => HealthCheckResult.Healthy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/customers", (ICustomerRepository repository) => Results.Ok(repository.Get()))
    .Produces<IEnumerable<Customer>>();

//app.MapGet("api/customers/{id}", (ICustomerRepository repository, int id) =>
//{
//    var customer = repository.Get(id);

//    if (customer == null)
//        return Results.NotFound();

//    return Results.Ok(repository.Get(id));
//});

// Przyk³ad z u¿yciem operatora is
//app.MapGet("api/customers/{id}", (ICustomerRepository repository, int id) => repository.Get(id) is Customer customer 
//        ? Results.Ok(customer) 
//        : Results.NotFound());

// Przyk³ad z u¿yciem Match Patterns
app.MapGet("api/customers/{id}", (ICustomerRepository repository, int id) => repository.Get(id) switch
{
    Customer customer => Results.Ok(customer),    
    null => Results.NotFound()    
})
    .Produces<Customer>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .WithName("GetCustomerById")
    ;


app.MapPost("api/customers", (ICustomerRepository repository, Customer customer) =>
{
    repository.Add(customer);
    
    return Results.CreatedAtRoute("GetCustomerById", new { Id = customer.Id }, customer);

});

app.MapGet("api/context", (HttpContext context) =>
{
    
});

app.MapGet("api/test", (HttpRequest req, HttpResponse res) =>
{
    return Results.Extensions.NotModified();
});

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
