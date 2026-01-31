namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for an object which stores serialized Type and value.
    /// </summary>
    public interface IHasSerializedTypeValueNullable
    {
        /// <summary>
        /// The Serialised Name of the Type of the value.
        /// </summary>
        string SerializedTypeName { get; set; }

        /// <summary>
        /// The serialized value
        /// </summary>
        string? SerializedTypeValue { get; set; }
    }
}