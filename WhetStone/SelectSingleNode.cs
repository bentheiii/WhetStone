using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WhetStone.Looping;

namespace WhetStone.Data
{
    public static class selectSingleNode
    {
        public static XmlNode SelectSingleNode(this XmlNode @this, string query, params string[] namespaces)
        {
            XmlNamespaceManager xnm = new XmlNamespaceManager(@this.OwnerDocument.NameTable);
            foreach (var ns in namespaces.Chunk(2))
            {
                xnm.AddNamespace(ns[0], ns[1]);
            }
            return @this.SelectSingleNode(query, xnm);
        }
    }
}
