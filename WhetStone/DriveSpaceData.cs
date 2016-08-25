using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WhetStone.Units.DataSizes;

namespace WhetStone.Enviroment
{
    class DriveSpaceData
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)] [return: MarshalAs(UnmanagedType.Bool)] private static extern bool
            GetDiskFreeSpaceEx(string lpDirectoryName, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes,
                out ulong lpTotalNumberOfFreeBytes);

        public DataSize freespace { get; }
        public DataSize totalspace { get; }
        public DataSize totalfreespace { get; }
        private DriveSpaceData(DataSize freespace, DataSize totalspace, DataSize totalfreespace)
        {
            this.freespace = freespace;
            this.totalspace = totalspace;
            this.totalfreespace = totalfreespace;
        }
        public static DriveSpaceData DriveSpace(char driveletter)
        {
            ulong f, t, ft;
            if (!GetDiskFreeSpaceEx(driveletter + ":", out f, out t, out ft))
                return null;
            return new DriveSpaceData(new DataSize(f, DataSize.Byte), new DataSize(t, DataSize.Byte), new DataSize(ft, DataSize.Byte));
        }
    }
}
