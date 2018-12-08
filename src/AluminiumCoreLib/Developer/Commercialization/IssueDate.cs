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

namespace AluminiumCoreLib.Developer.Commercialization{
    /// <summary>
    /// A class to help manage the Issue Date of a Card
    /// </summary>
    public class IssueDate{
        protected int month = 1;
        protected int year = 12;

        /// <summary>
        /// 
        /// </summary>
        public IssueDate(){

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public IssueDate(int month, int year){
            SetMonth(month);
            SetYear(year);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        public void SetMonth(int month){
            if (month <= 12 && month >= 1){
                this.month = month;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year"></param>
        public void SetYear(int year){
            this.year = year;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMonth(){
            return month;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetYear(){
            return year;
        }
    }
}