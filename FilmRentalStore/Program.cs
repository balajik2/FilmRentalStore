using System.Net;
using AutoMapper;
using FilmRentalStore.Map;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FilmRentalStore.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Register the DbContext with a connection string
builder.Services.AddDbContext<Sakila12Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//create mapper configuration and passing it to the mapper profile
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

//create Imapper instance and pass the mapperconfig to it
IMapper mapper = mapperConfig.CreateMapper();

//register the mapper instance to the service container
builder.Services.AddSingleton(mapper);

//Register the Repository
builder.Services.AddScoped<IRentalRepository, RentalService>();
builder.Services.AddValidatorsFromAssemblyContaining<RentalValidator>();
builder.Services.AddScoped<IActorRepository, ActorService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            var message = $"Global Exception :{exception.Error.Message} ";
            await context.Response.WriteAsync(message).ConfigureAwait(false);
        }
    });
});

app.MapControllers();

app.Run();
