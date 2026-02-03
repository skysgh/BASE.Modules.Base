using App.Modules.Sys.Domain.Authorization;
using App.Modules.Sys.Domain.Identity;
using App.Modules.Sys.Infrastructure.Data.EF.DbContexts.Implementations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace App.Modules.Sys.Infrastructure.Data.EF.Seeding
{
    /// <summary>
    /// Seeds demo/test data from YAML or JSON files.
    /// Supports multiple file formats and intelligent path resolution.
    /// </summary>
    /// <remarks>
    /// Design:
    /// - Supports both .yml and .json formats
    /// - Three-tier path resolution:
    ///   1. Host SeedData folder (production reality)
    ///   2. Environment variable SEEDDATA_PATH (deployment flexibility)
    ///   3. Assembly default (development convenience)
    /// - Idempotent (safe to run multiple times)
    /// 
    /// File discovery priority:
    /// 1. {fileName}.yml in resolved path
    /// 2. {fileName}.json in resolved path
    /// 3. Silently skip if neither exists
    /// </remarks>
    public class YamlDataSeeder
    {
        private readonly ModuleDbContext _context;
        private readonly IDeserializer _yamlDeserializer;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly string _seedDataPath;

        /// <summary>
        /// Create seeder with custom seed data path.
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="seedDataPath">Path to YAML/JSON files (defaults to intelligent resolution)</param>
        public YamlDataSeeder(ModuleDbContext context, string? seedDataPath = null)
        {
            _context = context;
            
            // Setup YAML deserializer
            _yamlDeserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            // Setup JSON options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Resolve seed data path with three-tier fallback
            _seedDataPath = ResolveSeedDataPath(seedDataPath);
        }

        /// <summary>
        /// Resolve seed data path with intelligent fallback.
        /// Priority: Explicit ? Host ? Environment ? Assembly
        /// </summary>
        private static string ResolveSeedDataPath(string? explicitPath)
        {
            // Priority 1: Explicit path provided
            if (!string.IsNullOrEmpty(explicitPath) && Directory.Exists(explicitPath))
            {
                return explicitPath;
            }

            // Priority 2: Host SeedData folder (production reality)
            var hostPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData");
            if (Directory.Exists(hostPath))
            {
                return hostPath;
            }

            // Priority 3: Environment variable (deployment flexibility)
            var envPath = Environment.GetEnvironmentVariable("SEEDDATA_PATH");
            if (!string.IsNullOrEmpty(envPath) && Directory.Exists(envPath))
            {
                return envPath;
            }

            // Priority 4: Assembly default (development convenience)
            return Path.Combine(AppContext.BaseDirectory, "SeedData");
        }

        /// <summary>
        /// Seed all demo data from YAML/JSON files.
        /// </summary>
        public async Task SeedAsync(CancellationToken ct = default)
        {
            if (!Directory.Exists(_seedDataPath))
            {
                // No seed data folder - skip silently
                return;
            }

            await SeedDemoUsersAsync(ct);
            await SeedDemoPermissionAssignmentsAsync(ct);
            
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Seed demo users from DemoUsers.yml or DemoUsers.json
        /// </summary>
        private async Task SeedDemoUsersAsync(CancellationToken ct)
        {
            var demoUsers = await LoadSeedDataAsync<DemoUsersSeed>("DemoUsers", ct);
            if (demoUsers?.Users == null)
            {
                return;
            }

            foreach (var userDto in demoUsers.Users)
            {
                // Check if user with this email identity already exists
                var existingIdentity = await _context.UserIdentities
                    .FirstOrDefaultAsync(i => i.Provider == "Local" && i.Email == userDto.Email, ct);

                if (existingIdentity != null)
                {
                    continue;  // User already exists
                }

                // Create new user
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    IsActive = userDto.IsActive ?? true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                // Add local identity with email
                if (!string.IsNullOrEmpty(userDto.Email))
                {
                    var identity = new UserIdentity
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Provider = "Local",
                        ProviderUserId = userDto.Email,  // Use email as username
                        Email = userDto.Email,           // Store email in Identity
                        PasswordHash = !string.IsNullOrEmpty(userDto.Password) 
                            ? HashPassword(userDto.Password) 
                            : null,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.UserIdentities.Add(identity);
                }

                // Add System.Admin permission if isSystemAdmin flag set
                if (userDto.IsSystemAdmin == true)
                {
                    var adminPerm = await _context.SystemPermissions
                        .FirstOrDefaultAsync(p => p.Key == "System.Admin", ct);

                    if (adminPerm != null)
                    {
                        _context.UserSystemPermissions.Add(new UserSystemPermission
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            PermissionKey = adminPerm.Key,
                            GrantedAt = DateTime.UtcNow,
                            GrantedBy = "DEMO_SEED"
                        });
                    }
                }
            }
        }

        /// <summary>
        /// Seed demo permission assignments from DemoPermissionAssignments.yml or .json
        /// </summary>
        private async Task SeedDemoPermissionAssignmentsAsync(CancellationToken ct)
        {
            var assignments = await LoadSeedDataAsync<DemoPermissionAssignmentsSeed>(
                "DemoPermissionAssignments", ct);

            if (assignments?.Assignments == null)
            {
                return;
            }

            foreach (var assignmentDto in assignments.Assignments)
            {
                // Find user by email in their Local identity
                var identity = await _context.UserIdentities
                    .Include(i => i.User)
                    .FirstOrDefaultAsync(i => i.Provider == "Local" && i.Email == assignmentDto.UserEmail, ct);

                if (identity?.User == null)
                {
                    continue;
                }

                // Find permission by key
                var permission = await _context.SystemPermissions
                    .FirstOrDefaultAsync(p => p.Key == assignmentDto.PermissionKey, ct);

                if (permission == null)
                {
                    continue;
                }

                // Idempotent - only add if doesn't exist
                var exists = await _context.UserSystemPermissions
                    .AnyAsync(up => up.UserId == identity.UserId && 
                                   up.PermissionKey == permission.Key, ct);

                if (!exists)
                {
                    _context.UserSystemPermissions.Add(new UserSystemPermission
                    {
                        Id = Guid.NewGuid(),
                        UserId = identity.UserId,
                        PermissionKey = permission.Key,
                        GrantedAt = DateTime.UtcNow,
                        GrantedBy = "DEMO_SEED"
                    });
                }
            }
        }

        /// <summary>
        /// Load seed data from YAML or JSON file.
        /// Tries .yml first, then .json as fallback.
        /// </summary>
        private async Task<T?> LoadSeedDataAsync<T>(string fileNameWithoutExt, CancellationToken ct)
        {
            // Try YAML first
            var yamlFile = Path.Combine(_seedDataPath, $"{fileNameWithoutExt}.yml");
            if (File.Exists(yamlFile))
            {
                var yaml = await File.ReadAllTextAsync(yamlFile, ct);
                return _yamlDeserializer.Deserialize<T>(yaml);
            }

            // Try JSON fallback
            var jsonFile = Path.Combine(_seedDataPath, $"{fileNameWithoutExt}.json");
            if (File.Exists(jsonFile))
            {
                var json = await File.ReadAllTextAsync(jsonFile, ct);
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }

            // Neither exists - return default
            return default;
        }

        /// <summary>
        /// Hash password (placeholder - use proper hashing in production!)
        /// </summary>
        private static string HashPassword(string password)
        {
            // TODO: Replace with Argon2 or bcrypt
            // This is a placeholder for demo purposes
            return Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"DEMO_HASH:{password}"));
        }
    }

    #region YAML DTOs

    /// <summary>
    /// Root object for DemoUsers.yml
    /// </summary>
    internal sealed class DemoUsersSeed
    {
        /// <summary>
        /// List of demo users
        /// </summary>
        public List<DemoUserDto>? Users { get; set; }
    }

    /// <summary>
    /// DTO for YAML user definition
    /// </summary>
    internal sealed class DemoUserDto
    {
        /// <summary>
        /// User email address
        /// </summary>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Local password (creates Local identity)
        /// </summary>
        public string? Password { get; set; }
        
        /// <summary>
        /// Whether user is active
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Whether user is system administrator
        /// </summary>
        public bool? IsSystemAdmin { get; set; }
    }

    /// <summary>
    /// Root object for DemoPermissionAssignments.yml
    /// </summary>
    internal sealed class DemoPermissionAssignmentsSeed
    {
        /// <summary>
        /// List of permission assignments
        /// </summary>
        public List<DemoPermissionAssignmentDto>? Assignments { get; set; }
    }

    /// <summary>
    /// DTO for YAML permission assignment
    /// </summary>
    internal sealed class DemoPermissionAssignmentDto
    {
        /// <summary>
        /// User email (must match DemoUsers.yml)
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;
        
        /// <summary>
        /// Permission key (must match SystemPermission)
        /// </summary>
        public string PermissionKey { get; set; } = string.Empty;
    }

    #endregion
}
