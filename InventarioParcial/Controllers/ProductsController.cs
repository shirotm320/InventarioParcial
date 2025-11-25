using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models; 
using InventarioParcial.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventarioParcial.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository; 
        private readonly IMapper _mapper;

       
        public ProductsController(IProductRepository productRepository,
                                  ICategoryRepository categoryRepository, 
                                  IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductReadDto>>(products);
            return View(productDtos);
        }

       
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

    
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductCreateDto>(product);

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
          
            ViewBag.ProductId = id;
            return View(productDto);
        }
    }
}