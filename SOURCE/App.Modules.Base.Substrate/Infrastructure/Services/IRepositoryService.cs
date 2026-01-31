//namespace App.Modules.Base.Infrastructure.Services
//{
//    using System;
//    using System.Linq;
//    using System.Linq.Expressions;
//    using App.Modules.Base.Substrate.Models;
//    using App.Modules.Base.Substrate.Models.Contracts;

//    /// <summary>
//    /// Specifies how objects being loaded into the context are merged with objects already in the context.
//    /// </summary>
//    public enum MergeOption
//    {
//        /// ....
//        Undefined = -1,

//        /// Objects already in the context are not loaded from the data source (default behavior).
//        AppendOnly = 0,

//        /// Objects are always loaded from the data source. Any property changes made
//        /// to objects in the object context are overwritten by the data source values.
//        OverwriteChanges = 1,

//        /// unmodified properties of objects in the object context are overwritten with server values.
//        PreserveChanges = 2,

//        /// Objects are maintained in a Detached state and are not tracked.
//        NoTracking = 3
//    }


//    /// <summary>
//    /// The contract for a generic repository service
//    /// </summary>
//    public interface IRepositoryService :
//        IQueryableService,
//        IHasAppInfrastructureService
//    {
//        /// <summary>
//        /// Determine if there have yet been any 
//        /// changes made to the db
//        /// during this Request.
//        /// </summary>
//        /// <param name="contextKey"></param>
//        /// <returns></returns>
//        bool HasChanges(string contextKey);

//        /// <summary>
//        /// Determines if any elements with the specified condition 
//        /// exists 
//        /// <para>
//        /// Note:
//        /// this method is faster than using 
//        /// <see cref="Count{TModel}(string, Expression{Func{TModel, bool}})"/>.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="filterPredicate"></param>
//        /// <returns></returns>
//        bool Any<TModel>(string contextKey, Expression<Func<TModel, bool>>? filterPredicate = null) where TModel : class;

//        /// <summary>
//        /// Returns the number of Records that match
//        /// the given Conditions.
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="filterPredicate"></param>
//        /// <returns>The count of found records</returns>
//        int Count<TModel>(string contextKey, Expression<Func<TModel, bool>>? filterPredicate = null)
//            where TModel : class;

//        /* 
//        // Defined in the base IQueryableService:
//        TModel GetSingle<TModel>(string contextKey, Expression<Func<TModel, bool>> filterPredicate,
//            MergeOption mergeOptions = MergeOption.Undefined) where TModel : class;

//        // Defined in the base IQueryableService:
//        IQueryable<TModel> GetQueryableSet<TModel>(string contextKey, Expression<Func<TModel, bool>> filterPredicate,
//            MergeOption mergeOptions = MergeOption.Undefined) where TModel : class;
//        */

//        /// <summary>
//        /// Gets a single Entity Model,
//        /// based on provided 
//        /// property filter expression
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="filterPredicate"></param>
//        /// <param name="mergeOptions"></param>
//        /// <returns></returns>
//        IQueryable<TModel> GetQueryableSingle<TModel>(
//            string contextKey,
//            Expression<Func<TModel, bool>> filterPredicate,
//            MergeOption mergeOptions = MergeOption.Undefined)
//            where TModel : class;

//        /// <summary>
//        /// Adds or updates a model which matches the given criteria. 
//        /// <para>
//        /// Used only for seeding.
//        /// </para>
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="identifierExpression"></param>
//        /// <param name="models"></param>
//        void AddOrUpdate<TModel>(
//            string contextKey,
//            Expression<Func<TModel, object>> identifierExpression,
//            params TModel[] models) where TModel : class;

//        /// <summary>
//        /// Persists the given entity.
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="model"></param>
//        void PersistOnCommit<TModel, TId>(string contextKey, TModel model) where TModel : class, IHasTimestamp;


//        /*
//        // Defined in the base IQueryableService:
//        void AddOnCommit<TModel>(string contextKey, TModel model) where TModel : class;
//        */

//        /// <summary>
//        /// Sets state to Modified. If untracked, Attachs first.
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="model"></param>
//        void UpdateOnCommit<TModel>(string contextKey, TModel model) where TModel : class;

//        /// <summary>
//        /// Removes the specified model, 
//        /// when the active IUnitOfWork is Committed.
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <typeparam name="TId"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="id"></param>
//        void DeleteOnCommit<TModel, TId>(string contextKey, TId id) where TModel : class, IHasId<TId>, new();

//        /// <summary>
//        /// Removes the specified model, 
//        /// when the active IUnitOfWork is Committed.
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="model"></param>
//        void DeleteOnCommit<TModel>(string contextKey, TModel model) where TModel : class;

//        /// <summary>
//        /// Removes entities from the datastore that match the 
//        /// given filter, when the active IUnitOfWork is Committed.
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="filterPredicate"></param>
//        void DeleteOnCommit<TModel>(string contextKey, Expression<Func<TModel, bool>>? filterPredicate)
//            where TModel : class;


//        /// <summary>
//        /// Determine if given model
//        /// is attached to specified Context.
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        bool IsAttached<TModel>(string contextKey, TModel model) where TModel : class;


//        /// <summary>
//        /// Attaches untracked -- but already saved 
//        /// at some point -- entities to specified 
//        /// Context.
//        /// <para>
//        /// Prefer UpdateOnCommit
//        /// </para>
//        /// <para>
//        /// IMPORTANT: 
//        /// Until Committed, most ORMs (eg: EF) will 
//        /// return unchanged entities in subsequent queries.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="model"></param>
//        void AttachOnCommit<TModel>(string contextKey, TModel model) where TModel : class;


//        /// <summary>
//        /// Detaches Models from current Context.
//        /// </summary>
//        /// <typeparam name="TModel"></typeparam>
//        /// <param name="contextKey"></param>
//        /// <param name="model"></param>
//        void Detach<TModel>(string contextKey, TModel model) where TModel : class;


//        /// <summary>
//        /// Determines whether entity is new or not.
//        /// <para>
//        /// TODO: improve documentation.
//        /// </para>
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        bool IsNew<T>(T model) where T : IHasTimestamp;

//        /// <summary>
//        /// It will save the specified content automatically
//        /// and immediately
//        /// (ie, be sure you want to do this, now, rather
//        /// than waiting till the end of the request to trigger
//        /// it).
//        /// </summary>
//        /// <param name="contextKey"></param>
//        void SaveChanges(string contextKey);
//    }
//}