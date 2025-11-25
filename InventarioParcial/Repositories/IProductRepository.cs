
    using global::InventarioParcial.Models;
    using InventarioParcial.Models;

    namespace InventarioParcial.Repositories
    {
        public interface IProductRepository
        {
            // Usamos Task para que sea asíncrono (Async/Await) - Muy importante hoy en día
            Task<IEnumerable<Product>> GetAllAsync();
            Task<Product?> GetByIdAsync(int id);
            Task CreateAsync(Product product);
            Task UpdateAsync(Product product);
            Task DeleteAsync(int id);
        }
    }

