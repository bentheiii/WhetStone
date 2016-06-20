using System;
using System.Security.Cryptography;

namespace WhetStone.Credentials
{
    [Serializable]
    public class Credential {}
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
    [Serializable]
    public class UnRefCredential<T> : Credential
    {
        public UnRefCredential(T value)
        {
            this.value = value;
        }
        public T value { get; }
        public override string ToString()
        {
            return value.ToString();
        }
    }
    public interface ICredentialValidator
    {
        bool isValid(Credential c);
    }
    [Serializable]
    public class CredentialValidator : ICredentialValidator
    {
        public Credential valid { get; }
        public CredentialValidator() : this(new Credential()) { }
        public CredentialValidator(Credential valid)
        {
            if (valid == null)
                throw new ArgumentException("cannot be null",nameof(valid));
            this.valid = valid;
        }
        public CredentialValidator(out Credential valid) : this(valid = new Credential()) { }
        public bool isValid(Credential c)
        {
            return ReferenceEquals(this.valid, c);
        }
    }
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
    [Serializable]
    public class OpenCredentialValidator : ICredentialValidator
    {
        public bool isValid(Credential c)
        {
            return true;
        }
    }
    [Serializable]
    public class ClosedCredentialValidator : ICredentialValidator
    {
        public bool isValid(Credential c)
        {
            return false;
        }
    }
    [Serializable]
    public class UnRefCredentialsValidator<T> : ICredentialValidator
    {
        private readonly Func<T, bool> _validator;
        public UnRefCredentialsValidator(Func<T, bool> validator)
        {
            this._validator = validator;
        }
        public bool isValid(Credential c)
        {
            var credentials = c as UnRefCredential<T>;
            return credentials != null && this._validator(credentials.value);
        }
    }
    public static class ValidationExtentions
    {
        public static void ThrowIfInvalid(this ICredentialValidator v, Credential c)
        {
            if(!v.isValid(c))
                throw new UnauthorizedAccessException("the credentials are not valid");
        }
    }
}