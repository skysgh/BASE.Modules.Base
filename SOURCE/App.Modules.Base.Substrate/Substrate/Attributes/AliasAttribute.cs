namespace App.Modules.Base.Substrate.Attributes
{

    /// <summary>
    /// Attribute that allows associating multiple Aliases to a 
    /// a Property or Class, determined by Reflection.
    /// <para>
    /// Example Usage: 
    /// A Configuration object may allow several Aliases 
    /// for storing/finding a parameter. The ambiguity allows
    /// for common shortenings (eg: ID/Identifier) or Upper/Lower case, 
    /// arguably making it slightly easier to configure and 
    /// get up and going with less swearing.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="displayName"></param>
    [AttributeUsage(AttributeTargets.All)]
    public class AliasAttribute(string displayName) : Attribute
    {

        /// <summary>
        /// The Alias/Display Name for the property or object,
        /// determinable by Reflection.
        /// </summary>
        public string DisplayName { get; set; } = displayName;
    }
}