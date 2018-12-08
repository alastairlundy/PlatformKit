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
    /// A class to help store card details
    /// </summary>
    public class Card{
        private string firstName;
        private string lastName;
        private Address billingAddress;
        private IssueDate issueDate;
        private ExpirationDate expirationDate;
        /// <summary>
        /// 
        /// </summary>
        public Card(){
            firstName = "";
            lastName = "";
            billingAddress = new Address();
            issueDate = new IssueDate();
            expirationDate = new ExpirationDate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public Card(string firstName, string lastName){
            this.firstName = firstName;
            this.lastName = lastName;
            billingAddress = new Address();
            issueDate = new IssueDate();
            expirationDate = new ExpirationDate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="billingAddress"></param>
        public Card(string firstName, string lastName, Address billingAddress){
            this.firstName = firstName;
            this.lastName = lastName;
            this.billingAddress = billingAddress;
            issueDate = new IssueDate();
            expirationDate = new ExpirationDate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="billingAddress"></param>
        /// <param name="issueDate"></param>
        public Card(string firstName, string lastName, Address billingAddress, IssueDate issueDate){
            this.firstName = firstName;
            this.lastName = lastName;
            this.issueDate = issueDate;
            this.billingAddress = billingAddress;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="billingAddress"></param>
        /// <param name="issueDate"></param>
        /// <param name="expirationDate"></param>
        public Card(string firstName, string lastName, Address billingAddress, IssueDate issueDate, ExpirationDate expirationDate){
            this.firstName = firstName;
            this.lastName = lastName;
            this.issueDate = issueDate;
            this.billingAddress = billingAddress;
            this.issueDate = issueDate;
            this.expirationDate = expirationDate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetFirstName(){
            return firstName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstName"></param>
        public void SetFirstName(string firstName){
            this.firstName = firstName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLastName(){
            return lastName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastName"></param>
        public void SetLastName(string lastName){
            this.lastName = lastName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Address GetBillingAddress(){
            return billingAddress;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="billingAddress"></param>
        public void SetBillingAddress(Address billingAddress){
            this.billingAddress = billingAddress;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IssueDate GetIssueDate(){
            return issueDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issueDate"></param>
        public void SetIssueDate(IssueDate issueDate){
            this.issueDate = issueDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ExpirationDate GetExpirationDate(){
            return expirationDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expirationDate"></param>
        public void SetExpirationDate(ExpirationDate expirationDate){
            this.expirationDate = expirationDate;
        }
    }
}