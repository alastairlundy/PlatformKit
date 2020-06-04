﻿/* MIT License

Copyright (c) 2018-2020 AluminiumTech

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
    */

namespace AluminiumTech.PlatformKit.enums{
    public enum CPUArchitectureFamily{
        // ReSharper disable once InconsistentNaming
        IA64,
        X64,
        X86,
        // ReSharper disable once InconsistentNaming
        ARM32,
        // ReSharper disable once InconsistentNaming
        ARM64,
        // ReSharper disable once InconsistentNaming
        MIPS,
        // ReSharper disable once InconsistentNaming
        PowerPC32,
        // ReSharper disable once InconsistentNaming
        PowerPC64,
        NotDetected
    }
}