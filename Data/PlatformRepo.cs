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

            // db.StringSet(model.Id, serialModel);
            // db.SetAdd("PlatformSet", serialModel);
            db.HashSet("hashplatform", new HashEntry[]
            {
                new HashEntry(model.Id, serialModel)
            });
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            var db = _connection.GetDatabase();

            //var set = db.SetMembers("PlatformSet");
            var hash = db.HashGetAll("hashplatform");
            if (hash.Length > 0)
            {
                var obj = Array.ConvertAll(hash, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
                return obj;
            }
            return null;
        }

        public Platform GetPlatformById(string id)
        {
            var db = _connection.GetDatabase();

            //var plat = db.StringGet(id);
            var plat = db.HashGet("hashplatform", id);

            if (!string.IsNullOrEmpty(plat))
                return JsonSerializer.Deserialize<Platform>(plat);

            return null;
        }
    }
}