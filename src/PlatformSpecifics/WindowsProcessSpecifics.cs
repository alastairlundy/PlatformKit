/*
 * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
 * 
 * Code licensed under CC BY SA 3.0 from StackOverflow.
 * 
 */

using AluminiumTech.DevKit.PlatformKit.enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics{
    public class WindowsProcessSpecifics{
        public static void Suspend(Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = WindowsProcessImports.OpenThread(WindowsThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                WindowsProcessImports.SuspendThread(pOpenThread);
            }
        }
        public static void Resume(Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                var pOpenThread = WindowsProcessImports.OpenThread(WindowsThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }
                WindowsProcessImports.ResumeThread(pOpenThread);
            }
        }
    }
}
