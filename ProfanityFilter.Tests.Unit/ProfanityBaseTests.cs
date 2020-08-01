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
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfanityFilter.Interfaces;

namespace ProfanityFilter.Tests.Unit
{
    [TestClass]
    public class ProfanityBaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsArgumentNullExceptionForNullWordListArray()
        {
            _ = new ProfanityFilter((string[])null);
        }

        [TestMethod]
        public void ConstructorOverridesProfanityListWithArray()
        {
            string[] _wordList =
            {
               "fuck",
               "shit",
               "bollocks"
            };

            IProfanityFilter filter = new ProfanityFilter(_wordList);

            Assert.AreEqual(3, filter.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsArgumentNullExceptionForNullWordList()
        {
            _ = new ProfanityFilter((List<string>)null);
        }

        [TestMethod]
        public void ConstructorOverridesProfanityList()
        {
            string[] _wordList =
            {
               "fuck",
               "shit",
               "bollocks"
            };

            IProfanityFilter filter = new ProfanityFilter(new List<string>(_wordList));

            Assert.AreEqual(3, filter.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProfanityThrowsArgumentNullExceptionForNullProfanity()
        {
            var filter = new ProfanityBase();
            filter.AddProfanity((string)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProfanityThrowsArgumentNullExceptionForEmptyProfanityString()
        {
            var filter = new ProfanityBase();
            filter.AddProfanity((string)string.Empty);
        }

        [TestMethod]
        public void AddProfanityAddsToList()
        {
            var filter = new ProfanityFilter();
            Assert.IsFalse(filter.IsProfanity("fluffy"));

            filter.AddProfanity("fluffy");
            Assert.IsTrue(filter.IsProfanity("fluffy"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddProfanityThrowsArgumentNullExceptionForNullProfanityArray()
        {
            var filter = new ProfanityBase();
            filter.AddProfanity((string[])null);
        }

        [TestMethod]
        public void AddProfanityAddsToProfanityArray()
        {
            string[] _wordList =
            {
               "fuck",
               "shit",
               "bollocks"
            };

            var filter = new ProfanityBase();

            filter.Clear();
            Assert.AreEqual(0, filter.Count);

            filter.AddProfanity(new List<string>(_wordList));

            Assert.AreEqual(3, filter.Count);
        }

        [TestMethod]
        public void AddProfanityAddsToProfanityList()
        {
            string[] _wordList =
            {
               "fuck",
               "shit",
               "bollocks"
            };

            var filter = new ProfanityBase();

            filter.Clear();
            Assert.AreEqual(0, filter.Count);

            filter.AddProfanity(_wordList);

            Assert.AreEqual(3, filter.Count);
        }

        [TestMethod]
        public void ReturnCountForDetaultProfanityList()
        {
            var filter = new ProfanityBase();
            int count = filter.Count;

            Assert.AreEqual(count, 1626);
        }

        [TestMethod]
        public void ClearEmptiesProfanityList()
        {
            var filter = new ProfanityBase();

            Assert.AreEqual(1626, filter.Count);

            filter.Clear();

            Assert.AreEqual(0, filter.Count);
        }

        [TestMethod]
        public void RemoveDeletesAProfanity()
        {
            var filter = new ProfanityBase();

            Assert.AreEqual(1626, filter.Count);

            filter.RemoveProfanity("shit");

            Assert.AreEqual(1625, filter.Count);
        }

        [TestMethod]
        public void RemoveDeletesAProfanityAndReturnsTrue()
        {
            var filter = new ProfanityBase();

            Assert.AreEqual(1626, filter.Count);

            Assert.IsTrue(filter.RemoveProfanity("shit"));

            Assert.AreEqual(1625, filter.Count);
        }

        [TestMethod]
        public void RemoveDeletesAProfanityAndIsProfanitiyIgnoresIt()
        {
            var filter = new ProfanityFilter();

            Assert.IsTrue(filter.IsProfanity("shit"));
            filter.RemoveProfanity("shit");

            Assert.IsFalse(filter.IsProfanity("shit"));
        }

        [TestMethod]
        public void RemoveDeletesAProfanityAndReturnsFalseIfProfanityDoesntExist()
        {
            var filter = new ProfanityBase();

            Assert.AreEqual(1626, filter.Count);

            Assert.IsFalse(filter.RemoveProfanity("fluffy"));

            Assert.AreEqual(1626, filter.Count);
        }


        [TestMethod]
        public void RemoveListDeletesProfanitiesFromPrimaryList()
        {
            var filter = new ProfanityBase();

            Assert.AreEqual(1626, filter.Count);

            List<string> listOfProfanitiesToRemove = new List<string>
            {
                "shit",
                "fuck",
                "cock"
            };

            Assert.IsTrue(filter.RemoveProfanity(listOfProfanitiesToRemove));

            Assert.AreEqual(1623, filter.Count);
        }

        [TestMethod]
        public void RemoveArrayDeletesProfanitiesFromPrimaryList()
        {
            var filter = new ProfanityBase();

            Assert.AreEqual(1626, filter.Count);

            string []listOfProfanitiesToRemove = new string[]
            {
                "shit",
                "fuck",
                "cock"
            };

            Assert.IsTrue(filter.RemoveProfanity(listOfProfanitiesToRemove));

            Assert.AreEqual(1623, filter.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveProfanityThrowsArgumentNullExceptionIfListNull()
        {
            var filter = new ProfanityBase();

            List<string> listOfProfanitiesToRemove = null;

            filter.RemoveProfanity(listOfProfanitiesToRemove);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveProfanityThrowsArgumentNullExceptionIfArrayNull()
        {
            var filter = new ProfanityBase();

            string[] listOfProfanitiesToRemove = null;

            filter.RemoveProfanity(listOfProfanitiesToRemove);
        }
    }
}