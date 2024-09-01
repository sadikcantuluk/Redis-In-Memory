using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Model;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateProduct(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllProduct()
        {
            var list = await _appDbContext.Products.ToListAsync();
            return list;
        }

        public async Task<Product> GetProductById(int id)
        {
            var item = await _appDbContext.Products.SingleOrDefaultAsync(x => x.Id == id);
            return item;
        }
    }
}
