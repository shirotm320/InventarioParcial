
    using global::InventarioParcial.Models;
    using InventarioParcial.Models;

    namespace InventarioParcial.Repositories
    {
        public interface IProductRepository
        {
            
            Task<IEnumerable<Product>> GetAllAsync();
            Task<Product?> GetByIdAsync(int id);
            Task CreateAsync(Product product);
            Task UpdateAsync(Product product);
            Task DeleteAsync(int id);
        }
    }

