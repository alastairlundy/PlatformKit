/*
 * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
 * 
 * Code licensed under CC BY SA 3.0 from StackOverflow.
 * 
 */

using AluminiumTech.DevKit.PlatformKit.enums;

using System;
using System.Diagnostics;

using HardwareKit.Components.Base.enums;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics{
    public static class WindowsProcessSpecifics{
        /// <summary>
        /// Suspend running processes on Windows.
        /// DOES NOT WORK on macOS or Linux. Running this on macOS or Linux will trigger a PlatformNotSupportedException.
        /// </summary>
        /// <param name="process"></param>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public static void Suspend(Process process)
        {
            try
            {
                Platform platform = new Platform();
                
                if (platform.IsWindows())
                {
                    foreach (ProcessThread thread in process.Threads)
                    {

                        var pOpenThread = WindowsProcessImports.OpenThread(WindowsThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
                        if (pOpenThread == IntPtr.Zero)
                        {
                            continue;
                        }
                        WindowsProcessImports.SuspendThread(pOpenThread);
                        WindowsProcessImports.CloseHandle(pOpenThread);
                    }
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
              
        }
        /// <summary>
        /// Resume a suspended Process on Windows. 
        /// DOES NOT WORK on macOS or Linux. Running this on macOS or Linux will trigger a PlatformNotSupportedException
        /// </summary>
        /// <param name="process"></param>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public static void Resume(Process process)
        {
            try
            {
                Platform platform = new Platform();

                if (platform.IsWindows())
                {
                    foreach (ProcessThread thread in process.Threads)
                    {
                        var pOpenThread = WindowsProcessImports.OpenThread(WindowsThreadAccess.SUSPEND_RESUME, false,
                            (uint)thread.Id);
                        if (pOpenThread == IntPtr.Zero)
                        {
                            continue;
                        }

                        WindowsProcessImports.ResumeThread(pOpenThread);
                        WindowsProcessImports.CloseHandle(pOpenThread);
                    }
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
    }
}
