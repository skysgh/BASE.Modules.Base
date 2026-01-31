// using System.Linq.Expressions;

//namespace App.Modules.Base.Infrastructure.Services
//{
//    /// <summary>
//    /// Contract that Infrastructure 
//    /// entity Repositories
//    /// services should implement.
//    /// <para>
//    /// For example,
//    /// <see cref="IRepositoryService"/>
//    /// and/or
//    /// <c>ModuleDbContextBase</c>
//    /// should implement it.
//    /// </para>
//    /// </summary>
//    public interface IQueryableService<TModel> : IHasAppInfrastructureService
//        where TModel : class
//    {
//        /// <summary>
//        /// Contract to return a queryable (ie, OData ready)
//        /// reference to a source.
//        /// <para>
//        /// When implemented by <c>IRepositoryService</c>
//        /// returns the specified <c>DbSet{T}</c>
//        /// </para>
//        /// </summary>
//        /// <param name="contextIdentifier"></param>
//        /// <param name="filterPredicate">Optional filter</param>
//        /// <param name="mergeOptions">TODO:Document better</param>
//        /// <returns></returns>
//        IQueryable<TModel> GetQueryableSet(
//        string contextIdentifier,
//            Expression<Func<TModel, bool>>? filterPredicate = null,
//            MergeOption mergeOptions = MergeOption.Undefined);

//        /// <summary>
//        /// Contract for a Method to persist changed
//        /// entities.
//        /// <para>
//        /// When implemented by <c>IRepositoryService</c>
//        /// it invokes a DbContext instance to save the entity.
//        /// </para>
//        /// <para>
//        /// IMPORTANT: 
//        /// Most ORMs will not commit immediately,
//        /// so will return from this method without any changes
//        /// to the DB yet (probably will be Committed at the 
//        /// end of the request).
//        /// </para>
//        /// </summary>
//        /// <param name="contextIdentifier"></param>
//        /// <param name="entity"></param>
//        public void AddOnCommit(
//            string contextIdentifier,
//            TModel entity);

//    }
//}

