using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhetStone.Credentials
{
    public static class throwIfInvalid
    {
        public static void ThrowIfInvalid(this ICredentialValidator v, Credential c)
        {
            if (!v.isValid(c))
                throw new UnauthorizedAccessException("the credentials are not valid");
        }
    }
}
