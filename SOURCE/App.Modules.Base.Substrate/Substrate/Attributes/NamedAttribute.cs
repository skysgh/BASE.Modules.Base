namespace App.Modules.Base.Substrate.Attributes
{
    /// <summary>
    /// Attribute to uniquely Key Types
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="key"></param>
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute(string key) : Attribute
    {

        /// <summary>
        /// The unique key.
        /// </summary>
        public string Key { get; set; } = key;

    }
}
