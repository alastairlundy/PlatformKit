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
using System;
using System.Security.Cryptography;

namespace AluminiumCoreLib.PasswordKit
{
    /// <summary>
    /// A wrapper class for the RNGCryptoServiceProvider class
    /// </summary>
   public class SecureRNG{
          private RNGCryptoServiceProvider x;

        public SecureRNG(){
            x = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Generates a new Random Byte between 0 and 255
        /// </summary>
        /// <returns></returns>
        public byte NextRandomByte(){
            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];
            // Fill the array with a random value.
            x.GetBytes(randomNumber);
            return randomNumber[0];
        }
        /// <summary>
        /// Generates a new Random Byte between 0 and 255
        /// </summary>
        /// <returns></returns>
        public byte NextRandomByte(int minimum){
            while (true){
                var b = NextRandomByte();

                 if(b >= minimum){
                    return b;
                 }
            }
        }
        /// <summary>
        /// Generatess a new Random Byte between 0 and 255
        /// </summary>
        /// <returns></returns>
        public byte NextRandomByte(byte minimum, byte maximum){
            while (true){
               var b = NextRandomByte();
                
                if (b >= minimum && b <= maximum && maximum <= (byte)255)
                {
                    return b;
                }
            }
        }

        /// <summary>
        /// Generates a new Random 32 Bit Integer between 0 and 255
        /// </summary>
        /// <returns></returns>
        public int NextRandom(){
            return Convert.ToInt32(NextRandomDouble() * 255);
        }
        /// <summary>
        ///  Generates a new Random 32 Bit Integer between 0 and 255
        /// </summary>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public int NextRandom(int minimum){
            while (true){
                var i = NextRandom();

                if (i >= minimum){
                    return i;
                }
            }
        }
        /// <summary>
        /// Generates a new Random 32 Bit Integer
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public int NextRandom(int minimum, int maximum){
            while (true){
                var l = Convert.ToInt32(NextRandomDouble(minimum / maximum) * maximum);

                if (l >= minimum && l <= maximum){
                    return l;
                }
            }
        }

        /// <summary>
        /// Generates a new Random 64 Bit Integer between 0 and 255
        /// </summary>
        /// <returns></returns>
        public long NextRandomLong(){
            return Convert.ToInt64(NextRandomDouble() * 255);
        }
        /// <summary>
        /// Generates a new Random 64 Bit Integer between 0 and 255
        /// </summary>
        /// <returns></returns>
        public long NextRandomLong(long minimum){
            while (true){
                var l = NextRandomLong();

                if (l >= minimum){
                    return l;
                }
            }
        }
        /// <summary>
        /// Generates a new Random 64 Bit Integer
        /// </summary>
        /// <returns></returns>
        public long NextRandomLong(long minimum, long maximum){
            while (true){
                var l = NextRandomLong(minimum);

                if (l >= minimum && l <= maximum){
                    return l;
                }
            }
        }

        /// <summary>
        /// Generates a new Random 64 Bit Double precision floating point number between 0 and 1.
        /// </summary>
        /// <returns></returns>
        public double NextRandomDouble(){
            return Convert.ToDouble(NextRandomByte() / 255.0);
        }
        /// <summary>
        /// Generates a new Random 64 Bit Double precision floating point number between 0 and 1.
        /// </summary>
        /// <returns></returns>
        public double NextRandomDouble(double minimum){
            while (true){
                var d = NextRandomDouble();

                if (d >= minimum){
                    return d;
                }
            }
        }
        /// <summary>
        /// Generates a new Random 64 Bit Double precision floating point number
        /// </summary>
        /// <returns></returns>
        public double NextRandomDouble(double minimum, double maximum){
            while (true){
                var d = NextRandomDouble(minimum) * maximum;

                if (d >= minimum && d <= maximum){
                    return d;
                }
            }
        }
    }
}