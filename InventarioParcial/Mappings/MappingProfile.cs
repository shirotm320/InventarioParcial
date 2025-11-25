using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models;

namespace InventarioParcial.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Categorías (Ida y vuelta)
            CreateMap<Category, CategoryDto>().ReverseMap();

            
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Sin Categoría"));

            // 3. ProductCreateDto -> Product (Creación)
            CreateMap<ProductCreateDto, Product>();

            // 4. Usuarios (Solo mapeamos Username, la password se maneja aparte por seguridad)
            CreateMap<UserRegisterDto, User>();
        }
    }
}