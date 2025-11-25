using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models; // Necesario para mapear clases
using InventarioParcial.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventarioParcial.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository; // <--- ESTO FALTABA
        private readonly IMapper _mapper;

        // Inyectamos AMBOS repositorios y el Mapper
        public ProductsController(IProductRepository productRepository,
                                  ICategoryRepository categoryRepository, // <--- Y ESTO EN EL CONSTRUCTOR
                                  IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // ======================================
        // 1. LISTADO (INDEX)
        // ======================================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductReadDto>>(products);
            return View(productDtos);
        }

        // ======================================
        // 2. CREAR (CREATE)
        // ======================================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Cargamos la lista para el dropdown
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateDto productDto)
        {
            if (ModelState.IsValid)
            {
                var productToSave = _mapper.Map<Product>(productDto);
                await _productRepository.CreateAsync(productToSave);
                return RedirectToAction(nameof(Index));
            }

            // Si falla, recargar categorías
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            return View(productDto);
        }

        // ======================================
        // 3. EDITAR (EDIT)
        // ======================================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductCreateDto>(product);

            // Importante: Cargar categorías de nuevo para el Select en Edit
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.ProductId = id;

            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateDto productDto)
        {
            if (ModelState.IsValid)
            {
                var productToUpdate = await _productRepository.GetByIdAsync(id);

                if (productToUpdate == null) return NotFound();

                // AutoMapper actualiza solo los campos que cambiaron
                _mapper.Map(productDto, productToUpdate);

                await _productRepository.UpdateAsync(productToUpdate);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            // Importante: volver a pasar el ID por si la vista lo necesita
            ViewBag.ProductId = id;
            return View(productDto);
        }
    }
}