using RedisAPI.Models;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform model);
        Platform GetPlatformById(string id);
        IEnumerable<Platform> GetAllPlatforms();
    }
}