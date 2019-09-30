/*
MIT License
Copyright (c) 2019 
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfanityFilter;
using ProfanityFilter.Interfaces;

namespace ProfanityFilter.Tests.Unit
{
    [TestClass]
    public class WhiteListTests
    {
        [TestMethod]
        public void ConstructorSetsWhiteList()
        {
            IProfanityFilter filter = new ProfanityFilter();
            Assert.IsNotNull(filter.WhiteList);
        }

        [TestMethod]
        public void ConstructorSetsWhiteListToEmpty()
        {
            IWhiteList filter = new WhiteList();
            Assert.AreEqual(0, filter.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddThrowsArgumentNullExceptionIfInputStringIsNullOrEmpty()
        {
            var whiteList = new WhiteList();

            whiteList.Add("");
        }

        [TestMethod]        
        public void AddInsertsItemIntoTheWhiteList()
        {
            var whiteList = new WhiteList();

            Assert.AreEqual(0, whiteList.Count);

            whiteList.Add("Scunthorpe");

            Assert.AreEqual(1, whiteList.Count);
        }

        [TestMethod]
        public void AddInsertsLowercaseItemIntoTheWhiteList()
        {
            var whiteList = new WhiteList();           

            whiteList.Add("Scunthorpe");

            Assert.IsTrue(whiteList.Contains("scunthorpe"));
        }

        [TestMethod]
        public void AddDoesntAllowDuplicateEntries()
        {
            var whiteList = new WhiteList();

            Assert.AreEqual(0, whiteList.Count);

            whiteList.Add("Scunthorpe");

            Assert.AreEqual(1, whiteList.Count);

            whiteList.Add("Scunthorpe");

            Assert.AreEqual(1, whiteList.Count);
        }

        [TestMethod]
        public void AddDoesntAllowDuplicateEntriesOfMixedCase()
        {
            var whiteList = new WhiteList();

            Assert.AreEqual(0, whiteList.Count);

            whiteList.Add("scunthorpe");

            Assert.AreEqual(1, whiteList.Count);

            whiteList.Add("Scunthorpe");

            Assert.AreEqual(1, whiteList.Count);

            whiteList.Add("ScunThorpe");

            Assert.AreEqual(1, whiteList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContainsThrowsArgumentNullExceptionIfInputStringIsNullOrEmpty()
        {
            var whiteList = new WhiteList();

            whiteList.Contains("");
        }

        [TestMethod]
        public void ContainsReturnsTrueForWhiteListItemInTheList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.IsTrue(whiteList.Contains("Scunthorpe"));
            Assert.IsTrue(whiteList.Contains("Penistone"));
        }

        [TestMethod]
        public void ContainsReturnsTrueForWhiteListItemInTheListWithMixedCase()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.IsTrue(whiteList.Contains("ScunThorpe"));
            Assert.IsTrue(whiteList.Contains("PeniStone"));
        }

        [TestMethod]
        public void ContainsReturnsFalseForWhiteListItemNotInTheList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.IsFalse(whiteList.Contains("Wibble"));
            Assert.IsFalse(whiteList.Contains("Gobble"));
        }

        [TestMethod]
        public void CountReturnsTwoForTwoEntriesInTheList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.AreEqual(2, whiteList.Count);
        }

        [TestMethod]
        public void CountReturnsTwoForTwoEntriesInTheListAfterMixedcaseAdditions()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");
            whiteList.Add("ScunThorpe");
            whiteList.Add("PeniStone");

            Assert.AreEqual(2, whiteList.Count);
        }

        [TestMethod]
        public void ClearRemovesEntriesFromTheList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.AreEqual(2, whiteList.Count);

            whiteList.Clear();

            Assert.AreEqual(0, whiteList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveThrowsArgumentNullExceptionIfInputStringIsNullOrEmpty()
        {
            var whiteList = new WhiteList();

            whiteList.Remove("");
        }

        [TestMethod]
        public void RemoveEntryFromTheWhiteList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.AreEqual(2, whiteList.Count);

            whiteList.Remove("Scunthorpe");

            Assert.AreEqual(1, whiteList.Count);
            Assert.IsFalse(whiteList.Contains("Scunthorpe"));
            Assert.IsTrue(whiteList.Contains("Penistone"));
        }

        [TestMethod]
        public void RemoveMixedCaseEntryFromTheWhiteList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.AreEqual(2, whiteList.Count);

            whiteList.Remove("ScUnThOrPe");

            Assert.AreEqual(1, whiteList.Count);
            Assert.IsFalse(whiteList.Contains("Scunthorpe"));
            Assert.IsTrue(whiteList.Contains("Penistone"));
        }

        [TestMethod]
        public void RemoveEntryFromTheWhiteListReturnsTrue()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.AreEqual(2, whiteList.Count);

            Assert.IsTrue(whiteList.Remove("Scunthorpe"));
        }

        [TestMethod]
        public void RemoveNonExistingEntryFromTheWhiteListReturnsFalse()
        { 
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            Assert.AreEqual(2, whiteList.Count);

            Assert.IsFalse(whiteList.Remove("DoesNotExist"));
        }

        [TestMethod]
        public void ToListReturnsReadOnlyCollectionContainingWhiteList()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            var readonlyList = whiteList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(2, readonlyList.Count);

            Assert.AreEqual("scunthorpe", readonlyList[0]);
            Assert.AreEqual("penistone", readonlyList[1]);
        }

        [TestMethod]
        public void ToListReturnsEmptyReadOnlyCollectionContaining()
        {
            var whiteList = new WhiteList();

            var readonlyList = whiteList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(0, readonlyList.Count);
        }

        [TestMethod]
        public void ToListReturnsReadOnlyCollectionContainingWhiteListAfterListModification()
        {
            var whiteList = new WhiteList();
            whiteList.Add("Scunthorpe");
            whiteList.Add("Penistone");

            var readonlyList = whiteList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(2, readonlyList.Count);

            Assert.AreEqual("scunthorpe", readonlyList[0]);
            Assert.AreEqual("penistone", readonlyList[1]);

            whiteList.Add("Bugger");
            whiteList.Add("Plonker");

            readonlyList = whiteList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(4, readonlyList.Count);

            Assert.AreEqual("scunthorpe", readonlyList[0]);
            Assert.AreEqual("penistone", readonlyList[1]);
            Assert.AreEqual("bugger", readonlyList[2]);
            Assert.AreEqual("plonker", readonlyList[3]);
        }
    }
}