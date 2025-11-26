using AutoMapper;
using InventarioParcial.Dtos;
using InventarioParcial.Models;

namespace InventarioParcial.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Categorías
            CreateMap<Category, CategoryDto>().ReverseMap();

            // 2. Producto -> ProductReadDto (Lectura en tabla)
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Sin Categoría"));

            // 3. AQUÍ ESTABA EL ERROR: Agregamos .ReverseMap()
            // Esto permite convertir DTO -> Producto (Guardar) y Producto -> DTO (Cargar formulario Editar)
            CreateMap<ProductCreateDto, Product>().ReverseMap();

            // 4. Usuarios
            CreateMap<UserRegisterDto, User>();
        }
    }
}