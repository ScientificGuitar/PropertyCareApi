using Microsoft.EntityFrameworkCore;
using PropertyCareApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PropertyCareApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return "Hello World!";
})
.WithName("GetPropertyCare");

app.Run();