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
    public class RSAValidator : ICredentialValidator
    {
        private readonly byte[] _message;
        private readonly RSAParameters _parameters;
        public RSAValidator(byte[] message, RSAParameters parameters)
        {
            _message = message;
            _parameters = parameters;
        }
        public bool isValid(Credential c)
        {
            var r = c as RSACredential;
            if (c == null)
                return false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                byte[] bytesToVerify = _message;
                byte[] signedBytes = r.value;
                try
                {
                    rsa.ImportParameters(_parameters);
                    SHA512Managed hash = new SHA512Managed();
                    byte[] hashedData = hash.ComputeHash(signedBytes);
                    return rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                }
                catch (CryptographicException)
                {
                    return false;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
