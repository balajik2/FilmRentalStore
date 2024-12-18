using AutoMapper;
using FilmRentalStore.Map;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
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





//Register the Repo

builder.Services.AddScoped<IFilmRepository, FilmService>();




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
