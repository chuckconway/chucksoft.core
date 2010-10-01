using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Chucksoft.Core.Cryptography
{
    public class RijndaelCryptography
    {
        private const string _defaultIV = "ZVuQgawm0CN5LYn0";
        private const string _defaultKey = "x29uRQfMStaOPty1";

        private static readonly object _lock = new object();
        private static string _iv = string.Empty;
        private static string _key = string.Empty;

        /// <summary>
        /// Sets Config Key from Web.Config
        /// </summary>
        /// <returns></returns>
        private static string GetKey
        {
            get
            {
                const string key = "Key";
                SetStaticVariableValue(key, ref _key);

                return _key;
            }
        }

        /// <summary>
        /// Sets IV Key from Web.Config
        /// </summary>
        /// <returns></returns>
        private static string GetIV
        {
            get
            {
                const string key = "IV";
                SetStaticVariableValue(key, ref _iv);

                return _iv;
            }
        }

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        public static string Encrypt(string data, string key, string iv)
        {
            return Encode(EncryptStringToBytesAes(data, GetLegalKey(key), Encoding.ASCII.GetBytes(iv)));
        }

        /// <summary>
        /// Raws the encrypt.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] RawEncrypt(string data)
        {
            return EncryptStringToBytesAes(data, GetLegalKey(GetKey), Encoding.ASCII.GetBytes(GetIV));
        }

        /// <summary>
        /// Encrypts the hex encoded.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <returns>Hex Encoded String</returns>
        public static string EncryptHexEncoded(string clearText)
        {
            byte[] encryptedBytes = RawEncrypt(clearText);
            return HexEncoding.ToString(encryptedBytes);
        }

        /// <summary>
        /// Raws the decrypt.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string RawDecrypt(byte[] bytes)
        {
            return DecryptStringFromBytesAes(bytes, GetLegalKey(GetKey), Encoding.ASCII.GetBytes(GetIV));
        }

        /// <summary>
        /// Decrypts the hex encoded.
        /// </summary>
        /// <param name="hexEncodedText">The hex encoded text.</param>
        /// <returns>clear text</returns>
        public static string DecryptHexEncoded(string hexEncodedText)
        {
            byte[] hex = HexEncoding.GetBytes(hexEncodedText);
            return RawDecrypt(hex);
        }

        /// <summary>
        /// Add DateTime Xor and Time Salt
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string XorString(string val)
        {
            // Get the current time (which will be used as the salt).
            DateTime currentTime = DateTime.UtcNow;
            byte[] timeSalt = BitConverter.GetBytes(currentTime.Ticks);

            // Convert the input data in the bytes.
            byte[] inputData = Encoding.UTF8.GetBytes(val);

            // Allocate an array for the encrypted data.
            var outputData = new byte[inputData.Length + 8];

            // Copy the time (salt) to the encrypted data output.
            Buffer.BlockCopy(timeSalt, 0, outputData, 0, 8);

            // XOR each byte of the data to encrypt with the time (salt) and put the result
            // in the encrypted data output.
            for (int x = 0; x < inputData.Length; x++)
            {
                outputData[x + 8] = (byte) (inputData[x] ^ timeSalt[x%timeSalt.Length]);
            }

            // Convert the output encrypted data in to a BASE64 string.
            val = Convert.ToBase64String(outputData);

            return val;
        }

        /// <summary>
        /// Remove Datetime Xor and time salt
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string RemoveXorFromString(string val)
        {
            // Get the encrypted data from the BASE64 string.
            byte[] encryptedDataWithSalt = Convert.FromBase64String(val);

            // Extract the salt.
            var timeSalt = new byte[8];
            Buffer.BlockCopy(encryptedDataWithSalt, 0, timeSalt, 0, 8);

            // Allocate an array to hold the unencrypted data (no salt).
            var unencryptedData = new byte[encryptedDataWithSalt.Length - 8];

            // XOR each byte of the data with the time (salt) and put the result
            // in the unencrypted data output.
            for (int x = 0; x < unencryptedData.Length; x++)
            {
                unencryptedData[x] = (byte) (encryptedDataWithSalt[x + 8] ^ timeSalt[x%8]);
            }

            // Convert the unencrypted bytes to a string and return.
            return Encoding.UTF8.GetString(unencryptedData);
        }

        /// <summary>
        /// Sets the static variable value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="staticVariable">The static variable.</param>
        private static void SetStaticVariableValue(string key, ref string staticVariable)
        {
            if (string.IsNullOrEmpty(staticVariable))
            {
                lock (_lock)
                {
                    if (string.IsNullOrEmpty(staticVariable))
                    {
                        if (key == "IV")
                        {
                            staticVariable = _defaultIV;
                        }
                        else if (key == "Key")
                        {
                            staticVariable = _defaultKey;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Encrypt(string data)
        {
            return Encrypt(data, GetKey, GetIV);
        }

        /// <summary>
        /// Encrypts the specified val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string Encrypt(string val, string key)
        {
            return Encrypt(val, key, GetIV);
        }

        /// <summary>
        /// Encrypts the string to bytes_ AES.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The IV.</param>
        /// <returns></returns>
        public static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the streams used
            // to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the bytes used to hold the
            // encrypted data.

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and iv.
                aesAlg = new RijndaelManaged {Key = key, IV = iv};

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);

                //Write all data to the stream.
                swEncrypt.Write(plainText);
            }
            finally
            {
                // Clean things up.

                // Close the streams.
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }


        /// <summary>
        /// Decrypts the safe.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string DecryptSafe(string data)
        {
            if (string.IsNullOrEmpty(data)) return data;
            try
            {
                return Decrypt(data);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The iv.</param>
        /// <returns></returns>
        public static string Decrypt(string data, string key, string iv)
        {
            return DecryptStringFromBytesAes(Convert.FromBase64String(data), GetLegalKey(key),
                                              Encoding.ASCII.GetBytes(iv));
        }

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Decrypt(string data)
        {
            return Decrypt(data, GetKey, GetIV);
        }

        /// <summary>
        /// Decrypts the specified val.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string Decrypt(string val, string key)
        {
            return Decrypt(val, key, GetIV);
        }

        /// <summary>
        /// Decrypts the string from bytes_ AES.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="key">The key.</param>
        /// <param name="iv">The IV.</param>
        /// <returns></returns>
        public static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");

            // TDeclare the streams used
            // to decrypt to an in memory
            // array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and iv.
                aesAlg = new RijndaelManaged {Key = key, IV = iv};

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                msDecrypt = new MemoryStream(cipherText);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }
            finally
            {
                // Clean things up.

                // Close the streams.
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();

                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }


        /// <summary>
        /// Base64 Encode the given string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encode(string data)
        {
            return Encode(Encoding.ASCII.GetBytes(data));
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string Encode(byte[] data)
        {
            /*
            // get the output and trim the '\0' bytes
            int i = 0;
            for (i = 0; i < data.Length; i++) {
                if (data[i] == 0) {
                    break;
                }
            }
             * */
            //return System.Convert.ToBase64String(data, 0, i);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Decode(string data)
        {
            return Convert.FromBase64String(data);
        }

        /// <remarks>
        /// Depending on the legal key size limitations of a specific CryptoService provider
        /// and length of the private key provided, padding the secret key with space character
        /// to meet the legal size of the algorithm.
        /// </remarks>
        private static byte[] GetLegalKey(string key)
        {
            string sTemp;
            SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();

            if (mobjCryptoService.LegalKeySizes.Length > 0)
            {
                int moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;
                // key sizes are in bits
                while (key.Length*8 > moreSize)
                {
                    moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
                }
                sTemp = key.PadRight(moreSize/8, ' ');
            }
            else
                sTemp = key;

            // convert the secret key to byte array
            return Encoding.ASCII.GetBytes(sTemp);
        }
    }
}