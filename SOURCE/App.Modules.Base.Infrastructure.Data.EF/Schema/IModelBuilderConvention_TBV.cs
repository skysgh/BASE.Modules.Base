using App.Modules.Base.Infrastructure.Storage.Db.EF.Schema.Management;
using Microsoft.EntityFrameworkCore;

namespace App.Modules.Base.Infrastructure.Data.EF.Schema.Definitions.Conventions
{
    /// <summary>
    /// Contract that 
    /// Conventions used to build models implement.
    /// <para>
    /// Defining DbContext Models Fluently, 
    /// turns out to be <c>HARD!</c> 
    /// for most developers to get right. 
    /// </para>
    /// <para>
    /// Using Conventions in your module 
    /// specific implementations of
    /// <see cref="IHasAppModuleDbContextModelBuilderInitializer"/>
    /// makes things easier and more predictable when a 
    /// <see cref="DbContext.OnModelCreating(ModelBuilder)"/>
    /// invokes an implementation of
    /// <see cref="IModelBuilderOrchestrator"/>
    /// which in turn invokes instances of 
    /// <see cref="IHasAppModuleDbContextModelBuilderInitializer"/>
    /// which in turn invoke implementations of this
    /// <see cref="IModelBuilderConvention"/>
    /// </para>
    /// </summary>
    public interface IModelBuilderConvention
    {
        // There is no Init/other method as each
        // convention does its own thing.
    }
}

