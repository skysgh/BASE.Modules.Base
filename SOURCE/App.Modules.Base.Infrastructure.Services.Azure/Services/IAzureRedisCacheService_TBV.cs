namespace App.Base.Infrastructure.Services
{
    using System;

    public interface IAzureRedisCacheService : IHasAppCoreService
    {

        void Set<T>(string key, T value, TimeSpan? duration=null);

        void Set<T>(string key, string subKey, T value, TimeSpan? duration=null);

        T Get<T>(string key);

        T Get<T>(string key, string subKey);
    }

}