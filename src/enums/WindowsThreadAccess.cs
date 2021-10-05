﻿/*
 * https://stackoverflow.com/questions/71257/suspend-process-in-c-sharp
 * 
 * Code licensed under CC BY SA 3.0 from StackOverflow.
 * 
 */

using System;

namespace AluminiumTech.DevKit.PlatformKit.enums
{
    [Flags]
    public enum WindowsThreadAccess : int
    {
        TERMINATE = (0x0001),
        SUSPEND_RESUME = (0x0002),
        GET_CONTEXT = (0x0008),
        SET_CONTEXT = (0x0010),
        SET_INFORMATION = (0x0020),
        QUERY_INFORMATION = (0x0040),
        SET_THREAD_TOKEN = (0x0080),
        IMPERSONATE = (0x0100),
        DIRECT_IMPERSONATION = (0x0200)
    }
}