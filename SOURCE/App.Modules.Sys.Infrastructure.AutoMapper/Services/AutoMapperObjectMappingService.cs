using App.Modules.Sys.Application.Services.ObjectMapping;
using App.Modules.Sys.Infrastructure.Services;
using App.Modules.Sys.Shared.ObjectMaps;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace App.Modules.Sys.Infrastructure.AutoMapper.Services
{
    /// <summary>
    /// AutoMapper implementation of IObjectMappingService.
    /// Plugin architecture - can be swapped with alternative mapping libraries.
    /// </summary>
    /// <remarks>
    /// This is the ONLY place in the entire solution that knows about AutoMapper.
    /// All other code uses IObjectMappingService abstraction.
    /// </remarks>
    public class AutoMapperObjectMappingService : IObjectMappingService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        /// <summary>
        /// Initialize with assemblies to scan for ObjectMapBase implementations.
        /// </summary>
        public AutoMapperObjectMappingService(params Assembly[] assembliesToScan)
        {
            // Discover all ObjectMapBase<,> implementations
            var mappers = new List<Type>();
            foreach (var assembly in assembliesToScan)
            {
                mappers.AddRange(ObjectMapDiscoveryService.DiscoverObjectMaps(assembly));
            }

            // Configure AutoMapper
            _configuration = new MapperConfiguration(cfg =>
            {
                // Register discovered mappers
                foreach (var mapperType in mappers)
                {
                    var mappingTypes = ObjectMapDiscoveryService.GetMappingTypes(mapperType);
                    if (mappingTypes.HasValue)
                    {
                        var (from, to) = mappingTypes.Value;
                        
                        // Create instance to get configuration
                        var instance = Activator.CreateInstance(mapperType);
                        if (instance is IObjectMap mapper)
                        {
                            // Get fluent builder configuration
                            var getConfigMethod = mapperType.GetMethod("GetMappingConfiguration");
                            var builder = getConfigMethod?.Invoke(instance, null);
                            
                            if (builder != null)
                            {
                                // Has custom configuration - apply rules
                                ApplyMapBuilderRules(cfg, from, to, builder);
                            }
                            else
                            {
                                // Pure convention - create simple map
                                var createMapMethod = typeof(IMapperConfigurationExpression)
                                    .GetMethod("CreateMap", Type.EmptyTypes);
                                
                                if (createMapMethod != null)
                                {
                                    var genericMethod = createMapMethod.MakeGenericMethod(from, to);
                                    genericMethod.Invoke(cfg, null);
                                }
                            }
                        }
                    }
                }
            });

            _mapper = _configuration.CreateMapper();
        }

        /// <summary>
        /// Apply MapBuilder rules to AutoMapper configuration
        /// </summary>
        private static void ApplyMapBuilderRules(IMapperConfigurationExpression cfg, Type from, Type to, object builder)
        {
            // Get rules from builder
            var rulesProperty = builder.GetType().GetProperty("Rules");
            if (rulesProperty?.GetValue(builder) is not System.Collections.IList rules)
            {
                return;
            }

            // Create AutoMapper map
            var createMapMethod = typeof(IMapperConfigurationExpression)
                .GetMethod("CreateMap", Type.EmptyTypes)?
                .MakeGenericMethod(from, to);
            
            var mapConfig = createMapMethod?.Invoke(cfg, null);
            if (mapConfig == null)
            {
                return;
            }

            // Apply each rule
            foreach (var rule in rules)
            {
                var ruleType = rule.GetType();
                var typeProperty = ruleType.GetProperty("Type");
                var destProperty = ruleType.GetProperty("DestinationProperty");
                var sourceExprProperty = ruleType.GetProperty("SourceExpression");
                
                var type = typeProperty?.GetValue(rule);
                var destProp = destProperty?.GetValue(rule) as string;
                
                // TODO: Convert our rules to AutoMapper ForMember calls
                // This is where we translate our API to AutoMapper's API
                // For now, just use convention-based mapping
            }
        }

        /// <inheritdoc/>
        public void SetConfiguration<T>(T configuration) where T : class
        {
            // Not needed for this implementation - configuration set in constructor
            throw new NotSupportedException("AutoMapper configuration is immutable after creation");
        }

        /// <inheritdoc/>
        public T GetConfiguration<T>() where T : class
        {
            return _configuration as T 
                ?? throw new InvalidOperationException($"Configuration is not of type {typeof(T).Name}");
        }

        /// <inheritdoc/>
        public T GetMapper<T>() where T : class
        {
            return _mapper as T
                ?? throw new InvalidOperationException($"Mapper is not of type {typeof(T).Name}");
        }

        /// <inheritdoc/>
        public TTarget Map<TSource, TTarget>(TSource source) where TSource : class where TTarget : new()
        {
            return _mapper.Map<TSource, TTarget>(source);
        }

        /// <inheritdoc/>
        public TDestination Map<TDestination>(object source)
        {
            return _mapper.Map<TDestination>(source);
        }

        /// <inheritdoc/>
        public object Map(object source, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, sourceType, destinationType);
        }

        /// <inheritdoc/>
        public TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TSource : class where TTarget : class
        {
            return _mapper.Map(source, target);
        }

        /// <inheritdoc/>
        public IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source) where TSource : class where TTarget : new()
        {
            return source.Select(item => _mapper.Map<TSource, TTarget>(item));
        }
    }
}
