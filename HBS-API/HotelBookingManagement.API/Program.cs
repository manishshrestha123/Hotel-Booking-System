using HotelBookingManagement.Application.AppService;
using HotelBookingManagement.Domain.Interface;
using HotelBookingManagement.Infrastructure.Persistence;
using HotelBookingManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Auth
builder.Services.AddScoped<AuthAppService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Room
builder.Services.AddScoped<RoomAppService>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

// Booking
builder.Services.AddScoped<BookingAppService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Customer
builder.Services.AddScoped<CustomerAppService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<HotelBookingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseHttpsRedirection();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.MapControllers();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.Run();

