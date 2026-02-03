using App.Modules.Sys.Infrastructure.Azure.Models.Enums;
using App.Modules.Sys.Shared.Services;

namespace App.Modules.Sys.Infrastructure.Azure.Services
{
    /// <summary>
    /// Contract for an Infrastructure Service to 
    /// manage access to Azure Storage Accounts
    /// and the Storage Containers and Blobs within.
    /// <para>
    /// Without bleeding References to other assemblies that use
    /// the service (ie App.Base.Application, or App.ModuleXX.Infrastructure)
    /// Hence it's just offering Methods and not direct access to 
    /// Objects (Containers and Blobs)
    /// </para>
    /// </summary>
    public interface IAzureBlobStorageService : IHasService
    {

        // Internally using an App-specific object used to Wrap
        // any proprietary Blob object.
        // to protect service invokers from having a ref to the 
        // Azure Assembly.
        //AzureBlobStorageServiceConfiguration Configuration
        //{
        //    get;
        //}

        /// <summary>
        /// Ensure the named Container exists.
        /// <para>
        /// Method intended to be used during startup phase.
        /// </para>
        /// <para>
        /// Note that Method does not return any Object that would 
        /// drag an Assembly Reference to other Assemblies that invoke
        /// the service.
        /// </para>
        /// </summary>
        /// <param name="storageAccountContextKey"></param>
        /// <param name="containerName"></param>
        /// <param name="blobContainerPublicAccessTypeIfNew"></param>
        void EnsureContainer(string storageAccountContextKey, 
            string containerName,
            PublicBlobContainerAccessEnumerationType blobContainerPublicAccessTypeIfNew = 
              PublicBlobContainerAccessEnumerationType.Blobs);

        /// <summary>
        /// Upload a single text fragment to the named Blob Storage.
        /// <para>
        /// Note that Method does not return any Object that would 
        /// drag an Assembly Reference to other Assemblies that invoke
        /// the service.
        /// </para>
        /// </summary>
        /// <param name="storageAccountContextKey"></param>
        /// <param name="containerName"></param>
        /// <param name="remoteBlobName"></param>
        /// <param name="text"></param>
        void UploadAText(string storageAccountContextKey, string containerName, string remoteBlobName, string text);

        /// <summary>
        /// Download a single text fragment to the named Blob Storage.
        /// <para>
        /// Note that Method does not return any Object that would 
        /// drag an Assembly Reference to other Assemblies that invoke
        /// the service.
        /// </para>
        /// </summary>
        /// <param name="storageAccountContextKey"></param>
        /// <param name="containerName"></param>
        /// <param name="remoteBlobBame"></param>
        /// <returns></returns>
        string DownloadAText(string storageAccountContextKey, string containerName, string remoteBlobBame);

        /// <summary>
        /// Upload a single file fragment to the named Blob Storage.
        /// <para>
        /// Note that Method does not return any Object that would 
        /// drag an Assembly Reference to other Assemblies that invoke
        /// the service.
        /// </para>
        /// </summary>
        /// <param name="storageAccountContextKey"></param>
        /// <param name="containerName"></param>
        /// <param name="localFilePath"></param>
        /// <returns></returns>
        void UploadAFile(string storageAccountContextKey, string containerName, string localFilePath);



        //void Persist(byte[] bytes, string targetRelativePath);
        //void Persist(Stream contents, string targetRelativePath);

    }
}
