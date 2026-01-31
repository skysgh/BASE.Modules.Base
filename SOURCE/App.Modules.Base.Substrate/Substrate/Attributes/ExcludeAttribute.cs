namespace App.Modules.Base.Substrate.Attributes
{

    /// <summary>
    /// Attribute to exclude something from being processed
    /// (ie, ignored)
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="reason"></param>
    /// <exception cref="NotImplementedException"></exception>
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class ExcludeAttribute(string reason) : Attribute
    {
        /// <summary>
        /// Reason Item is being excluded from processing.
        /// </summary>
        public string Reason { get; } = reason;
    }
}