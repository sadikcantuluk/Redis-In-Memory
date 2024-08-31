using StackExchange.Redis;

namespace ExchangeApiRedisApp.Web.Services
{
    public class RedisService
    {
        ConnectionMultiplexer _connectionMultiplexer;
        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:1453");
        public IDatabase GetDb(int db) => _connectionMultiplexer.GetDatabase(db);
    }
}








