<<<<<<< HEAD
using System.Net;
using AutoMapper;
using FilmRentalStore.Map;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FilmRentalStore.Validator;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
=======


﻿using AutoMapper;
using FilmRentalStore.Map;


using AutoMapper;


using AutoMapper;

using FilmRentalStore.Models;
using FilmRentalStore.Services;
using Microsoft.EntityFrameworkCore;


using FilmRentalStore.Validators;

using FilmRentalStore.DTO;
using FilmRentalStore.Validators;
using FluentValidation;
using FilmRentalStore.Services;



>>>>>>> c9f394a110474532bb4f8cbdea417adb7c59f041
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using FilmRentalStore.Map;

using FluentValidation;
using FilmRentalStore.DTO;

using FilmRentalStore.Map;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

using FilmRentalStore.Services;
using FilmRentalStore.DTO;
using FilmRentalStore.Map;
using FilmRentalStore.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

<<<<<<< HEAD
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
=======
builder.Services.AddControllers();


// Register the DbContext with a connection string
builder.Services.AddDbContext<Sakila12Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var mapperConfig = new MapperConfiguration(mc =>

{

    mc.AddProfile(new MappingProfile());

});
IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);








//Register the Repository

builder.Services.AddScoped<IFilmRepository, FilmService>();

builder.Services.AddScoped<IAdduserRepository, AddUserService>();

//Configure the Validator

builder.Services.AddValidatorsFromAssemblyContaining<FilmValidator>();



//IMapper mapper = mapperConfig.CreateMapper();



builder.Services.AddSingleton(mapper);



builder.Services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
builder.Services.AddScoped<IStoreRepository, StoreClass>();
builder.Services.AddValidatorsFromAssemblyContaining<StoreValidators>();
builder.Services.AddScoped<IInventoryRepository,InventoryServices>();


builder.Services.AddControllers();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});




//add the repository
builder.Services.AddScoped<IStaffRepository, StaffService>();
builder.Services.AddScoped<IPaymentRepository, PaymentService>();




//Fluent Validator....
builder.Services.AddValidatorsFromAssemblyContaining<StaffValidator>();









>>>>>>> c9f394a110474532bb4f8cbdea417adb7c59f041
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddScoped<ICustomerRepository,CustomerService>();
builder.Services.AddScoped<IAuthRepository, AuthService>();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
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






//Global Exception 


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


//Setting up AJAX

app.UseStaticFiles();

app.UseRouting();


app.MapControllers();

app.Run();
