/* Code licensed under CC BY SA 4.0 from StackOverflow.
 *
 * Minimal changes made to work better with PlatformKit
 * 
 * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
 *
 * https://stackoverflow.com/a/71457 - CC BY SA 4.0
 */

using System;
using System.Runtime.InteropServices;

namespace AluminiumTech.DevKit.PlatformKit.Deprecation.Deprecated.Windows
{
    [Obsolete(DeprecationMessages.DeprecationV2_3)]
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
