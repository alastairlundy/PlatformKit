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
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AluminiumCoreLib.IO{
    /// <summary>
    /// A class to help manage saving files.
    /// </summary>
    public class FileSaver {

        /// <summary>
        /// Save an array of strings to a text file.
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="directory"></param>
        /// <param name="FileName"></param>
        public void SaveTextFile(string[] strings, string directory, string FileName){
            try{
                StreamWriter sw = File.CreateText(directory + Path.DirectorySeparatorChar + FileName);

                foreach (string str in strings){
                    sw.WriteLine(str);
                }

                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Save elements in an ObjectList of type string to a text file.
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="directory"></param>
        /// <param name="FileName"></param>
        public void SaveTextFile(List<string> strings, string directory, string FileName){
            try{
                StreamWriter sw = File.CreateText(directory + Path.DirectorySeparatorChar + FileName);

                for (int i = 0; i < strings.Capacity; i++ ){
                    sw.WriteLine(strings[i]);
                }

                sw.Close();
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        ///  Save an array of objects to a text file.
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="directory"></param>
        /// <param name="FileName"></param>
        public void SaveTextFile(object[] objects, string directory, string FileName){
            try{
                StreamWriter sw = File.CreateText(directory + Path.DirectorySeparatorChar + FileName);

                foreach (Object obj in objects)
                {
                    sw.WriteLine(obj.ToString());
                }

                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Saves an object to a JSON file to the specified path.
        /// </summary>
        /// <param name="log"></param>
        public void SaveJSONFile(object obj, string path, string FileName){
            JObject json = new JObject(obj);
            FileName = Path.Combine(path, FileName);
            File.WriteAllText(FileName, json.ToString());
        }
        /// <summary>
        /// Saves an object to a XML file to the specified path.
        /// </summary>
        public void SaveXMLFile(object obj, string path, string FileName){
            JObject json = new JObject(obj);
            XmlDocument doc = JsonConvert.DeserializeXmlNode(json.ToString());
            FileName = Path.Combine(path, FileName);
            //See https://msdn.microsoft.com/en-us/library/dw229a22(v=vs.110).aspx
            doc.Save(FileName);
        }
    }
}