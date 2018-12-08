/*
MIT License

Copyright (c) 2018 AluminiumTech

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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace AluminiumCoreLib.IO{
    /// <summary>
    /// 
    /// </summary>

    public static class ZipExtractor{

        /// <summary>
        /// 
        /// </summary>
        public static void ExtractZip(string FileName, string path, ProcessStartInfo processStartInfo){
           
              foreach (var process in Process.GetProcesses()){
                    try{
                        if (process.MainModule.FileName.Equals(FileName)){
                            Console.WriteLine("Waiting for Application to Exit...");
                            process.WaitForExit();
                        }
                    }
                    catch (Exception exception){
                        Debug.WriteLine(exception.Message);
                    }
                }
           
            var _path = Path.GetDirectoryName(path);
            // Open an existing zip file for reading.
            ZipStorer zip = ZipStorer.Open(FileName, FileAccess.Read);
            // Read the central directory collection.
            List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();

            for (var index = 0; index < dir.Count; index++){
                ZipStorer.ZipFileEntry entry = dir[index];
                zip.ExtractFile(entry, Path.Combine(path, entry.FilenameInZip));
            }

             zip.Close();

            try{     
                Process.Start(processStartInfo);
            }
            catch (Exception exception){
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
    }
}