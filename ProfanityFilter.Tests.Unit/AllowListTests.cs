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
using ProfanityDetector;
using ProfanityDetector.Interfaces;

namespace ProfanityDetector.Tests.Unit
{
    [TestClass]
    public class AllowListTests
    {
        [TestMethod]
        public void ConstructorSetsAllowList()
        {
            IProfanityFilter filter = new ProfanityFilter();
            Assert.IsNotNull(filter.AllowList);
        }

        [TestMethod]
        public void ConstructorSetsAllowListToEmpty()
        {
            IAllowList filter = new AllowList();
            Assert.AreEqual(0, filter.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddThrowsArgumentNullExceptionIfInputStringIsNullOrEmpty()
        {
            var allowList = new AllowList();

            allowList.Add("");
        }

        [TestMethod]
        public void AddInsertsItemIntoTheAllowList()
        {
            var allowList = new AllowList();

            Assert.AreEqual(0, allowList.Count);

            allowList.Add("Scunthorpe");

            Assert.AreEqual(1, allowList.Count);
        }

        [TestMethod]
        public void AddInsertsLowercaseItemIntoTheAllowList()
        {
            var allowList = new AllowList();

            allowList.Add("Scunthorpe");

            Assert.IsTrue(allowList.Contains("scunthorpe"));
        }

        [TestMethod]
        public void AddDoesntAllowDuplicateEntries()
        {
            var allowList = new AllowList();

            Assert.AreEqual(0, allowList.Count);

            allowList.Add("Scunthorpe");

            Assert.AreEqual(1, allowList.Count);

            allowList.Add("Scunthorpe");

            Assert.AreEqual(1, allowList.Count);
        }

        [TestMethod]
        public void AddDoesntAllowDuplicateEntriesOfMixedCase()
        {
            var allowList = new AllowList();

            Assert.AreEqual(0, allowList.Count);

            allowList.Add("scunthorpe");

            Assert.AreEqual(1, allowList.Count);

            allowList.Add("Scunthorpe");

            Assert.AreEqual(1, allowList.Count);

            allowList.Add("ScunThorpe");

            Assert.AreEqual(1, allowList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ContainsThrowsArgumentNullExceptionIfInputStringIsNullOrEmpty()
        {
            var allowList = new AllowList();

            allowList.Contains("");
        }

        [TestMethod]
        public void ContainsReturnsTrueForAllowListItemInTheList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.IsTrue(allowList.Contains("Scunthorpe"));
            Assert.IsTrue(allowList.Contains("Penistone"));
        }

        [TestMethod]
        public void ContainsReturnsTrueForAllowListItemInTheListWithMixedCase()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.IsTrue(allowList.Contains("ScunThorpe"));
            Assert.IsTrue(allowList.Contains("PeniStone"));
        }

        [TestMethod]
        public void ContainsReturnsFalseForAllowListItemNotInTheList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.IsFalse(allowList.Contains("Wibble"));
            Assert.IsFalse(allowList.Contains("Gobble"));
        }

        [TestMethod]
        public void CountReturnsTwoForTwoEntriesInTheList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.AreEqual(2, allowList.Count);
        }

        [TestMethod]
        public void CountReturnsTwoForTwoEntriesInTheListAfterMixedcaseAdditions()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");
            allowList.Add("ScunThorpe");
            allowList.Add("PeniStone");

            Assert.AreEqual(2, allowList.Count);
        }

        [TestMethod]
        public void ClearRemovesEntriesFromTheList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.AreEqual(2, allowList.Count);

            allowList.Clear();

            Assert.AreEqual(0, allowList.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveThrowsArgumentNullExceptionIfInputStringIsNullOrEmpty()
        {
            var allowList = new AllowList();

            allowList.Remove("");
        }

        [TestMethod]
        public void RemoveEntryFromTheAllowList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.AreEqual(2, allowList.Count);

            allowList.Remove("Scunthorpe");

            Assert.AreEqual(1, allowList.Count);
            Assert.IsFalse(allowList.Contains("Scunthorpe"));
            Assert.IsTrue(allowList.Contains("Penistone"));
        }

        [TestMethod]
        public void RemoveMixedCaseEntryFromTheAllowList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.AreEqual(2, allowList.Count);

            allowList.Remove("ScUnThOrPe");

            Assert.AreEqual(1, allowList.Count);
            Assert.IsFalse(allowList.Contains("Scunthorpe"));
            Assert.IsTrue(allowList.Contains("Penistone"));
        }

        [TestMethod]
        public void RemoveEntryFromTheAllowListReturnsTrue()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.AreEqual(2, allowList.Count);

            Assert.IsTrue(allowList.Remove("Scunthorpe"));
        }

        [TestMethod]
        public void RemoveNonExistingEntryFromTheAllowListReturnsFalse()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            Assert.AreEqual(2, allowList.Count);

            Assert.IsFalse(allowList.Remove("DoesNotExist"));
        }

        [TestMethod]
        public void ToListReturnsReadOnlyCollectionContainingAllowList()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            var readonlyList = allowList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(2, readonlyList.Count);

            Assert.AreEqual("scunthorpe", readonlyList[0]);
            Assert.AreEqual("penistone", readonlyList[1]);
        }

        [TestMethod]
        public void ToListReturnsEmptyReadOnlyCollectionContaining()
        {
            var allowList = new AllowList();

            var readonlyList = allowList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(0, readonlyList.Count);
        }

        [TestMethod]
        public void ToListReturnsReadOnlyCollectionContainingAllowListAfterListModification()
        {
            var allowList = new AllowList();
            allowList.Add("Scunthorpe");
            allowList.Add("Penistone");

            var readonlyList = allowList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(2, readonlyList.Count);

            Assert.AreEqual("scunthorpe", readonlyList[0]);
            Assert.AreEqual("penistone", readonlyList[1]);

            allowList.Add("Bugger");
            allowList.Add("Plonker");

            readonlyList = allowList.ToList;
            Assert.IsNotNull(readonlyList);
            Assert.AreEqual(4, readonlyList.Count);

            Assert.AreEqual("scunthorpe", readonlyList[0]);
            Assert.AreEqual("penistone", readonlyList[1]);
            Assert.AreEqual("bugger", readonlyList[2]);
            Assert.AreEqual("plonker", readonlyList[3]);
        }
    }
}