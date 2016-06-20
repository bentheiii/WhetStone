using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WhetStone.Timer;

namespace WhetStone.Processes
{
    public static class ProcessMonitor
    {
        public static int countprocesseswithsamename(params string[] otherprocessnames)
        {
            return Process.GetProcesses().Count(clsProcess => clsProcess.ProcessName.Equals(Process.GetCurrentProcess().ProcessName) || otherprocessnames.Contains(clsProcess.ProcessName));
        }
        public static void OpenConsoleWindow(out Process p, out StreamWriter sw, out StreamReader sr)
	    {
		    StreamReader er;
		    OpenConsoleWindow(out p, out sw, out sr, out er);
	    }
	    public static void OpenConsoleWindow(out Process p, out StreamWriter sw, out StreamReader sr, out StreamReader er)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe")
            {
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            p = Process.Start(psi);
            sw = p.StandardInput;
            sr = p.StandardOutput;
	        er = p.StandardError;
        }
        [DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;
        public static void ShowConsole()
        {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }
        public static void HideConsole()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }
    }
    public static class Routine
    {
        public static bool TimeOut(this Action action, TimeSpan maxtime)
        {
            TimeSpan time;
            return TimeOut(action, maxtime, out time);
        }
        public static bool TimeOut(this Action action, TimeSpan maxtime, out TimeSpan time)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            int timeOut = (int)maxtime.TotalMilliseconds;
            var task = Task.Factory.StartNew(action, token);
            IdleTimer t = new IdleTimer();
            bool ret = task.Wait(timeOut, token);
            time = t.timeSinceStart;
            return ret;
        }
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out TimeSpan time)
        {
            T result;
            return TimeOut(action, maxtime, out time, out result);
        }
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out T result)
        {
            TimeSpan time;
            return TimeOut(action, maxtime, out time, out result);
        }
        public static bool TimeOut<T>(this Func<T> action, TimeSpan maxtime, out TimeSpan time, out T result)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            int timeOut = (int)maxtime.TotalMilliseconds;
            var task = Task<T>.Factory.StartNew(action, token);
            IdleTimer t = new IdleTimer();
            bool ret = task.Wait(timeOut, token);
            time = t.timeSinceStart;
            result = task.Result;
            return ret;
        }
    }
    public static class NewProcesses
    {
        public static void openexplorer(string path = "")
        {
            string arg = '"'+path+'"';
            ProcessStartInfo pfi = new ProcessStartInfo("Explorer.exe", arg);
            Process.Start(pfi);
        }
        public static void openexplorerselectfile(string path = "D:")
        {
            string arg = @"/select," + '"' + path +'"';
            ProcessStartInfo pfi = new ProcessStartInfo("Explorer.exe", arg);
            Process.Start(pfi);
        }
        public static void openbrowser(string url = "www.google.com")
        {
            Process.Start(url);
        }
        public static void openfile(string path)
        {
            Process.Start(path);
        }
    }
}
