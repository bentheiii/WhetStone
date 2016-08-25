using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using WhetStone.Looping;

namespace WhetStone.Enviroment
{
    public static class macAddress
    {
        public static IEnumerable<byte> MacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == string.Empty)// only return MAC Address from first card  
                {
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return range.Range(0, sMacAddress.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(sMacAddress.Substring(x, 2), 16));
        }
    }
}
