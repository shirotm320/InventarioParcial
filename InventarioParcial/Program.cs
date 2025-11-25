using InventarioParcial.Data;
using InventarioParcial.Data.InventarioParcial.Models;
using InventarioParcial.Repositories; // Necesario para los repos
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. ZONA DE SERVICIOS (AGREGAR INGREDIENTES)
// Todo esto debe ir ANTES de builder.Build()
// ==========================================

// A. Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// B. AutoMapper (Aquí estaba tu error, estaba abajo)
builder.Services.AddAutoMapper(typeof(Program));

// C. Repositorios (Inyección de Dependencias)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// D. Controladores y Vistas
builder.Services.AddControllersWithViews();

// ==========================================
// ⛔ EL MURO: AQUÍ SE CONSTRUYE LA APP ⛔
// No puedes agregar más 'builder.Services' después de esta línea
// ==========================================
var app = builder.Build();

// ==========================================
// 2. ZONA DE PIPELINE (CÓMO FUNCIONA LA APP)
// Todo esto debe ir DESPUÉS de builder.Build()
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();