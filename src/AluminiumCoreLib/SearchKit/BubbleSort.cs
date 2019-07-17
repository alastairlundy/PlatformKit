/*
MIT License

Copyright (c) 2018-2019 AluminiumTech

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
using System.Collections;
using System.Collections.Generic;

namespace AluminiumCoreLib.SearchKit
{
    /// <summary>
    /// 
    /// </summary>
    public class BubbleSort<T>{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> Sort(List<T> list){
            int length = list.Count;
            T temp = list[0];

            for (int i = 0; i < length; i++){
                for (int j = i + 1; j < length; j++){
                    if (Comparer.Default.Compare(list[i], list[j]) < 0){
                        temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] Sort(T[] array){
            /// <summary>
            /// Adapted from https://stackoverflow.com/questions/36764347/how-to-bubble-sort-a-string-array
            /// </summary>
            int length = array.Length;
            T temp = array[0];

            for (int i = 0; i < length; i++){
                for (int j = i + 1; j < length; j++){
                    if (Comparer.Default.Compare(array[i], array[j]) < 0){
                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
            return array;
        }
    }
}