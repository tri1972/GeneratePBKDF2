using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace GeneratePDFK2
{
    /// <summary>
    /// 参考url　https://qiita.com/inasync/items/b822c9826757f84e031a
    /// PBKDF2-SHA1を使用
    /// </summary>
    public static class PBKDF2
    {
        private const int DerivedKeyLength = 512 / 8;

        public static PBKDF2Hash Hash(string password, int saltSize = 16, int iterations = 10000,string salt=null)
        {
            //passwordのBase64によるByteArray変換
            var enc = Encoding.GetEncoding("UTF-8");
            var base64Password = Convert.ToBase64String(enc.GetBytes(password));
            byte[] dataPassword = System.Text.Encoding.UTF8.GetBytes(base64Password);

            if (salt == null)
            {
                //saltのBase64によるByteArray変換
                var base64Salt = Convert.ToBase64String(enc.GetBytes(salt));
                byte[] dataSalt = System.Text.Encoding.UTF8.GetBytes(base64Salt);

                using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize: saltSize, iterations: iterations))
                {
                    var dk = deriveBytes.GetBytes(DerivedKeyLength);
                    return new PBKDF2Hash(deriveBytes.IterationCount, deriveBytes.Salt, dk);
                }
            }
            else
            {
                //saltのBase64によるByteArray変換
                var base64Salt = Convert.ToBase64String(enc.GetBytes(salt));
                byte[] dataSalt = System.Text.Encoding.UTF8.GetBytes(base64Salt);

                using (var deriveBytes = new Rfc2898DeriveBytes(dataPassword, dataSalt, iterations,HashAlgorithmName.SHA1))
                {
                    var dk = deriveBytes.GetBytes(DerivedKeyLength);
                    return new PBKDF2Hash(deriveBytes.IterationCount, deriveBytes.Salt,dk);
                }

            }
        }

        public static bool Verify(string password, string hashStr)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (hashStr == null) throw new ArgumentNullException(nameof(hashStr));
            if (PBKDF2Hash.TryParse(hashStr, out var hash) == false) throw new FormatException(nameof(hashStr));

            return Verify(password, hash);
        }

        public static bool Verify(string password, PBKDF2Hash hash)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (hash == null) throw new ArgumentNullException(nameof(hash));

            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt: hash.Salt, iterations: hash.IterationCount))
            {
                var dk = deriveBytes.GetBytes(DerivedKeyLength);
                return hash.DerivedKey.SequenceEqual(dk);
            }
        }
    }

    public class PBKDF2Hash
    {
        private const string HashId = "pbkdf2";

        public PBKDF2Hash(int iterationCount, byte[] salt, byte[] derivedKey)
        {
            IterationCount = iterationCount;
            Salt = salt;
            DerivedKey = derivedKey;
        }

        public int IterationCount { get; }
        public byte[] Salt { get; }
        public byte[] DerivedKey { get; }

        public override string ToString()
        {
            return Convert.ToBase64String(DerivedKey);
            //return $"${HashId}${IterationCount}${AdaptedBase64Encode(Salt)}${AdaptedBase64Encode(DerivedKey)}";

        }

        public static bool TryParse(string hashStr, out PBKDF2Hash result)
        {
            result = null;
            if (hashStr == null) return false;
            if (hashStr.StartsWith("$") == false) return false;

            var elems = hashStr.Split(new[] { '$' }, StringSplitOptions.RemoveEmptyEntries);
            if (elems.Length != 4) return false;

            if (elems[0] != HashId) return false;
            if (int.TryParse(elems[1], out var iterationCount) == false) return false;
            var salt = AdaptedBase64Decode(elems[2]);
            var dk = AdaptedBase64Decode(elems[3]);

            result = new PBKDF2Hash(iterationCount, salt, dk);
            return true;
        }

        #region Helper

        /// <summary>
        /// adapted base64 encoding
        /// http://nullege.com/codes/search/passlib.utils.ab64_encode
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        private static string AdaptedBase64Encode(byte[] bin)
        {
            return Convert.ToBase64String(bin).TrimEnd('=').Replace('+', '.');
        }

        /// <summary>
        /// http://nullege.com/codes/search/passlib.utils.ab64_decode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte[] AdaptedBase64Decode(string value)
        {
            var paddingLen = (4 - value.Length % 4) & 0x3;
            var bldr = new StringBuilder(value);
            bldr.Replace('.', '+');
            bldr.Append('=', paddingLen);
            return Convert.FromBase64String(bldr.ToString());
        }

        #endregion Helper
    }


}
