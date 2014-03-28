using System;
using System.Text;

namespace CoreServices
{
    public class CoreKey : ICoreKey
    {
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string PublicKeyHex
        {
            get
            {
                Byte[] stringBytes = Encoding.Default.GetBytes(this.PublicKey);
                StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);

                foreach (byte b in stringBytes)
                {
                    sbBytes.AppendFormat("{0:X2}", b);
                }

                return sbBytes.ToString();
            }
        }

        public CoreKey()
        {
            this.PrivateKey = string.Empty;
            this.PublicKey = string.Empty;
        }

        public CoreKey(string privateKey)
        {
            this.PrivateKey = privateKey;
        }

        /// <summary>
        /// This method generates a new ECDSA public and private key pair.
        /// </summary>
        public void Generate()
        {
            // Generate an ECDSA key pair.
        }
    }
}
