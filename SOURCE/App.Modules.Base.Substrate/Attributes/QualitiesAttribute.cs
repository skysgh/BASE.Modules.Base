using App.Modules.Base.Substrate.Infrastructure.Outcomes;

namespace App.Modules.Base.Substrate.Attributes
{
    /// <summary>
    /// The System, Data, In Use Qualities addressed.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.All)]
    public class QualitiesAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the system qualities addressed.
        /// </summary>
        public SystemQualities SystemQualities { get; private set; }
        /// <summary>
        /// Gets or sets the system data qualities addressed.
        /// </summary>
        public SystemDataQualities SystemDataQualities { get; private set; }
        /// <summary>
        /// Gets or sets the system in use qualities addressed.
        /// </summary>
        public SystemInUseQualities SystemInUseQualities { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="systemQualities"></param>
        /// <param name="systemDataQualities"></param>
        /// <param name="systemInUseQualities"></param>
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0290 // Use primary constructor
        public QualitiesAttribute(SystemQualities systemQualities, SystemDataQualities systemDataQualities, SystemInUseQualities systemInUseQualities)
#pragma warning restore IDE0290 // Use primary constructor
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            SystemQualities = systemQualities;
            SystemDataQualities = systemDataQualities;
            SystemInUseQualities = systemInUseQualities;
        }
    }


}