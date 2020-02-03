/*
MIT License

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
using System;
using System.Collections.Generic;

namespace AluminiumCoreLib{
    /// <summary>
    /// A class to store hashed keys and values similar to how Java's HashMap works.
    /// NOTE: This class is deprecated and may be removed in the future - Please use C#'s built in Dictionary class instead
    /// </summary>
    public class HashMap<TKey, TValue>{
        private Dictionary<TKey, TValue> dictionary;

        public HashMap(){
            dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Adds a Key with a specified value.
        /// </summary>
        /// <param name="key">The key to be added</param>
        /// <param name="value">The value to be associated with the key</param>
        /// <returns></returns>
        public void Put(TKey key, TValue value){
            dictionary.Add(key, value);
        }

        /// <summary>
        /// Adds a Key with a specified value if it is not in the HashMap. Returns true if it was Absent and false if present.
        /// </summary>
        /// <param name="key">The key to be added if it is absent</param>
        /// <param name="value">The value to be associated with the key</param>
        /// <returns></returns>
        public bool PutIfAbsent(TKey key, TValue value){
                if(ContainsKey(key) && GetValue(key).Equals(value)){
                    return false;
                }
                else{
                    Put(key, value);
                    return true;
                }
        }

        /// <summary>
        /// Remove a key from HashMap.
        /// </summary>
        /// <param name="key">The key to be removed</param>
        /// <returns></returns>
        public bool Remove(TKey key){
            return dictionary.Remove(key);
        }

        /// <summary>
        /// Replaces the value of a key only if thet key is associated with a specified value.
        /// </summary>
        /// <param name="key">The key to replace the value of.</param>
        /// <param name="OldValue">The old value already associated with the key</param>
        /// <param name="NewValue">The new value to be associated with the key</param>
        /// <returns></returns>
        public bool Replace(TKey key, TValue OldValue, TValue NewValue){
                if (ContainsKey(key) && GetValue(key).Equals(OldValue)){
                    dictionary.Remove(key);
                    dictionary.Add(key, NewValue);
                    return true;
                }
            else{
                return false;
            }
        }
        /// <summary>
        /// Replaces the value of a key.
        /// </summary>
        /// <param name="key">The key to replace the value of.</param>
        /// <param name="value">The new value to be associated with the key</param>
        /// <returns></returns>
        public bool Replace(TKey key, TValue value){
            try{
                if (ContainsKey(key))
                {
                    dictionary.Remove(key);
                    dictionary.Add(key, value);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex){
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Returns the value associated with the specified key.
        /// </summary>
        /// <returns></returns>
        public TValue GetValue(TKey key){
            return dictionary[key];
        }

        /// <summary>
        /// Returns the value associated with the specified key or returns a default.
        /// </summary>
        /// <param name="DefaultValue">The default value to be returned in case the key is not associated with a value</param>
        /// <returns></returns>
        public TValue GetValueOrDefault(TKey key, TValue DefaultValue){
            try{
                return GetValue(key);
            }
            catch{
                return DefaultValue;
            }
        }
        /// <summary>
        /// Returns the value associated with the specified key or returns a default and Adds the key with the default value.
        /// </summary>
        /// <param name="DefaultValue">The default value to be returned in case the key is not associated with a value</param>
        /// <returns></returns>
        public TValue GetValueOrDefaultAndPutIfAbsent(TKey key, TValue DefaultValue){
            try{
                return GetValue(key);
            }
            catch{
                PutIfAbsent(key, DefaultValue);
                return DefaultValue;
            }
        }

        /// <summary>
        /// Return the number of key and value pairs in the hashmap.
        /// </summary>
        /// <returns></returns>
        public int Size(){
            return dictionary.Count;
        }

        /// <summary>
        ///  Checks to see if the HashMap contains a specific key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key){
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Checks to see if the HashMap contains a specific value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(TValue value){
            return dictionary.ContainsValue(value);
        }

        /// <summary>
        /// Tries to add a key wiht a specified value. Returns true if successful and false if it failed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryPut(TKey key, TValue value){
            try{
               Put(key, value);
               return true;
            }
            catch{
                return false;
            }
        }

        /// <summary>
        /// Removes all the keys and values in the HashMap.
        /// </summary>
        public void Clear(){
            dictionary.Clear();
        }

        /// <summary>
        /// Returns if the HashMap is empty.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty(){
           if(dictionary.Count > 0){
                return false;
            }
            else{
                return true;
            }
        }

        /// <summary>
        /// Returns the HashMap as a C# Dictionary.
        /// </summary>
        /// <returns></returns>
        public Dictionary<TKey, TValue> ToDictionary(){
          return dictionary;
        }

        /// <summary>
        /// Returns the HashMap as an List of type TValue.
        /// Note: This loses all of a HashMap's Key Values.
        /// </summary>
        /// <returns></returns>
        public List<TValue> ToList(){
          var objList = new List<TValue>();

            foreach(KeyValuePair<TKey, TValue> t in dictionary){
                TValue tv = GetValue(t.Key);
                objList.Add(tv);
            }
         return objList;  
        }

        /// <summary>
        /// Returns the HashMap as an Array of type TValue.
         /// Note: This loses all of a HashMap's Key Values.
        /// </summary>
        /// <returns></returns>
        public TValue[] ToArray(){
          List<TValue> objList = ToList();
           return objList.ToArray();
        }
    }
}