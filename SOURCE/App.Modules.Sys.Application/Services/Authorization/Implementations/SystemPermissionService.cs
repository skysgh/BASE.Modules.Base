using App.Modules.Sys.Application.Contracts.Repositories;
using App.Modules.Sys.Application.Services.Authorization;
using App.Modules.Sys.Interfaces.Models.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Modules.Sys.Application.Services.Authorization.Implementations
{
    /// <summary>
    /// Implementation of system permission service.
    /// Uses ISystemPermissionRepository interface - no direct EF dependency.
    /// Handles DTO mapping and business logic.
    /// </summary>
    internal sealed class SystemPermissionService : ISystemPermissionService
    {
        private readonly ISystemPermissionRepository _repository;

        public SystemPermissionService(ISystemPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SystemPermissionDto>> GetPermissionsAsync(
            string? category = null,
            CancellationToken ct = default)
        {
            var permissions = await _repository.GetAllAsync(category, ct);

            return permissions.Select(p => new SystemPermissionDto
            {
                Key = p.Key,
                Title = p.Title,
                Description = p.Description,
                Category = p.Category,
                CreatedAt = p.CreatedAt.ToString("o")
            });
        }

        public async Task<SystemPermissionDto?> GetPermissionByKeyAsync(
            string key,
            CancellationToken ct = default)
        {
            var permission = await _repository.GetByKeyAsync(key, ct);
            
            if (permission == null)
            {
                return null;
            }

            return new SystemPermissionDto
            {
                Key = permission.Key,
                Title = permission.Title,
                Description = permission.Description,
                Category = permission.Category,
                CreatedAt = permission.CreatedAt.ToString("o")
            };
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync(
            CancellationToken ct = default)
        {
            return await _repository.GetCategoriesAsync(ct);
        }
    }
}
