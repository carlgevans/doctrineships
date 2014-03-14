namespace Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Assorted security and encryption tools.
    /// </summary>
    public static class Security
    {
        /// <summary>
        /// Generate a hash from a password/key and a salt.
        /// </summary>
        /// <param name="key">The password/key to be hashed.</param>
        /// <param name="salt">The random salt to hash with.</param>
        /// <returns>Returns a hashed password/key.</returns>
        public static string GenerateHash(string key, string salt)
        {
            string saltedKey = String.Concat(key, salt);
            string hashedKey = GenerateSHA1(saltedKey);

            return hashedKey;
        }

        /// <summary>
        /// Generate a random salt.
        /// </summary>
        /// <param name="size">Size of the salt in bytes.</param>
        /// <returns>Returns a Base64 random number.</returns>
        public static string GenerateSalt(int size)
        {
            // Generate a cryptographic random number.
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[size];
            crypto.GetBytes(buffer);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Generate a random string from a given length and character set.
        /// </summary>
        /// <param name="length">Length of the generated string.</param>
        /// <param name="allowedChars">A list of characters that should be used in the generated string.</param>
        /// <returns>Returns a random string of a given length.</returns>
        public static string GenerateRandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();

            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];

                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }

                return result.ToString();
            }
        }

        internal static string GenerateSHA1(string saltedPassword)
        {
            SHA1 algorithm = SHA1.Create();
            byte[] data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            string hashedPassword = String.Empty;

            for (int i = 0; i < data.Length; i++)
            {
                hashedPassword += data[i].ToString("x2").ToUpperInvariant();
            }

            return hashedPassword;
        }
    }
}
