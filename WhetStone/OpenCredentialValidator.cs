using System;

namespace WhetStone.Credentials
{
    [Serializable]
    public class OpenCredentialValidator : ICredentialValidator
    {
        public bool isValid(Credential c)
        {
            return true;
        }
    }
}
