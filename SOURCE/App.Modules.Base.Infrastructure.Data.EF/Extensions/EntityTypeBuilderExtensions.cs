using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App
{
    /// <summary>
    /// Extension methods for ModelBuilder to simplify common schema configuration tasks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Design Philosophy:</b>
    /// - Reduces repetitive code in entity initializers
    /// - Enforces consistent conventions across modules
    /// - Provides opt-in temporal table support (SQL Server)
    /// - Keeps domain layer clean (no EF attributes)
    /// </para>
    /// <para>
    /// <b>?? Important for Entity Initializers:</b>
    /// Entity initializers using these extensions MUST have a parameterless constructor
    /// for design-time migrations to work. They should NOT require injected services.
    /// Configuration should be pure and declarative.
    /// </para>
    /// </remarks>
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configures an entity with standard conventions: table name, schema, and optional temporal history.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to configure.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="tableName">The table name (defaults to entity type name).</param>
        /// <param name="schemaName">The schema name (optional).</param>
        /// <param name="enableTemporalTable">Enable SQL Server temporal table (system-versioned) for audit history.</param>
        /// <returns>The entity type builder for chaining.</returns>
        /// <remarks>
        /// <para>
        /// <b>Temporal Tables:</b>
        /// When enabled, SQL Server automatically tracks all changes with system-time columns.
        /// History is stored in a companion table (e.g., "Users" ? "UsersHistory").
        /// This eliminates the need for custom audit logging for data changes.
        /// </para>
        /// <para>
        /// <b>Example Usage:</b>
        /// <code>
        /// modelBuilder.Entity&lt;User&gt;()
        ///     .ConfigureTable("Users", "Base", enableTemporalTable: true)
        ///     .HasKey(u => u.Id);
        /// </code>
        /// </para>
        /// <para>
        /// <b>Note:</b> Temporal tables require SQL Server 2016+ or Azure SQL.
        /// For PostgreSQL, set enableTemporalTable: false and consider alternative audit solutions.
        /// </para>
        /// </remarks>
        public static EntityTypeBuilder<TEntity> ConfigureTable<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string? tableName = null,
            string? schemaName = null,
            bool enableTemporalTable = true) where TEntity : class
        {
            // Use entity name if table name not specified
            tableName ??= typeof(TEntity).Name;

            // Configure table with schema
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                builder.ToTable(tableName, schemaName);
            }
            else
            {
                builder.ToTable(tableName);
            }

            // Enable temporal table if requested (SQL Server only)
            if (enableTemporalTable)
            {
                builder.ToTable(tb => tb.IsTemporal(temporal =>
                {
                    temporal.UseHistoryTable($"{tableName}History", schemaName);
                    temporal.HasPeriodStart("ValidFrom");
                    temporal.HasPeriodEnd("ValidTo");
                }));
            }

            return builder;
        }

        /// <summary>
        /// Configures a standard GUID primary key with default value generation.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="keyPropertyName">The name of the key property (defaults to "Id").</param>
        /// <returns>The entity type builder for chaining.</returns>
        /// <remarks>
        /// Sets up a GUID primary key with ValueGeneratedOnAdd() for database-generated values.
        /// </remarks>
        public static EntityTypeBuilder<TEntity> ConfigureGuidKey<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string keyPropertyName = "Id") where TEntity : class
        {
            builder.HasKey(keyPropertyName);
            builder.Property(keyPropertyName).ValueGeneratedOnAdd();
            return builder;
        }

        /// <summary>
        /// Configures a standard integer primary key with identity generation.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="keyPropertyName">The name of the key property (defaults to "Id").</param>
        /// <returns>The entity type builder for chaining.</returns>
        public static EntityTypeBuilder<TEntity> ConfigureIntKey<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string keyPropertyName = "Id") where TEntity : class
        {
            builder.HasKey(keyPropertyName);
            builder.Property(keyPropertyName).ValueGeneratedOnAdd();
            return builder;
        }

        /// <summary>
        /// Configures standard audit fields (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate).
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <returns>The entity type builder for chaining.</returns>
        /// <remarks>
        /// <para>
        /// These fields are typically populated by the pre-commit service
        /// (via strategies implementing IDbCommitPreCommitProcessingStrategy).
        /// </para>
        /// <para>
        /// <b>Note:</b> If using temporal tables, you may not need these audit fields
        /// as SQL Server maintains full history automatically via ValidFrom/ValidTo columns.
        /// </para>
        /// </remarks>
        public static EntityTypeBuilder<TEntity> ConfigureAuditFields<TEntity>(
            this EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            builder.Property<string>("CreatedBy").HasMaxLength(256);
            builder.Property<DateTime>("CreatedDate");
            builder.Property<string>("ModifiedBy").HasMaxLength(256);
            builder.Property<DateTime>("ModifiedDate");
            return builder;
        }

        /// <summary>
        /// Configures a required string property with standard max length.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="maxLength">Maximum length (default 256).</param>
        /// <returns>The entity type builder for chaining.</returns>
        public static EntityTypeBuilder<TEntity> ConfigureRequiredString<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string propertyName,
            int maxLength = 256) where TEntity : class
        {
            builder.Property<string>(propertyName)
                .IsRequired()
                .HasMaxLength(maxLength);
            return builder;
        }

        /// <summary>
        /// Configures an optional string property with standard max length.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="maxLength">Maximum length (default 256).</param>
        /// <returns>The entity type builder for chaining.</returns>
        public static EntityTypeBuilder<TEntity> ConfigureOptionalString<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string propertyName,
            int maxLength = 256) where TEntity : class
        {
            builder.Property<string>(propertyName)
                .IsRequired(false)
                .HasMaxLength(maxLength);
            return builder;
        }

        /// <summary>
        /// Configures multi-tenancy support by adding a required TenantId foreign key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="tenantIdPropertyName">The tenant ID property name (default "TenantId").</param>
        /// <returns>The entity type builder for chaining.</returns>
        /// <remarks>
        /// This sets up the FK constraint and index but does NOT configure query filters.
        /// Future enhancement: automatic query filtering by tenant context.
        /// </remarks>
        public static EntityTypeBuilder<TEntity> ConfigureTenancy<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            string tenantIdPropertyName = "TenantId") where TEntity : class
        {
            builder.Property<Guid>(tenantIdPropertyName).IsRequired();
            builder.HasIndex(tenantIdPropertyName);
            return builder;
        }

        /// <summary>
        /// Configures soft delete support with IsDeleted flag.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <param name="applyQueryFilter">Automatically filter out deleted records in queries.</param>
        /// <returns>The entity type builder for chaining.</returns>
        public static EntityTypeBuilder<TEntity> ConfigureSoftDelete<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            bool applyQueryFilter = true) where TEntity : class
        {
            builder.Property<bool>("IsDeleted").HasDefaultValue(false);
            builder.HasIndex("IsDeleted");

            if (applyQueryFilter)
            {
                builder.HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));
            }

            return builder;
        }
    }
}
