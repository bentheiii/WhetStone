using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Credentials
{
    [Serializable]
    public class CredentialValidator : ICredentialValidator
    {
        public Credential valid { get; }
        public CredentialValidator() : this(new Credential()) { }
        public CredentialValidator(Credential valid)
        {
            if (valid == null)
                throw new ArgumentException("cannot be null", nameof(valid));
            this.valid = valid;
        }
        public CredentialValidator(out Credential valid) : this(valid = new Credential()) { }
        public bool isValid(Credential c)
        {
            return ReferenceEquals(this.valid, c);
        }
    }
}
