//namespace App.Modules.Base.Infrastructure.Data.EF.Interception.Implementations
//{
//    using System;
//    using App.Modules.Base.Infrastructure.Services;
//    using App.Modules.Base.Shared.Models;
//    using App.Modules.Base.Shared.Models.Entities;

//    /// <summary>
//    /// 
//    /// <para>
//    /// Invoked when the Request is wrapping up, 
//    /// and invokes <see cref="IUnitOfWorkService"/>'s 
//    /// commit operation, 
//    /// which in turn invokes each DbContext's SaveChanges, 
//    /// which are individually overridden, to in turn 
//    /// invoke <see cref="IDbContextPreCommitService"/>
//    /// which invokes 
//    /// all PreCommitProcessingStrategy implementations, such 
//    /// as this.
//    /// </para>
//    /// </summary>
//    /// <seealso cref="Implementations.Base.DbContextPreCommitProcessingStrategyBase{IHasTenantFK}" />
//    public class
//        HasTenantFKAuditabilityDbContextPreCommitStrategy : DbContextPreCommitProcessingStrategyBase<IHasTenantFK>
//    {
//        private readonly IDiagnosticsTracingService _diagnosticsTracingService;
//        private readonly ITenantService _tenantService;

//        public HasTenantFKAuditabilityDbContextPreCommitStrategy(IUniversalDateTimeService dateTimeService,
//            IDiagnosticsTracingService diagnosticsTracingService,
//            IPrincipalService principalService, ITenantService tenantService) : base(dateTimeService, principalService,
//            EntityState.Added)
//        {
//            _diagnosticsTracingService = diagnosticsTracingService;
//            _tenantService = tenantService;
//        }

//        protected override void PreProcessEntity(IHasTenantFK entity)
//        {
//            if (entity.TenantFK.Equals(Guid.Empty))
//            {
//                try
//                {
//                    entity.TenantFK = _tenantService.CurrentTenant.Id;
//                }
//                catch
//                {
//                    _diagnosticsTracingService.Trace(TraceLevel.Error, "TODO: Tenant management has to be sorted out.");
//                }
//            }
//        }
//    }
//}
