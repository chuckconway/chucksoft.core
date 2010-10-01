namespace Chucksoft.Core.Services
{
    public interface ICryptographyService
    {
        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <returns></returns>
        string Decrypt(string encryptedText);

        /// <summary>
        /// Raws the decrypt.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string RawDecrypt(byte[] text);

        /// <summary>
        /// Raws the encrypt.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        byte[] RawEncrypt(string text);

        /// <summary>
        /// Encrypts the hex encoded.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns></returns>
        string EncryptHexEncoded(string clearText);

        /// <summary>
        /// Decrypts the hex encoded.
        /// </summary>
        /// <param name="hexEncodedText">The hex encoded text.</param>
        /// <returns></returns>
        string DecryptHexEncoded(string hexEncodedText);
    }
}