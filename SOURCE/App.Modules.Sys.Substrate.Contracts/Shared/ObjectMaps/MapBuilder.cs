using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Modules.Sys.Shared.ObjectMaps
{
    /// <summary>
    /// Fluent builder for configuring object mappings.
    /// Provides IntelliSense without coupling to specific mapping library.
    /// </summary>
    /// <typeparam name="TFrom">Source type</typeparam>
    /// <typeparam name="TTo">Destination type</typeparam>
    public class MapBuilder<TFrom, TTo>
    {
        internal List<MappingRule> Rules { get; } = new();

        /// <summary>
        /// Ignore a property on the destination.
        /// Property will not be mapped even if source has matching property.
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="property">Property to ignore (IntelliSense available!)</param>
        /// <returns>Builder for chaining</returns>
        /// <example>
        /// CreateMap()
        ///     .Ignore(dest => dest.CalculatedField);
        /// </example>
        public MapBuilder<TFrom, TTo> Ignore<TProp>(
            Expression<Func<TTo, TProp>> property)
        {
            Rules.Add(new MappingRule
            {
                Type = RuleType.Ignore,
                DestinationProperty = GetPropertyName(property)
            });
            return this;
        }

        /// <summary>
        /// Map destination property from custom source expression.
        /// Use when property names don't match or need transformation.
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="destination">Destination property (IntelliSense!)</param>
        /// <param name="source">Source expression (IntelliSense!)</param>
        /// <returns>Builder for chaining</returns>
        /// <example>
        /// CreateMap()
        ///     .MapFrom(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
        /// </example>
        public MapBuilder<TFrom, TTo> MapFrom<TProp>(
            Expression<Func<TTo, TProp>> destination,
            Expression<Func<TFrom, TProp>> source)
        {
            Rules.Add(new MappingRule
            {
                Type = RuleType.MapFrom,
                DestinationProperty = GetPropertyName(destination),
                SourceExpression = source
            });
            return this;
        }

        /// <summary>
        /// Apply custom transformation function.
        /// Use for complex logic that can't be expressed as expression.
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="destination">Destination property (IntelliSense!)</param>
        /// <param name="transformer">Transform function</param>
        /// <returns>Builder for chaining</returns>
        /// <example>
        /// CreateMap()
        ///     .Transform(dest => dest.Email, src => src.Email.ToLower());
        /// </example>
        public MapBuilder<TFrom, TTo> Transform<TProp>(
            Expression<Func<TTo, TProp>> destination,
            Func<TFrom, TProp> transformer)
        {
            Rules.Add(new MappingRule
            {
                Type = RuleType.Transform,
                DestinationProperty = GetPropertyName(destination),
                TransformFunction = transformer
            });
            return this;
        }

        /// <summary>
        /// Set constant value for destination property.
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="destination">Destination property (IntelliSense!)</param>
        /// <param name="value">Constant value</param>
        /// <returns>Builder for chaining</returns>
        /// <example>
        /// CreateMap()
        ///     .SetValue(dest => dest.Type, "User");
        /// </example>
        public MapBuilder<TFrom, TTo> SetValue<TProp>(
            Expression<Func<TTo, TProp>> destination,
            TProp value)
        {
            Rules.Add(new MappingRule
            {
                Type = RuleType.Constant,
                DestinationProperty = GetPropertyName(destination),
                ConstantValue = value
            });
            return this;
        }

        /// <summary>
        /// Apply condition - only map if predicate is true.
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="destination">Destination property</param>
        /// <param name="source">Source expression</param>
        /// <param name="condition">Condition predicate</param>
        /// <returns>Builder for chaining</returns>
        /// <example>
        /// CreateMap()
        ///     .MapFromWhen(
        ///         dest => dest.Email,
        ///         src => src.Email,
        ///         src => src.IsActive);
        /// </example>
        public MapBuilder<TFrom, TTo> MapFromWhen<TProp>(
            Expression<Func<TTo, TProp>> destination,
            Expression<Func<TFrom, TProp>> source,
            Func<TFrom, bool> condition)
        {
            Rules.Add(new MappingRule
            {
                Type = RuleType.Conditional,
                DestinationProperty = GetPropertyName(destination),
                SourceExpression = source,
                Condition = condition
            });
            return this;
        }

        private static string GetPropertyName<T>(Expression<T> expression)
        {
            if (expression.Body is MemberExpression member)
            {
                return member.Member.Name;
            }
            
            if (expression.Body is UnaryExpression unary && 
                unary.Operand is MemberExpression unaryMember)
            {
                return unaryMember.Member.Name;
            }

            throw new ArgumentException("Expression must be a property access", nameof(expression));
        }
    }

    /// <summary>
    /// Mapping rule types
    /// </summary>
    internal enum RuleType
    {
        Ignore,
        MapFrom,
        Transform,
        Constant,
        Conditional
    }

    /// <summary>
    /// Internal representation of mapping rule
    /// </summary>
    internal sealed class MappingRule
    {
        public RuleType Type { get; set; }
        public string? DestinationProperty { get; set; }
        public object? SourceExpression { get; set; }
        public object? TransformFunction { get; set; }
        public object? ConstantValue { get; set; }
        public object? Condition { get; set; }
    }
}
