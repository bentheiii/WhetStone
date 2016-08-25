using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Credentials;

namespace WhetStone.Credentials
{
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
}
