using App.Modules.Base.Infrastructure.Identifiers;
using App.Modules.Base.Substrate.Factories;

namespace App.Modules.Base.Infrastructure.Indentifiers
{
    ///<inheritdoc/>
    public class UUIDService : IUUIDService
    {
        ///<inheritdoc/>
        public Guid Generate()
        {
            return GuidFactory.NewGuid();
        }
    }
}
