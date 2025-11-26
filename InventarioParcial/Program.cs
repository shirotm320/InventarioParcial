using InventarioParcial.Data.InventarioParcial.Models;
using InventarioParcial.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies; // <--- NECESARIO PARA COOKIES
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
builder.Services.AddScoped<IAuthRepository, AuthRepository>(); // <--- NUEVO: Repositorio de Auth

// D. Configuración de Autenticación (COOKIES)
// Esto le dice a la App: "Usa cookies para recordar al usuario y si no está logueado, mándalo al Login"
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Ruta al Login
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Ruta si no tiene permiso (ej: User queriendo borrar)
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // La sesión dura 1 hora
    });

// E. Controladores y Vistas
builder.Services.AddControllersWithViews();

// ==========================================
// ⛔ CONSTRUCCIÓN DE LA APP
// ==========================================
var app = builder.Build();

// ==========================================
// 2. PIPELINE
// ==========================================

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