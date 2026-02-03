// Extensions are always put in root namespace
// for maximum usability from elsewhere:

using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace App
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// <para>
    /// Class to allow for use of EF's Include statement
    /// from other assemblies, without needing to take 
    /// a direct reference to EF.
    /// Ie. App.Base.Domain Services and any specific Repositories 
    /// can use 'Include'. 
    /// </para>
    /// </summary>
    public static class QueryableEFExtensions
    {
        ///// <summary>
        ///// TODO: Improve documentation
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static IQueryable Include(this IQueryable source, string path)
        //{
        //    return EntityFrameworkQueryableExtensions.Include(source, path);
        //}

        /// <summary>
        /// TODO: Improve documentation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
            where T:class
        {
            return EntityFrameworkQueryableExtensions.Include<T>(source, path);
        }

        ///// <summary>
        ///// TODO: Improve documentation
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TProperty"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source,
        //    Expression<Func<T, TProperty>> path)
        //{
        //    return EntityFrameworkQueryableExtensions.Include<T>(source, path);
        //}
    }
}