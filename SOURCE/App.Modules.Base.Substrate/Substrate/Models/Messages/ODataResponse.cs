// using System.Collections.Generic;

namespace App.Modules.Base.Substrate.Models.Messages
{
    /// <summary>
    /// TODO: Describe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ODataResponse<T>
    {
        /// <summary>
        /// The value of the response
        /// </summary>
        public List<T>? Value { get; set; }
    }
}