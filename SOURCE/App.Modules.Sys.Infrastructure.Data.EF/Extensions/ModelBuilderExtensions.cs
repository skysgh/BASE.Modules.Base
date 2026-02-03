using App.Modules.Sys.Infrastructure.Data.EF.Constants;
using App.Modules.Sys.Shared.Models;
using App.Modules.Sys.Shared.Models.Persistence;
using App.Modules.Sys.Substrate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

// Note how namespace does not match folders.
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Class of Extension methods to 
    /// a Db ModelBuilder, to provide 
    /// more consistency to the development 
    /// of Entity model definitions.
    /// <para>
    /// Note: 
    /// In general, a method accepts a <c>ref int order</c>
    /// and increments it each time, in order to specify 
    /// the column location.
    /// </para>
    /// </summary>
    public static class ModelBuilderExtensions
    {

        /// <summary>
        /// Defines the specs of a TenantFK column for the storage 
        /// of entities that implement <see cref="IHasTenantFK"/>.
        /// <para>
        /// The property is indexed, but not unique.
        /// </para>
        /// <para>
        /// Note: 
        /// The propery is usually put in first place, 
        /// before getting on with defining the attributes
        /// of the record (Id, Accountability, Text, etc.)
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="indexTenantFK"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineTenantFK<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool indexTenantFK = true,
            Func<int, int>? injectedPropertyDefs = null)
            where TEntity : class, IHasTenantFK
        {
            // Define Property:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.TenantFK)
                .HasColumnOrder(order++)
                .IsRequired()
                ;
            if (indexTenantFK)
            {
                // Define non-unique index:
                modelBuilder.Entity<TEntity>()
                    .HasIndex(x => x.TenantFK)
                    .HasDatabaseName($"IX_{typeof(TEntity).Name}_TenantFK")
                    .IsUnique(false);
                ;
            }
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);

            //modelBuilder.Entity<TEntity>()
            //    .HasRequired(x => x.Tenant)
            //    .WithMany()
            //    .HasForeignKey(x => x.TenantFK)
            //    .WillCascadeOnDelete(false);

        }

        /// <summary>
        /// Defines the specs of a GUID ID column for the storage 
        /// of entities that implement <see cref="IHasId{TId}"/>
        /// where the type is struct/GUID.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="applyIdIndex"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineCustomId<TEntity, TId>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyIdIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
            where TEntity : class, IHasId<TId>
            where TId : struct
        {
            modelBuilder.Entity<TEntity>()
                .HasKey(x => x.Id);

            //Define property:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.Id)
                .HasColumnOrder(order++)
                .ValueGeneratedNever()
                .IsRequired();

            if (applyIdIndex == true)
            {

                modelBuilder.Entity<TEntity>()
                    .HasIndex(x => x.Id)
                    .HasDatabaseName($"IX_{typeof(TEntity).Name}_Id")
                    .IsUnique(true);
            }
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
        }


        /// <summary>
        /// Defines the specs for the storage 
        /// of entities that implement <see cref="IHasKey"/>
        /// and applies a non-unique index.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="applyIdIndex"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineUniqueKey<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            int? size = DbFieldConstants.StringLengthTiny,
            bool applyIdIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
    where TEntity : class, IHasKey
        {
            InternalDefineNonUniqueKey<TEntity>(
                modelBuilder,
                ref order,
                isUnique: true,
                size: size,
                applyIdIndex: applyIdIndex,
                injectedPropertyDefs: injectedPropertyDefs
                );
        }


        /// <summary>
        /// Defines the specs for the storage 
        /// of entities that implement <see cref="IHasKey"/>
        /// and applies a non-unique index.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="applyIdIndex"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineNonUniqueKey<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            int? size = DbFieldConstants.StringLengthTiny,
            bool applyIdIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
where TEntity : class, IHasKey
        {
            InternalDefineNonUniqueKey<TEntity>(
                modelBuilder,
                ref order,
                isUnique: false,
                size: size,
                applyIdIndex: applyIdIndex,
                injectedPropertyDefs: injectedPropertyDefs
                );
        }


        private static void InternalDefineNonUniqueKey<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool isUnique = false,
            int? size = DbFieldConstants.StringLengthTiny,
            bool applyIdIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
where TEntity : class, IHasKey

        {
            if (size == null)
            {
                size = DbFieldConstants.StringLengthTiny;
            }
            modelBuilder.Entity<TEntity>()
                .Property(x => x.Key)
                .HasColumnOrder(order++)
                .HasMaxLength((int)size)
                .IsRequired();

            if (applyIdIndex)
            {
                modelBuilder.Entity<TEntity>()
                    .HasIndex(x => x.Key)
                    .HasDatabaseName($"IX_{typeof(TEntity).Name}_Key")
                    .IsUnique(isUnique)
                    ;
            }
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
        }

        /// <summary>
        /// Defines the specs for the storage 
        /// of entities that implement
        /// probably have already specified TenantFK and/or Id
        /// and now moving on to:
        /// <see cref="IHasTimestamp"/>
        /// </summary>
        public static void DefineTimestamp<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            Func<int, int>? injectedPropertyDefs = null)
        where TEntity : class, IHasTimestamp
        {
            modelBuilder.Entity<TEntity>()
                .Property(x => x.Timestamp)
                .IsConcurrencyToken()
                .IsRowVersion()
                .HasColumnOrder(order++)
                //.IsRequired()
                ;
            //After every property:
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
        }


        /// <summary>
        /// Define a RecordState property.
        /// <para>
        /// This is usually invoked after a
        /// Timestamp column.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="applyRecordStateIndex"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineRecordState<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyRecordStateIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
        where TEntity : class, IHasRecordState
        {
            modelBuilder.Entity<TEntity>()
                .Property(x => x.RecordState)
                .HasColumnOrder(order++)
                .IsRequired()
                ;

            if (applyRecordStateIndex)
            {
                modelBuilder.Entity<TEntity>()
                    .HasIndex(x => x.RecordState)
                    .HasDatabaseName($"IX_{typeof(TEntity).Name}_RecordState")
                    .IsUnique(false)
                    ;
            }

            //After every property:
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
        }

        /// <summary>
        /// Define columns for Auditability requirements.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="applyLastModifiedIndex"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineRecordAuditability<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyLastModifiedIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
        where TEntity : class, IHasInRecordAuditability
        {
            modelBuilder.Entity<TEntity>()
                .Property(x => x.CreatedOnUtc)
                .HasColumnOrder(order++)
                .IsRequired();
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);

            //6:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.CreatedByPrincipalId)
                .HasColumnOrder(order++)
                .HasMaxLength(DbFieldConstants.GuidStringLength)
                .IsRequired();
            //After every property:
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);

            //7:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.LastModifiedOnUtc)
                .HasColumnOrder(order++)
                .IsRequired();
            if (applyLastModifiedIndex)
            {
                modelBuilder.Entity<TEntity>()
                    .HasIndex(x => x.LastModifiedOnUtc)
                    .HasDatabaseName($"IX_{typeof(TEntity).Name}_LastModifiedOnUtc")
                    .IsUnique(false);
                //After every property:
                InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
            }

            //8:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.LastModifiedByPrincipalId)
                .HasColumnOrder(order++)
                .HasMaxLength(DbFieldConstants.GuidStringLength)
                .IsRequired();
            if (applyLastModifiedIndex)
            {
                modelBuilder.Entity<TEntity>()
                .HasIndex(x => x.LastModifiedByPrincipalId)
                .HasDatabaseName($"IX_{typeof(TEntity).Name}_LastModifiedByPrincipalId")
                .IsUnique(false)
                ;
                //After every field:
                InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
            }

            //9:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.DeletedOnUtc)
                .HasColumnOrder(order++)
                //IsOptional
                ;
            //After Every field
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);

            //10:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.DeletedByPrincipalId)
                .HasColumnOrder(order++)
                .HasMaxLength(DbFieldConstants.GuidStringLength)
                //IsOptional
                ;
            //After Every field
            InvokeInjectedPropertyDefsIfAny(ref order, injectedPropertyDefs);
        }

        /// <summary>
        /// Defines the specs for the storage 
        /// of entities that implement
        /// probably have already specified TenantFK and/or Id
        /// and now moving on to:
        /// <see cref="IHasTimestamp"/>,
        /// <see cref="IHasInRecordAuditability"/>,
        /// <see cref="IHasRecordState"/>
        /// <para>
        /// Note:
        /// Invoked *after* the Id column has been defined.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="applyLastModifiedIndex"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineTimestampedAuditedRecordStated<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyLastModifiedIndex = true,
            Func<int, int>? injectedPropertyDefs = null)
        where TEntity : class,
            IHasTimestamp,
            IHasRecordState,
            IHasInRecordAuditability
        {
            //3:
            DefineTimestamp<TEntity>(
                modelBuilder: modelBuilder,
                order: ref order,
                injectedPropertyDefs: injectedPropertyDefs);

            //4:
            DefineRecordState<TEntity>(
                modelBuilder: modelBuilder,
                order: ref order,
                applyRecordStateIndex: true,
                injectedPropertyDefs: injectedPropertyDefs);


            //5-10:
            DefineRecordAuditability<TEntity>(
                modelBuilder: modelBuilder,
                order: ref order,
                applyLastModifiedIndex: applyLastModifiedIndex,
                injectedPropertyDefs: injectedPropertyDefs);

        }




        /// <summary>
        /// A Db ModelBuilder ExtensionMethod 
        /// to describe the schema required for entities that implement
        /// <see cref="IHasEnabled"/>
        /// contract.
        /// <para>
        /// Increments the passed <c>order</c> property
        /// setting it up for the next field description.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="injectedPropertyDefs">Reserved for future extensibility</param>
        public static void DefineRequiredEnabled<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
#pragma warning disable IDE0060 // Remove unused parameter - Reserved for future property injection scenarios
            Func<int, int>? injectedPropertyDefs = null)
#pragma warning restore IDE0060
            where TEntity : class, IHasEnabled
        {
            modelBuilder.Entity<TEntity>()
                .Property(x => x.Enabled)
                .HasColumnOrder(order++)
                .IsRequired();
        }


        /// <summary>
        /// A Db ModelBuilder ExtensionMethod 
        /// to describe the schema required for entities that implement
        /// <see cref="IHasTitleAndDescription"/>
        /// contract.
        /// <para>
        /// Note:
        /// Invoked after TenantFK, Id, Timestamp, Auditability
        /// columns have been defined.
        /// </para>
        /// <para>
        /// Note that it increments the passed <c>ref order</c> property
        /// setting it up for reuse in subsequent field descriptions.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="titleFieldLength"></param>
        /// <param name="applyTitleIndex"></param>
        /// <param name="descriptionMaxLength"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineRequiredTitleAndDescription<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyTitleIndex = true,
            int? titleFieldLength = DbFieldConstants.StringLengthTiny,
            int? descriptionMaxLength = DbFieldConstants.StringLengthLarge,
            Func<int, int>? injectedPropertyDefs = null)
            where TEntity : class, IHasTitle, IHasDescription
        {
            //In case a null was passed to skip to the injectedPropertyDefs

            if (titleFieldLength == null)
            {
                titleFieldLength = DbFieldConstants.StringLengthTiny;
            }
            if (descriptionMaxLength == null)
            {
                descriptionMaxLength = DbFieldConstants.StringLengthLarge;
            }



            modelBuilder.Entity<TEntity>()
                .Property(x => x.Title)
                .HasColumnOrder(order++)
                .HasMaxLength((int)titleFieldLength)
                //.HasColumnAnnotation("Index",
                //    new IndexAnnotation(new IndexAttribute($"IX_{typeof(TEntity).Name}_Title")
                //    {
                //        IsUnique = false
                //    }))
                .IsRequired()
                ;

            if (applyTitleIndex)
            {
                modelBuilder.Entity<TEntity>()
                    .HasIndex(x => x.Title)
                    .HasDatabaseName($"IX_{typeof(TEntity).Name}_Title")
                    .IsUnique(false)
                    ;
            }

            //Invoke after every field:
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }

            if (descriptionMaxLength != DbFieldConstants.StringLengthMax)
            {
                modelBuilder.Entity<TEntity>()
                    .Property(x => x.Description)
                    .HasColumnOrder(order++)
                    .HasMaxLength((int)descriptionMaxLength)
                    .IsRequired()
                    ;
            }
            //Invoke after every field:
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }
        }


        /// <summary>
        /// A Db ModelBuilder ExtensionMethod 
        /// to describe the schema required for entities that implement
        /// <see cref="IHasEnabled"/>
        /// and
        /// <see cref="IHasTitleAndDescription"/>
        /// contract.
        /// <para>
        /// Note that it increments the passed <c>ref order</c> property
        /// setting it up for reuse in subsequent field descriptions.
        /// </para>
        /// <para>
        /// Internally it invokes
        /// <see cref="DefineRequiredEnabled{T}(ModelBuilder, ref int, Func{int, int})"/>
        /// before invoking
        /// <see cref="DefineRequiredTitleAndDescription{T}(ModelBuilder, ref int, bool, int?, int?, Func{int, int})"/>
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="applyTitleIndex"></param>
        /// <param name="titleFieldLength"></param>
        /// <param name="descriptionMaxLength"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineReferenceDataLite<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyTitleIndex = true,
            int? titleFieldLength = DbFieldConstants.StringLengthTiny,
            int? descriptionMaxLength = DbFieldConstants.StringLengthLarge,
            Func<int, int>? injectedPropertyDefs = null)
            where TEntity : class, IHasEnabled, IHasTitle, IHasDescription
        {
            //12:
            modelBuilder.DefineRequiredEnabled<TEntity>(ref order);
            //Invoke after every field:
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }

            //-- Might inject keys here...(if order == 12...)

            //13:
            modelBuilder.DefineRequiredTitleAndDescription<TEntity>(
                ref order,
                applyTitleIndex: applyTitleIndex,
                titleFieldLength: titleFieldLength,
                descriptionMaxLength: descriptionMaxLength,
                injectedPropertyDefs: injectedPropertyDefs
                );
        }


        /// <summary>
        /// A Db ModelBuilder ExtensionMethod 
        /// to describe the schema required for entities that implement
        /// <see cref="IHasReferenceDataOfNoIdEnabledTitleDescImgUrlDisplayHints"/>
        /// contract.
        /// <para>
        /// Note that it increments the passed <c>ref order</c> property
        /// setting it up for reuse in subsequent field descriptions.
        /// </para>
        /// <para>
        /// Internally, it invokes 
        /// <see cref="DefineReferenceDataLite{T}(ModelBuilder, ref int, bool, int?,int?, Func{int, int}?)"/>
        /// before setting up Order and Display Hints.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="order"></param>
        /// <param name="applyTitleIndex"></param>
        /// <param name="titleFieldLength"></param>
        /// <param name="descriptionMaxLength"></param>
        /// <param name="injectedPropertyDefs"></param>
        public static void DefineReferenceDataDisplayable<TEntity>(
            this ModelBuilder modelBuilder,
            ref int order,
            bool applyTitleIndex = true,
            int? titleFieldLength = DbFieldConstants.StringLengthTiny,
            int? descriptionMaxLength = DbFieldConstants.StringLengthLarge,
            Func<int, int>? injectedPropertyDefs = null
            )
            where TEntity : class,
            IHasEnabled,
            IHasTitle,
            IHasDescription,
            IHasImageUrl,
            IHasDisplayHints
        {

            // Invoke child Convention
            modelBuilder.DefineReferenceDataLite<TEntity>(
                ref order,
                applyTitleIndex: applyTitleIndex,
                titleFieldLength: titleFieldLength,
                descriptionMaxLength: descriptionMaxLength,
                injectedPropertyDefs: injectedPropertyDefs);

            modelBuilder.Entity<TEntity>()
                .Property(x => x.ImageUrl)
                .HasColumnOrder(order++)
                //.IsRequired()
                ;
            //Invoke after every field:
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }

            //14:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.DisplayOrderHint)
                .HasColumnOrder(order++)
                .IsRequired();
            //Invoke after every field:
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }

            //15:
            modelBuilder.Entity<TEntity>()
                .Property(x => x.DisplayStyleHint)
                .HasColumnOrder(order++)
                .HasMaxLength(DbFieldConstants.StringLengthTiny)
                //IsOptional
                ;
            //Invoke after every field:
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }
        }

        //        public static void DefineStandard(this DbModelBuilder modelBuilder, Expression<Func<TDto, object>> func)
        //        {
        //            ((MemberExpression)func.Body).Member.MemberType
        //            Func<TDto, object> p;
        //            p.)
        //            func.
        //            modelBuilder.Entity<Course>()
        //.Property(x => x.Key)
        //.HasColumnOrder(order++)
        //.IsRequired();

        //        }


        /// <summary>
        /// Helper method used after defining 
        /// each property in an entity model.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="injectedPropertyDefs"></param>
        private static void InvokeInjectedPropertyDefsIfAny(
            ref int order,
            Func<int, int>? injectedPropertyDefs)
        {
            if (injectedPropertyDefs != null)
            {
                order = injectedPropertyDefs.Invoke(order);
            }

        }


        /// <summary>
        /// Sets the default Schema for the model being built.
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure.</param>
        /// <param name="schemaKey">The schema name to use as default. If null or empty, no default schema is set.</param>
        /// <returns>The same model builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="modelBuilder"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// This extension method wraps EF Core's <see cref="ModelBuilder.HasDefaultSchema"/> 
        /// with additional null-safety checks for the schema key parameter.
        /// </para>
        /// <para>
        /// <b>Behavior:</b>
        /// - If <paramref name="schemaKey"/> is null or whitespace, no schema is set (tables go to database default, usually 'dbo')
        /// - If <paramref name="schemaKey"/> is a valid string, it's used as the default schema for all entities
        /// </para>
        /// <para>
        /// <b>Usage Example:</b>
        /// <code>
        /// protected override void OnModelCreating(ModelBuilder modelBuilder)
        /// {
        ///     modelBuilder.HasDefaultSchema(this.SchemaKey);
        ///     // All entities will now use this schema by default
        /// }
        /// </code>
        /// </para>
        /// </remarks>
        public static ModelBuilder HasDefaultSchema(this ModelBuilder modelBuilder, string? schemaKey)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);

            // Only set schema if a non-empty value is provided
            if (!string.IsNullOrWhiteSpace(schemaKey))
            {
                //recursive unless calling it explicitly.
                RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, schemaKey);
            }

            return modelBuilder;
        }
    }
}