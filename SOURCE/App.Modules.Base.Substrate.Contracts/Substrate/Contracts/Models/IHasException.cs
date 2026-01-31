namespace App.Modules.Base.Substrate.Contracts.Models
{
    /// <summary>
    /// Contract for an object that refers to a thrown exception.
    /// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public interface IHasException : IHasException<Exception>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
    }

    /// <summary>
    /// Contract for an object that refers to a thrown exception.
    /// </summary>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public interface IHasException<TException> where TException : Exception
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        /// <summary>
        /// Gets or sets the optional/nullable exception.
        /// </summary>
        Exception? Exception { get; set; }

    }
}