using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models;
using InventarioParcial.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InventarioParcial.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // 1. LISTADO
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return View(dtos);
        }

        // 2. CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(dto);
                await _categoryRepository.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // 3. EDITAR
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            var dto = _mapper.Map<CategoryDto>(category);
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDto dto)
        {
            if (ModelState.IsValid)
            {
                var category = await _categoryRepository.GetByIdAsync(dto.Id);
                if (category == null) return NotFound();

                _mapper.Map(dto, category); // Actualiza los datos
                await _categoryRepository.UpdateAsync(category);

                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        // 4. BORRAR
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            var dto = _mapper.Map<CategoryDto>(category);
            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}