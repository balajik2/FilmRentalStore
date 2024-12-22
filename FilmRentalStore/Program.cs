using AutoMapper;
using FilmRentalStore.Map;

using FilmRentalStore.Models;
using FilmRentalStore.Services;
using Microsoft.EntityFrameworkCore;
using FilmRentalStore.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

using FluentValidation;
using FilmRentalStore.DTO;

using FilmRentalStore.Map;
using FluentValidation;


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

builder.Services.AddScoped<IFilmRepository, FilmService>();

//Configure the Validator

builder.Services.AddValidatorsFromAssemblyContaining<FilmValidator>();



//IMapper mapper = mapperConfig.CreateMapper();



builder.Services.AddSingleton(mapper);



builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddScoped<ICustomerRepository, CustomerService>();
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

app.MapControllers();

app.Run();
