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

namespace AluminiumCoreLib.PasswordKit
{
    /// <summary>
    /// A class to help generate secure passwords.
    /// </summary>
    public class PasswordGenerator{
        private string[] alphabetLower = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private string[] alphabetUpper = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private string[] specialCharacters = { "!", "#", "~", "_", "-", "+", "=", "(", ")", "*", "&", "%", "$", ";", ":" };

        public string GeneratePassword(int PasswordLength, bool IncludeUpperCase, bool IncludeNumbers, bool IncludeSpecialCharacters){
            string password = "";

            for (int i = 0; i < PasswordLength; i++){
                password += GenerateRandomCharacterString(IncludeUpperCase, IncludeNumbers, IncludeSpecialCharacters);
            }

            return password;
        }
        public string GenerateRandomCharacterString(bool IncludeUpperCase, bool IncludeNumbers, bool IncludeSpecialCharacters){
            string password = "";
            SecureRNG secure = new SecureRNG();

            int random = 0;
            random = secure.NextRandom(0, 3);

            if (random.Equals(0)){
                random = secure.NextRandom(0, alphabetLower.Length - 1);
                password += alphabetLower[random];
            }
            else if (random.Equals(1) && IncludeUpperCase){
                random = secure.NextRandom(0, alphabetUpper.Length - 1);
                password += alphabetUpper[random];
            }
            else if (random.Equals(2) && IncludeNumbers){
                random = secure.NextRandom(0, numbers.Length - 1);
                password += numbers[random];
            }
            else if (random.Equals(3) && IncludeSpecialCharacters){
                random = secure.NextRandom(0, specialCharacters.Length - 1);
                password += specialCharacters[random];
            }
            return password;
        }
    }
}