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
            db.SetAdd("PlatformSet", serialModel);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            var db = _connection.GetDatabase();

            var set = db.SetMembers("PlatformSet");

            if (set.Length > 0)
            {
                var obj = Array.ConvertAll(set, val => JsonSerializer.Deserialize<Platform>(val)).ToList();
                return obj;
            }
            return null;
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