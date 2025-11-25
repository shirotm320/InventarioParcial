
    using InventarioParcial.Models;
    using InventarioParcial.Models;

    namespace InventarioParcial.Repositories
    {
        public interface ICategoryRepository
        {
            Task<IEnumerable<Category>> GetAllAsync();
            Task<Category?> GetByIdAsync(int id);
            Task CreateAsync(Category category);
            Task UpdateAsync(Category category);
            Task DeleteAsync(int id);
        }
    }
