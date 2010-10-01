using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Web.Configuration;

namespace Chucksoft.Core.Cryptography
{
    /// <summary>
    /// Provides encryption services that use the ASP.NET machine key.
    /// </summary>
    public static class MachineKeyEncryption
    {
        /// <summary>
        /// Encrypts an array of bytes using the ASP.NET machine key.
        /// </summary>
        /// <param name="dataToEncrypt">The data to encrypt.</param>
        /// <returns>The encrypted data.</returns>
        public static Byte[] Encrypt(Byte[] dataToEncrypt)
        {
            // Build a stream to hold the newly encrypted data.
            var encryptedData = new MemoryStream();

            // Build the encryption transform and crypto stream.
            ICryptoTransform encryptionTransform = GetEncryptorTransform();
            var encryptionStream = new CryptoStream(encryptedData, encryptionTransform, CryptoStreamMode.Write);

            // Write the unencrypted data to the encryption stream.
            encryptionStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
            encryptionStream.FlushFinalBlock();

            // Return the contents of the newly encrypted data stream.
            return encryptedData.ToArray();
        }

        /// <summary>
        /// Decrypts an array of bytes using the ASP.NET machine key.
        /// </summary>
        /// <param name="dataToDecrypt">The encrypted data.</param>
        /// <returns>The unencrypted data.</returns>
        public static Byte[] Decrypt(Byte[] dataToDecrypt)
        {
            // Build a stream to hold the encrypted data.
            var decryptedData = new MemoryStream();

            // Build the decryption transform and crypto stream.
            ICryptoTransform decryptionTransform = GetDecryptorTransform();
            var decryptionStream = new CryptoStream(decryptedData, decryptionTransform, CryptoStreamMode.Write);

            // Write the encrypted data to the decryption stream.
            decryptionStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
            decryptionStream.FlushFinalBlock();

            // Return the contents of the newly decrypted data stream.
            return decryptedData.ToArray();
        }

        /// <summary>
        /// Gets the machine key encryption transform.
        /// </summary>
        /// <returns></returns>
        private static ICryptoTransform GetEncryptorTransform()
        {
            SymmetricAlgorithm encryptionAlgorithm = GetEncryptionAlgorithm();
            return encryptionAlgorithm.CreateEncryptor();
        }

        /// <summary>
        /// Gets the machine key decryption transform.
        /// </summary>
        /// <returns></returns>
        private static ICryptoTransform GetDecryptorTransform()
        {
            SymmetricAlgorithm encryptionAlgorithm = GetEncryptionAlgorithm();
            return encryptionAlgorithm.CreateDecryptor();
        }

        /// <summary>
        /// Builds a symetric encryption algorithm instance that is pre-initialized
        /// with the key and IV.
        /// </summary>
        /// <returns>A symetric encryption algorithm instance.</returns>
        private static SymmetricAlgorithm GetEncryptionAlgorithm()
        {
            SymmetricAlgorithm retVal;

            // Retrieve the machine key configuration section.
            var machineKeySection = (MachineKeySection) ConfigurationManager.GetSection("system.web/machineKey");

            // Get the encryption key.
            Byte[] encryptionKey = HexStringToBytes(machineKeySection.DecryptionKey);

            // Build the appropriate SymetricAlgorithm.
            switch (machineKeySection.Decryption)
            {
                case "3DES":
                    retVal = new TripleDESCryptoServiceProvider();
                    break;

                case "DES":
                    retVal = new DESCryptoServiceProvider();
                    break;

                case "AES":
                    retVal = new RijndaelManaged();
                    break;

                default:
                    if (encryptionKey.Length == 8)
                    {
                        retVal = new DESCryptoServiceProvider();
                    }
                    else
                    {
                        retVal = new RijndaelManaged();
                    }
                    break;
            }

            // Initialize the algorithm with the encryption key.
            if ((encryptionKey.Length == 0x18) && (retVal is DESCryptoServiceProvider))
            {
                // I'm not sure why this shortening happens but the ASP.NET implementation does it.
                var shortenedKey = new byte[8];
                Buffer.BlockCopy(encryptionKey, 0, shortenedKey, 0, 8);
                encryptionKey = shortenedKey;
            }
            retVal.Key = encryptionKey;

            // Initialize the algorithm with an all 0 initialization vector.
            retVal.GenerateIV();
            retVal.IV = new Byte[retVal.IV.Length];

            return retVal;
        }

        /// <summary>
        /// Decodes a hex string to an array of bytes.
        /// </summary>
        /// <returns>The bytes that the hex string represented.</returns>
        private static Byte[] HexStringToBytes(String str)
        {
            if ((str.Length & 1) == 1)
            {
                return null;
            }

            var buffer = new byte[0x67];
            int index = buffer.Length;
            while (--index >= 0)
            {
                if ((0x30 <= index) && (index <= 0x39))
                {
                    buffer[index] = (byte) (index - 0x30);
                }
                else
                {
                    if ((0x61 <= index) && (index <= 0x66))
                    {
                        buffer[index] = (byte) ((index - 0x61) + 10);
                        continue;
                    }
                    if ((0x41 <= index) && (index <= 70))
                    {
                        buffer[index] = (byte) ((index - 0x41) + 10);
                    }
                }
            }

            var buffer2 = new byte[str.Length/2];
            int num2 = 0;
            int num3 = 0;
            int length = buffer2.Length;
            while (--length >= 0)
            {
                int num5;
                int num6;
                try
                {
                    num5 = buffer[str[num2++]];
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (ArgumentException)
                {
                    return null;
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
                try
                {
                    num6 = buffer[str[num2++]];
                }
                catch (ArgumentNullException)
                {
                    return null;
                }
                catch (ArgumentException)
                {
                    return null;
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
                buffer2[num3++] = (byte) ((num5 << 4) + num6);
            }
            return buffer2;
        }
    }
}