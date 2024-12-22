using AutoMapper;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using FilmRentalStore.DTO;
using FilmRentalStore.Validators;
using FluentValidation;
using FilmRentalStore.Services;


using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using FilmRentalStore.MAP;

var builder = WebApplication.CreateBuilder(args);





// Register the DbContext with a connection string
builder.Services.AddDbContext<Sakila12Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var mapperConfig = new MapperConfiguration(mc =>

{

    mc.AddProfile(new MappingProfile());

});
IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);


builder.Services.AddScoped<IStoreRepository, StoreClass>();
builder.Services.AddValidatorsFromAssemblyContaining<StoreValidators>();
builder.Services.AddScoped<IInventoryRepository,InventoryServices>();

builder.Services.AddControllers();
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
