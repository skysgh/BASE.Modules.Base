using App.Modules.Sys.Shared.Services;
using System;

namespace App.Modules.Sys.Infrastructure.Azure.Services
{

    /// <summary>
    /// Contract for a service to manage Redis Cache in Azure
    /// </summary>
    public interface IAzureRedisCacheService : IHasService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        void SetValue<T>(string key, T value, TimeSpan? duration=null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="subKey"></param>
        /// <param name="value"></param>
        /// <param name="duration"></param>
        void SetValue<T>(string key, string subKey, T value, TimeSpan? duration=null);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetValue<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="subKey"></param>
        /// <returns></returns>
        T GetValue<T>(string key, string subKey);
    }

}