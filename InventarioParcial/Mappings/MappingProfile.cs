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

            // 2. Producto -> ProductReadDto (Lectura)
            // TRUCO: Mapeamos el nombre de la categoría (que está en otra tabla) 
            // directamente a la propiedad plana 'CategoryName' del DTO.
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