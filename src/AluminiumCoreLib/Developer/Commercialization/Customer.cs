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
using AluminiumCoreLib.Utilities;
using System;

namespace AluminiumCoreLib.Developer.Commercialization{
    /// <summary>
    /// 
    /// </summary>
    public class Customer{
        private bool isABusinessCustomer;
        private bool isAPayingCustomer;
        private ObjectList<Card> paymentDetails;

        /// <summary>
        /// 
        /// </summary>
        public Customer(){
            paymentDetails = new ObjectList<Card>();
            //Assume that the customer is not a business customer.
            isABusinessCustomer = false;
            //Assume that the customer is a paying customer.
            isAPayingCustomer = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isABusinessCustomer"></param>
        public Customer(bool isABusinessCustomer){
            paymentDetails = new ObjectList<Card>();
            this.isABusinessCustomer = isABusinessCustomer;
            //Assume that the customer is a paying customer.
            isAPayingCustomer = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isABusinessCustomer"></param>
        /// <param name="isAPayingCustomer"></param>
        public Customer(bool isABusinessCustomer, bool isAPayingCustomer){
            paymentDetails = new ObjectList<Card>();
            this.isABusinessCustomer = isABusinessCustomer;
            this.isAPayingCustomer = isAPayingCustomer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card){
            paymentDetails.Add(card);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="card"></param>
        public void RemoveCard(Card card){
            paymentDetails.Remove(card);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveCard(int index){
            paymentDetails.Remove(index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsABusinessCustomer(){
            return isABusinessCustomer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAPayingCustomer(){
            return isAPayingCustomer;
        }
    }
}