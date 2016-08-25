using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WhetStone.Data
{
    public static class toXmlDoc
    {
        public static XmlDocument ToXmlDoc(string innertext)
        {
            XmlDocument ret = new XmlDocument();
            ret.LoadXml(innertext);
            return ret;
        }
    }
}
