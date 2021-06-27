/*
 * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
 * 
 * Code licensed under CC BY SA 3.0 from StackOverflow.
 * 
 */
using AluminiumTech.DevKit.PlatformKit.enums;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics
{
    internal class WindowsProcessImports
    {
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(WindowsThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern uint ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);


    }
}
