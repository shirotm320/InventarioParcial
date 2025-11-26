using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models;
using InventarioParcial.Repositories;

namespace InventarioParcial.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task CreateProductAsync(ProductCreateDto dto)
        {
            // Aquí podrías poner validaciones de negocio extra
            var product = _mapper.Map<Product>(dto);
            await _repo.CreateAsync(product);
        }

    }


}
