using CardBooster.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCardBoosterServices(builder.Configuration);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCardBoosterClient", policy =>
    {
        policy.WithOrigins("http://localhost:7138",
        "http://localhost:1450")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI();



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseHttpsRedirection();

app.UseCors("AllowCardBoosterClient");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
