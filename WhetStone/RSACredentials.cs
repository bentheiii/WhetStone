using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Credentials;

namespace WhetStone.Credentials
{
    [Serializable]
    // ReSharper disable once InconsistentNaming
    public class RSACredential : UnRefCredential<byte[]>
    {
        private RSACredential(byte[] sign) : base(sign) { }
        public static RSACredential Create(byte[] message, RSAParameters privateKey)
        {
            // The array to store the signed message in bytes
            using (var rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    // Import the private key used for signing the message
                    rsa.ImportParameters(privateKey);

                    // Sign the data, using SHA512 as the hashing algorithm 
                    var signedBytes = rsa.SignData(message, CryptoConfig.MapNameToOID("SHA512"));
                    return new RSACredential(signedBytes);
                }
                finally
                {
                    // Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        public static void GetKey(out RSAParameters @private, out RSAParameters @public, int size = 1024)
        {
            using (RSACryptoServiceProvider prov = new RSACryptoServiceProvider(size))
            {
                try
                {
                    @public = prov.ExportParameters(false);
                    @private = prov.ExportParameters(true);
                }
                finally
                {
                    prov.PersistKeyInCsp = false;
                }
            }
        }
    }
}
