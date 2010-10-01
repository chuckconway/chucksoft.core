using Chucksoft.Core.Cryptography;

namespace Chucksoft.Core.Services
{
    public class CryptographyService : ICryptographyService
    {
        /// <summary>
        /// Encrypts the specified plain text.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            string encryptText = RijndaelCryptography.Encrypt(plainText);
            return encryptText;
        }

        /// <summary>
        /// Decrypts the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <returns></returns>
        public string Decrypt(string encryptedText)
        {
            string decryptText = RijndaelCryptography.Decrypt(encryptedText);
            return decryptText;
        }

        /// <summary>
        /// Raws the decrypt.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public string RawDecrypt(byte[] text)
        {
            return RijndaelCryptography.RawDecrypt(text);
        }

        /// <summary>
        /// Raws the encrypt.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public byte[] RawEncrypt(string text)
        {
            return RijndaelCryptography.RawEncrypt(text);
        }

        /// <summary>
        /// Encrypts the hex encoded.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>Hex encoded String</returns>
        public string EncryptHexEncoded(string clearText)
        {
            return RijndaelCryptography.EncryptHexEncoded(clearText);
        }

        /// <summary>
        /// Decrypts the hex encoded.
        /// </summary>
        /// <param name="hexEncodedText">The hex encoded text.</param>
        /// <returns>clear Text</returns>
        public string DecryptHexEncoded(string hexEncodedText)
        {
            return RijndaelCryptography.DecryptHexEncoded(hexEncodedText);
        }
    }
}