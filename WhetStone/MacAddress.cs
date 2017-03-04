using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using WhetStone.Looping;

namespace WhetStone.Enviroment
{
    /// <summary>
    /// A static container for identity method
    /// </summary>
    public static class macAddress
    {
        /// <summary>
        /// Get the first viable MAC address for the current machine.
        /// </summary>
        /// <returns>The MAC address of the machine, or <see langword="null"/> of none found.</returns>
        public static IList<byte> MacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string sMacAddress = string.Empty;
            return nics.Select(a => a.GetPhysicalAddress().GetAddressBytes()).FirstOrDefault(a => a.Length > 0, null);
        }
    }
}
