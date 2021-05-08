using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Models;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Services
{
    public class CookieService
    {
        public static List<CookieItem> ReadChromeCookie(string hostName)
        {
            return ReadCookies(hostName, @"Google\chrome");
        }

        public static List<CookieItem> Read115Cookie(string hostName)
        {
            return ReadCookies(hostName, @"115Chrome");
        }

        private static List<CookieItem> ReadCookies(string hostName, string program)
        {
            if (hostName == null) throw new ArgumentNullException("hostName");

            List<CookieItem> ret = new List<CookieItem>();

            var oriCookieFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @$"\{program}\User Data\Default\Cookies";
            var copyCookieFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @$"\{program}\User Data\Default\CookiesCopy";

            if (File.Exists(oriCookieFile))
            {
                File.Copy(oriCookieFile, copyCookieFile, true);
            }
            else
            {
                throw new FileNotFoundException("Cant find cookie store", oriCookieFile);
            }

            var dbPath = copyCookieFile;
            if (!File.Exists(dbPath)) throw new FileNotFoundException("Cant find cookie store", dbPath);

            var connectionString = "Data Source=" + dbPath + ";pooling=false";

            using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                var prm = cmd.CreateParameter();
                prm.ParameterName = "hostName";
                prm.Value = hostName;
                cmd.Parameters.Add(prm);

                cmd.CommandText = "SELECT name,encrypted_value FROM cookies WHERE host_key = '" + hostName + "'";

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader[0].ToString();
                        var encryptedData = (byte[])reader[1];

                        string encKey = File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + @$"\{program}\User Data\Local State");
                        encKey = JObject.Parse(encKey)["os_crypt"]["encrypted_key"].ToString();
                        var decodedKey = System.Security.Cryptography.ProtectedData.Unprotect(Convert.FromBase64String(encKey).Skip(5).ToArray(), null, System.Security.Cryptography.DataProtectionScope.LocalMachine);
                        var _cookie = _decryptWithKey(encryptedData, decodedKey, 3);

                        if (string.IsNullOrEmpty(_cookie))
                        { 
                            _cookie = Encoding.ASCII.GetString(System.Security.Cryptography.ProtectedData.Unprotect(encryptedData, null, System.Security.Cryptography.DataProtectionScope.CurrentUser));
                        }

                        if (ret.Find(x => x.Name == name) == null)
                        {
                            ret.Add(new CookieItem
                            {
                                Name = name,
                                Value = _cookie
                            });
                        }
                    }
                }
                conn.Close();
            }
            return ret;
        }

        private static string _decryptWithKey(byte[] message, byte[] key, int nonSecretPayloadLength)
        {
            const int KEY_BIT_SIZE = 256;
            const int MAC_BIT_SIZE = 128;
            const int NONCE_BIT_SIZE = 96;

            if (key == null || key.Length != KEY_BIT_SIZE / 8)
                throw new ArgumentException(String.Format("Key needs to be {0} bit!", KEY_BIT_SIZE), "key");
            if (message == null || message.Length == 0)
                throw new ArgumentException("Message required!", "message");

            using (var cipherStream = new MemoryStream(message))
            using (var cipherReader = new BinaryReader(cipherStream))
            {
                var nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);
                var nonce = cipherReader.ReadBytes(NONCE_BIT_SIZE / 8);
                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(key), MAC_BIT_SIZE, nonce);
                cipher.Init(false, parameters);
                var cipherText = cipherReader.ReadBytes(message.Length);
                var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];
                try
                {
                    var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                    cipher.DoFinal(plainText, len);
                }
                catch (InvalidCipherTextException)
                {
                    return null;
                }
                return Encoding.Default.GetString(plainText);
            }
        }
    }
}
