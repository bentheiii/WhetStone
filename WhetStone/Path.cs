using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using WhetStone.Arrays;

namespace WhetStone.Path
{
    public static class LoadFiles
    {
        public static byte[] loadAsBytes(FileStream stream, int bufferSize = 4096)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var b = new ResizingArray<byte>();
            byte[] buffer = new byte[bufferSize];
            while (true)
            {
                var grabbed = stream.Read(buffer, 0, bufferSize);
                if (grabbed == 0)
                    break;
                b.AddRange(buffer.Take(grabbed));
            }
            return b.ToArray();
        }
    }
    public static class FilePath
    {
        public static string currentexeroot
        {
            get
            {
                return Assembly.GetCallingAssembly().GetName().CodeBase;
            }
        }
        public static bool IsFileAccessible(string filename,FileAccess access = FileAccess.ReadWrite)
        {
            try
            {
                FileStream fs = File.Open(filename, FileMode.Open, access, FileShare.None);
                fs.Close();
                return true;
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (IOException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
                return false;
            }
        }
        public static string MutateFileName(string filepath, Func<string, string> mutation)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filepath), mutation(System.IO.Path.GetFileName(filepath)));
        }
    }
}
