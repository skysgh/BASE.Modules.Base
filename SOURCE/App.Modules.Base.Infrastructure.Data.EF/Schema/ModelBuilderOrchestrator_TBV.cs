namespace App.Modules.Base.Infrastructure.Data.EF.Schema.Management.Implementations
{
    using System;
    using System.Reflection;
    using App.Modules.Base.Infrastructure.Factories;
    using App.Modules.Base.Infrastructure.Storage.Db.EF.Schema.Management;
    using App.Modules.Base.Shared.Contracts;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// <para>
    /// A specialised class to define a DbContext model. 
    /// It's extracted from the DbContext itself for SOC
    /// objectives (when done by manual method, can end up with
    /// lots of models, and it gets unwieldy, making DbContext
    /// hard to grock.
    /// </para>
    /// </summary>
    public class ModelBuilderOrchestrator : IModelBuilderOrchestrator
    {

        /// <summary>
        /// Constructor
        /// <para>
        /// Invoked by <see cref="ServiceLocator"/>
        /// from within the <c>OnModelCreating</c> method of a 
        /// DbContext.
        /// </para>
        /// </summary>
        public ModelBuilderOrchestrator()
        {

        }

        /// <summary>
        /// Invoked from within a
        /// <see cref="DbContext.OnModelCreating(ModelBuilder)"/>.
        /// <para>
        /// It's purpose is to search in assemblies for 
        /// all classes that implement a given contract
        /// (<see cref="IHasAppModuleDbContextModelBuilderInitializer"/>)
        /// to indicate it is part of a database schema.
        /// And invokes them.
        /// </para>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="assemblies"></param>
        public void Initialize(ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            DefineByReflection(modelBuilder, assemblies);
        }


        private void DefineByReflection(ModelBuilder modelBuilder, params Assembly[] assemblies)
        {
            if (modelBuilder is null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            // You can initialize manually or by
            // Convention over Configuration
            // using a combination of
            // common interfaces and reflection.

            // Here we're finding all implementations of a specific
            // app defined contract, and if it's not specifically
            // marked to be ignored, the instance's Define instance
            // is invoked -- passin in the modelBuilder -- in order
            // for it to append model definitions to it.
            ServiceLocator.
                GetAll<IHasAppModuleDbContextModelBuilderInitializer>()
                .ForEach(x =>
                {
                    if (typeof(IHasIgnoreThis)
                        .IsAssignableFrom(x.GetType()))
                    {
                        return;
                    }
                    if (assemblies.Length > 0)
                    {
                        if (!assemblies.Any(
                            a =>
                            a == x.GetType().Assembly
                            ))
                        {
                            return;
                        }
                    }
                    //Otherwise proceed with adding it to the 
                    // passed modelBuilder:
                    x.Define(modelBuilder);
                });
        }

    }
}
