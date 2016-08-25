using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Credentials
{
    [Serializable]
    public class ClosedCredentialValidator : ICredentialValidator
    {
        public bool isValid(Credential c)
        {
            return false;
        }
    }
}
