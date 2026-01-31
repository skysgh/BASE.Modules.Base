// using System;

namespace App.Modules.Base.Substrate.Attributes
{

    /// <summary>
    /// TODO: Describe better
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="roles"></param>
    [AttributeUsage(AttributeTargets.All)]
    public class RoleSecuredDtoModelAttributeAttribute(string roles) : Attribute
    {

        /// <summary>
        /// Get/set the Roles 
        /// <para>
        /// TODO: Describe use better.
        /// </para>
        /// </summary>
        public string Roles { get; set; } = roles;
    }
}