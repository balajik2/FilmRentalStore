using AutoMapper;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using FilmRentalStore.Services;
using FilmRentalStore.DTO;
using FilmRentalStore.Map;
using FilmRentalStore.Validators;
using FluentValidation;



var builder = WebApplication.CreateBuilder(args);




builder.Services.AddControllers();


// Register the DbContext with a connection string
builder.Services.AddDbContext<Sakila12Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));





//Creating mapping configuration & passing it to mapper profile
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

//create Imapper instance & pass mapperconfig to it
IMapper mapper = mapperConfig.CreateMapper();

//Register the mapper instance to the service conatiner
builder.Services.AddSingleton(mapper);




//add the repository
builder.Services.AddScoped<IStaffRepository, StaffService>();





//Fluent Validator....
builder.Services.AddValidatorsFromAssemblyContaining<StaffValidator>();








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

app.MapControllers();

app.Run();
