using RedisExampleApp.API.Model;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;

        private string productListName = "cacheProducts";
        private IDatabase _database;

        public ProductRepositoryWithCache(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _database = _redisService.GetDb(2);
        }

        public async Task CreateProduct(Product product)
        {
            await _productRepository.CreateProduct(product);

            if (await _database.KeyExistsAsync(productListName))
            {
                await _database.HashSetAsync(productListName,product.Id,JsonSerializer.Serialize(product));
            }
        }

        public async Task<List<Product>> GetAllProduct()
        {
            if (!await _database.KeyExistsAsync(productListName))
            {
                await LoadToCacheFromDb();
            }

            List<Product> products = new List<Product>();

            var cacheList = await _database.HashGetAllAsync(productListName);
            foreach (var item in cacheList)
            {
                products.Add(JsonSerializer.Deserialize<Product>(item.Value));
            }

            return products;
        }

        public async Task<Product> GetProductById(int id)
        {
            if (await _database.KeyExistsAsync(productListName))
            {
                var product = await _database.HashGetAsync(productListName, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var dbList = await LoadToCacheFromDb();
            return dbList.FirstOrDefault(x => x.Id == id);

        }

        public async Task<List<Product>> LoadToCacheFromDb()
        {
            var dbList = await _productRepository.GetAllProduct();

            foreach (var item in dbList)
            {
                await _database.HashSetAsync(productListName, item.Id, JsonSerializer.Serialize(item));
            }

            return dbList;
        }
    }
}
