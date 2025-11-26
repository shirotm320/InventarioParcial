using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models;
using InventarioParcial.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Necesario para crear la identidad del usuario

namespace InventarioParcial.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        // ==========================================
        // REGISTRO (Solo para crear usuarios nuevos)
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            if (!ModelState.IsValid) return View(userDto);

            // 1. Validar si ya existe
            if (await _authRepository.UserExists(userDto.Username))
            {
                ModelState.AddModelError("Username", "Este usuario ya existe.");
                return View(userDto);
            }

            // 2. Mapear y registrar
            var userToCreate = _mapper.Map<User>(userDto);

            // El repositorio se encarga de hashear la password y asignar el rol
            await _authRepository.Register(userToCreate, userDto.Password);

            // 3. Redirigir al Login
            return RedirectToAction("Login");
        }

        // ==========================================
        // LOGIN (Ingreso al sistema)
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginDto userDto)
        {
            if (!ModelState.IsValid) return View(userDto);

            // 1. Verificar credenciales en BD
            var userFromRepo = await _authRepository.Login(userDto.Username.ToLower(), userDto.Password);

            if (userFromRepo == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(userDto);
            }

            // 2. CREAR LA IDENTIDAD (CLAIMS)
            // Esto es como crear el "Carnet de Identidad" que llevará el usuario en la Cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            // Agregamos los roles al carnet (importante para [Authorize(Roles="Admin")])
            foreach (var role in userFromRepo.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Mantener sesión abierta aunque cierre el navegador
            };

            // 3. ESCRIBIR LA COOKIE EN EL NAVEGADOR
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // 4. Redirigir al Home
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // LOGOUT (Salir)
        // ==========================================
        public async Task<IActionResult> Logout()
        {
            // Borra la cookie del navegador
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // ==========================================
        // ACCESO DENEGADO
        // ==========================================
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}



