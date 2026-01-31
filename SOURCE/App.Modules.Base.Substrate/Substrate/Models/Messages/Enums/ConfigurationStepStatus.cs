namespace App.Modules.Base.Substrate.Models.Messages.Enums
{
    /// <summary>
    /// The completion state of the Configuration Step
    /// </summary>
    public enum ConfigurationStepStatus
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Display the settings as neutral/ongoing.
        /// </summary>
        White = 1,
        /// <summary>
        /// Display as green/ok
        /// </summary>
        Green = 2,
        /// <summary>
        /// Display as orange. Has warnings
        /// </summary>
        Orange = 3,
        /// <summary>
        /// Display as red. Has errors.
        /// </summary>
        Red = 4,
    }
}