/*
 * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
 * 
 * Code licensed under CC BY SA 3.0 from StackOverflow.
 * 
 */
using AluminiumTech.DevKit.PlatformKit.enums;

using System;
using System.Runtime.InteropServices;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics
{
    internal static class WindowsProcessImports
    {

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(WindowsThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr handle);

    }
}
