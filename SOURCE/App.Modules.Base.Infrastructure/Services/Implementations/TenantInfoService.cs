// using App.Modules.Base.Infrastructure.NewFolder.Services;
using App.Modules.Base.Infrastructure.Services;
using App.Modules.Base.Substrate.Models.CacheItems;

namespace App.Modules.Base.Infrastructure.NewFolder.Services.Implementations
{
    /// <summary>
    /// Implementation of the 
    /// <see cref="ITenantInfoService"/>
    /// </summary>
    public class TenantInfoService : ITenantInfoService
    {
        /// <summary>
        /// Find information regarding a Tenant.
        /// <para>
        /// If not found, returns Default tenant.
        /// </para>
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="returnDefaultIfNotFound"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TenancyInfo Find(string tenantId, bool returnDefaultIfNotFound = true)
        {
            throw new NotImplementedException();
        }
    }
}
