using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data
{
    public class PlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _connection;

        public PlatformRepo(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }
        public void CreatePlatform(Platform model)
        {
            if (model is null)
                throw new ArgumentOutOfRangeException(nameof(model));

            var db = _connection.GetDatabase();

            var serialModel = JsonSerializer.Serialize(model);

            db.StringSet(model.Id, serialModel);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Platform GetPlatformById(string id)
        {
            var db = _connection.GetDatabase();

            var plat = db.StringGet(id);

            if (!string.IsNullOrEmpty(plat))
                return JsonSerializer.Deserialize<Platform>(plat);

            return null;
        }
    }
}