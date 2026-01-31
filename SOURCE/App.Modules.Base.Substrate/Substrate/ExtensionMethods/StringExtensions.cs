// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace App.Modules.Base.Substrate.ExtensionMethods
{
    /// <summary>
    /// Extensions to Strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Easier way to see if a string contains a given string 
        /// in a case insensitive manner.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <param name="stringComparison"></param>
        /// <returns></returns>
        public static bool Contains(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
#pragma warning disable IDE0075 // Simplify conditional expression
            return text == null || value == null ? false : text.Contains(value, stringComparison);
#pragma warning restore IDE0075 // Simplify conditional expression
        }


        /// <summary>
        /// Pluralise nouns
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SimplePluralise(this string text)
        {
#pragma warning disable IDE0046 // Convert to conditional expression
            if (text.LastOrDefault(' ') == 'y')
            {
#pragma warning disable IDE0057 // Use range operator
                return $"{text.Substring(0, text.Length - 1)}ies";
#pragma warning restore IDE0057 // Use range operator
            }
#pragma warning restore IDE0046 // Convert to conditional expression
            return text + 's';
        }
    }
}