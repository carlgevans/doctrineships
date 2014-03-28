using System;

namespace CoreServices
{
    public interface ICoreKey
    {
        string PrivateKey { get; set; }
        string PublicKey { get; set; }
        string PublicKeyHex { get; set; }

        /// <summary>
        /// This method generates a new ECDSA public and private key pair.
        /// </summary>
        void Generate();
    }
}
