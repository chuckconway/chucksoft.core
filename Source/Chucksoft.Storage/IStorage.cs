namespace Chucksoft.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Creates the bucket.
        /// </summary>
        /// <param name="name">The name.</param>
        void CreateBucket(string name);

        /// <summary>
        /// Adds the file.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="file">The file.</param>
        void AddFile(string bucketName, string keyName, string contentType, byte[] file);

        /// <summary>
        /// Deletes the bucket.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        void DeleteBucket(string bucketName);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="keyName">Name of the key.</param>
        void DeleteFile(string bucketName, string keyName);

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <returns></returns>
        byte[] GetFile(string bucketName, string keyName);

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <returns></returns>
        string GetUrl(string bucketName, string keyName);
    }
}