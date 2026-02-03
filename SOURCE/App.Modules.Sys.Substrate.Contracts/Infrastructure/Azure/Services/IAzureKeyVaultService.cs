using App.Modules.Sys.Infrastructure.Azure.Models.Implementations.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Modules.Sys.Infrastructure.Azure.Services
{



    /// <summary>
    /// Base Contract for an Infrastructure Service to 
    /// to manage access to an Azure KeyVault.
    /// </summary>
    public interface IAzureKeyVaultService
    {

        /// <summary>
        /// Gets or sets the standard key divider character ('-').
        /// <para>
        /// Whereas AppHost keys can contain ':', etc. -- KeyVault cannot, so 
        /// they must be converted to this character (eg: '-', or '_', or maybe even '')
        /// </para>
        /// </summary>
        string CleanKeyName(string key);

        ///// <summary>
        ///// Retrieve a <see cref="JsonWebKey"/> 
        ///// containing the key, secret,
        ///// </summary>
        ///// <param name="secretKey"></param>
        ///// <param name="vaultUrl"></param>
        ///// <returns></returns>
        //Task<JsonWebKey> RetrieveKeyAsync(string secretKey, string vaultUrl = null);

        /// <summary>
        /// Retrieve a Secret value (not the whole key) from the Keystore.
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="vaultUrl"></param>
        /// <returns></returns>
        Task<string> RetrieveSecretAsync(string secretKey, string? vaultUrl = null);

        /// <summary>
        /// Sets a string secret in the (remote)
        /// keystore
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="secret"></param>
        /// <param name="vaultUrl"></param>
        /// <returns></returns>
        Task<AppSecretBundle> SetSecretAsync(string secretKey, string secret, string vaultUrl);

        /// <summary>
        /// Gets a dictionary of keys/secrets from the keyvalue.
        /// </summary>
        /// <param name="returnFQIdentifier"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyVaultUrl"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetSecretsAsync(bool returnFQIdentifier = false, int pageSize = 0, string? keyVaultUrl = null);


        /// <summary>
        /// Returns a list of the key names (without their secrets).
        /// </summary>
        /// <param name="returnFQIdentifier"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyVaultUrl"></param>
        /// <returns></returns>
        Task<string[]> ListSecretKeysAsync(bool returnFQIdentifier = false, int pageSize = 0, string? keyVaultUrl = null);



        /// <summary>
        ///     Create a Configuration object and fill properties from KeyVault Secrets with the same name.
        ///     <para>
        ///         Note that default values are not provided if the property value = default(T)
        ///     </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix"></param>
        /// <param name="keyVaultUrl"></param>
        /// <returns></returns>
        T GetObject<T>(string? prefix = null, string? keyVaultUrl = null) where T : class;
    }
}
