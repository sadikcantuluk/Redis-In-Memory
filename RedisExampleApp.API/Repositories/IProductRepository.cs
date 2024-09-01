using RedisExampleApp.API.Model;

namespace RedisExampleApp.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProduct();
        Task<Product> GetProductById(int id);
        Task CreateProduct(Product product);
    }
}
