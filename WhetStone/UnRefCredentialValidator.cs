using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Credentials
{
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
}
