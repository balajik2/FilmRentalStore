using AutoMapper;
<<<<<<< HEAD
using FilmRentalStore.Map;
=======

>>>>>>> origin/FilmRentalStore-1
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using Microsoft.EntityFrameworkCore;
using FilmRentalStore.Validators;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
<<<<<<< HEAD
using FluentValidation;
using FilmRentalStore.DTO;
using FilmRentalStore.Vaidators;
=======
using FilmRentalStore.Map;
using FluentValidation;
>>>>>>> origin/FilmRentalStore-1

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


// Register the DbContext with a connection string
builder.Services.AddDbContext<Sakila12Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


<<<<<<< HEAD
//create mapper configuration and passing it to the mapper profile
=======
>>>>>>> origin/FilmRentalStore-1
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

<<<<<<< HEAD
//create Imapper instance and pass the mapperconfig to it
IMapper mapper = mapperConfig.CreateMapper();

//register the mapper instance to the service container
builder.Services.AddSingleton(mapper);





//Register the Repo

builder.Services.AddScoped<IFilmRepository, FilmService>();

//Configure the Validator

builder.Services.AddValidatorsFromAssemblyContaining<FilmValidator>();


=======
IMapper mapper = mapperConfig.CreateMapper();



builder.Services.AddSingleton(mapper);



builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();

builder.Services.AddControllers();
>>>>>>> origin/FilmRentalStore-1
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddScoped<ICustomerRepository,CustomerService>();
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
