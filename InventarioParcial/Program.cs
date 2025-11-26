using InventarioParcial.Data.InventarioParcial.Models;
using InventarioParcial.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. ZONA DE SERVICIOS
// ==========================================

// A. Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// B. AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// C. Repositorios (Inyección de Dependencias)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Ruta al Login
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Ruta si no tiene permiso 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // La sesión dura 1 hora
    });

// E. Controladores y Vistas
builder.Services.AddControllersWithViews();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Seguridad (Orden estricto: Primero Auth, luego Authorize)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
