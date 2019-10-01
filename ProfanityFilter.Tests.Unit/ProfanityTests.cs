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
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfanityFilter;
using ProfanityFilter.Interfaces;

namespace ProfanityFilter.Tests.Unit
{
    [TestClass]
    public class ProfanityTests
    {
        public class TestWhiteLst : IWhiteList
        {
            public ReadOnlyCollection<string> ToList => throw new NotImplementedException();
            public void Add(string wordToWhitelist) => throw new NotImplementedException();
            public void Clear() => throw new NotImplementedException();
            public bool Contains(string wordToCheck) => throw new NotImplementedException();
            public int Count => throw new NotImplementedException();
            public bool Remove(string wordToRemove) => throw new NotImplementedException();
        }

        [TestMethod]
        public void ConstructorSetsWhiteList()
        {
            IProfanityFilter filter = new ProfanityFilter();
            Assert.IsNotNull(filter.WhiteList);
        }

        [TestMethod]
        public void ConstructorSetsWhiteListThatIsInjectedIn()
        {
            IWhiteList whiteList = new TestWhiteLst();

            IProfanityFilter filter = new ProfanityFilter(whiteList);
            Assert.AreEqual(whiteList, filter.WhiteList);
        }

        [TestMethod]
        public void IsProfanityReturnsTrueForSwearWord()
        {
            var filter = new ProfanityFilter();
            Assert.IsTrue(filter.IsProfanity("arsehole"));
        }
        
        [TestMethod]
        public void IsProfanityReturnsTrueForSwearWord2()
        {
            var filter = new ProfanityFilter();
            Assert.IsTrue(filter.IsProfanity("shitty"));
        }
        
        [TestMethod]
        public void IsProfanityReturnsFalseForNonSwearWord()
        {
            var filter = new ProfanityFilter();
            Assert.IsFalse(filter.IsProfanity("fluffy"));
        }
       
        
        [TestMethod]
        public void IsProfanityReturnsFalseForEmptyString()
        {
            var filter = new ProfanityFilter();
            Assert.IsFalse(filter.IsProfanity(string.Empty));
        }
        
        [TestMethod]
        public void IsProfanityReturnsFalseForNullString()
        {
            var filter = new ProfanityFilter();
            Assert.IsFalse(filter.IsProfanity(null));
        }

        [TestMethod]
        public void IsProfanityReturnsFalseForWordOnTheWhiteList()
        {
            var filter = new ProfanityFilter();
            Assert.IsTrue(filter.IsProfanity("shitty"));

            filter.WhiteList.Add("shitty");

            Assert.IsFalse(filter.IsProfanity("shitty"));
        }

        [TestMethod]
        public void IsProfanityReturnsFalseForWordOnTheWhiteListWithMixedCase()
        {
            var filter = new ProfanityFilter();
            Assert.IsTrue(filter.IsProfanity("shitty"));

            filter.WhiteList.Add("shitty");

            Assert.IsFalse(filter.IsProfanity("ShiTty"));
        }

        [TestMethod]
        public void StringContainsProfanityReturnsEmptyStringForEmptyInput()
        {
            var filter = new ProfanityFilter();
            var swearWord = filter.StringContainsFirstProfanity(string.Empty);
            
            Assert.AreEqual(string.Empty, swearWord);
        }
        
        [TestMethod]
        public void StringContainsProfanityReturnsEmptyStringForNullInput()
        {
            var filter = new ProfanityFilter();
            var swearWord = filter.StringContainsFirstProfanity(null);
            
            Assert.AreEqual(string.Empty, swearWord);
        }
        
        [TestMethod]
        public void StringContainsProfanityReturnsSwearWordForSentenceContainingANaughtyWordForPartialWordMatch()
        {
            var filter = new ProfanityFilter();
            var swearWord = filter.StringContainsFirstProfanity("Mary had a little shitty lamb");
            
            Assert.AreEqual("shitty", swearWord);
        }
        
        [TestMethod]
        public void StringContainsProfanityReturnsSwearWordForSentenceContainingANaughtyWordForFullWordMatch()
        {
            var filter = new ProfanityFilter();
            var swearWord = filter.StringContainsFirstProfanity("Mary had a little shit lamb");
            
            Assert.AreEqual("shit", swearWord);
        }
        
        [TestMethod]
        public void StringContainsProfanityReturnsFirstSwearWordForSentenceContainingMultipleWearWords()
        {
            var filter = new ProfanityFilter();
            var swearWord = filter.StringContainsFirstProfanity("Mary had a little shit lamb who was a little fucker.");
            
            Assert.AreEqual("shit", swearWord);
        }
        
        [TestMethod]
        public void StringContainsProfanityReturnsFirstSwearWordForSentenceContainingMultipleWearWordsForMixedCase()
        {
            var filter = new ProfanityFilter();
            var swearWord = filter.StringContainsFirstProfanity("Mary had a little ShIt lamb who was a little FuCkEr.");
            
            Assert.AreEqual("shit", swearWord);
        }

        [TestMethod]
        public void StringContainsProfanityScunthorpeTest()
        {
            var filter = new ProfanityFilter();
            filter.WhiteList.Add("scunthorpe");
            var swearWord = filter.StringContainsFirstProfanity("I live in scunthorpe and I fucking hate it.");

            Assert.AreEqual("fucking", swearWord);
        }

        [TestMethod]
        public void StringContainsProfanityFiltersWithWhiteList()
        {
            var filter = new ProfanityFilter();
            filter.WhiteList.Add("shitty");

            var swearWord = filter.StringContainsFirstProfanity("I live in shitty scunthorpe and I fucking hate it.");

            Assert.AreEqual("fucking", swearWord);
        }

        [TestMethod]
        public void StringContainsProfanityFiltersPenistone()
        {
            var filter = new ProfanityFilter();
            filter.WhiteList.Add("shitty");

            var swearWord = filter.StringContainsFirstProfanity("I live in shitty penistone and I fucking hate it.");

            Assert.AreEqual("fucking", swearWord);
        }

        [TestMethod]
        public void DetectAllProfanitiesReturnsEmptyListForEmptyInput()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities(string.Empty);
            
            Assert.AreEqual(0, swearList.Count);
        }
        
        [TestMethod]
        public void DetectAllProfanitiesReturnsNulListForEmptyInput()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities(null);
            
            Assert.AreEqual(0, swearList.Count);
        }
        
        [TestMethod]
        public void DetectAllProfanitiesReturns2SwearWords()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("You are a complete twat and a dick.");
            
            Assert.AreEqual(2, swearList.Count);
            Assert.AreEqual("twat", swearList[1]);
            Assert.AreEqual("dick", swearList[0]);
        }
        
        [TestMethod]
        public void DetectAllProfanitiesReturns2SwearWordsforMixedCase()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("You are a complete tWat and a DiCk.");
            
            Assert.AreEqual(2, swearList.Count);
            Assert.AreEqual("twat", swearList[1]);
            Assert.AreEqual("dick", swearList[0]);
        }

        [TestMethod]
        public void DetectAllProfanitiesReturns1SwearPhrase()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("2 girls 1 cup is my favourite video");
            
            Assert.AreEqual(1, swearList.Count);
            Assert.AreEqual("2 girls 1 cup", swearList[0]);
        }

        [TestMethod]
        public void DetectAllProfanitiesReturns3SwearPhrase()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("2 girls 1 cup is my favourite twatting video");
            
            Assert.AreEqual(3, swearList.Count);
            Assert.AreEqual("2 girls 1 cup", swearList[0]);
            Assert.AreEqual("twat", swearList[1]);
            Assert.AreEqual("twatting", swearList[2]);
        }

        [TestMethod]
        public void DetectAllProfanitiesReturns2SwearPhraseBecauseOfMatchDeduplication()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("2 girls 1 cup is my favourite twatting video", true);

            Assert.AreEqual(2, swearList.Count);
            Assert.AreEqual("2 girls 1 cup", swearList[0]);            
            Assert.AreEqual("twatting", swearList[1]);
        }

        [TestMethod]
        public void DetectAllProfanitiesScunthorpe()
        {
            var filter = new ProfanityFilter();
            filter.WhiteList.Add("scunthorpe");
            filter.WhiteList.Add("penistone");

            var swearList = filter.DetectAllProfanities("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.");
            
            Assert.AreEqual(6, swearList.Count);
            Assert.AreEqual("cock", swearList[0]);
            Assert.AreEqual("fuc", swearList[1]);
            Assert.AreEqual("fuck", swearList[2]);
            Assert.AreEqual("fuckin", swearList[3]);
            Assert.AreEqual("fucking", swearList[4]);
            Assert.AreEqual("shit", swearList[5]);
        }

        [TestMethod]
        public void DetectAllProfanitiesScunthorpeWithDuplicatesTurnedOff()
        {
            var filter = new ProfanityFilter();
            filter.WhiteList.Add("scunthorpe");
            filter.WhiteList.Add("penistone");

            var swearList = filter.DetectAllProfanities("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.", true);

            Assert.AreEqual(3, swearList.Count);
            Assert.AreEqual("cock", swearList[0]);            
            Assert.AreEqual("fucking", swearList[1]);
            Assert.AreEqual("shit", swearList[2]);
        }

        [TestMethod]
        public void DetectAllProfanitiesForSingleWord()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("cock", true);

            Assert.AreEqual(1, swearList.Count);
            Assert.AreEqual("cock", swearList[0]);
        }

        [TestMethod]
        public void DetectAllProfanitiesForEmptyString()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("", true);

            Assert.AreEqual(0, swearList.Count);            
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut()
        {
            var filter = new ProfanityFilter();
            filter.WhiteList.Add("scunthorpe");
            filter.WhiteList.Add("penistone");

            var censored = filter.CensorString("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.", '*');
            var result = "I ******* live in Scunthorpe and it is a **** place to live. I would much rather live in penistone you great big **** ****.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut2()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("2 girls 1 cup, is my favourite twatting video.");
            var result = "* ***** * ***, is my favourite ******** video.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut3()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("Mary had a little shit lamb who was a little fucker.");
            var result = "Mary had a little **** lamb who was a little ******.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut4()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a stupid little twat, and you like to blow your load in an alaskan pipeline.");
            var result = "You are a ****** little ****, and you like to **** **** **** in an ******* ********.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut2WithDifferentCharacter()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("2 girls 1 cup, is my favourite twatting video.", '@');
            var result = "@ @@@@@ @ @@@, is my favourite @@@@@@@@ video.";

            Assert.AreEqual(censored, result);
        }


        [TestMethod]
        public void CensoredStringReturnsEmptyString()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("", '@');
            var result = "";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithNoCensorship()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("Hello, I am a fish.", '*');
            var result = "Hello, I am a fish.";

            Assert.AreEqual(censored, result);
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
        public void ReturnCountForDetaultProfanityList()
        {
            var filter = new ProfanityFilter();
            int count = filter.Count;

            Assert.AreEqual(count, 1636);
        }

        [TestMethod]
        public void ClearEmptiesProfanityList()
        {
            var filter = new ProfanityFilter();

            Assert.AreEqual(1636, filter.Count);

            filter.Clear();

            Assert.AreEqual(0, filter.Count);
        }

        [TestMethod]
        public void RemoveDeletesAProfanity()
        {
            var filter = new ProfanityFilter();

            Assert.AreEqual(1636, filter.Count);

            filter.RemoveProfanity("shit");

            Assert.AreEqual(1635, filter.Count);
        }

        [TestMethod]
        public void RemoveDeletesAProfanityAndReturnsTrue()
        {
            var filter = new ProfanityFilter();

            Assert.AreEqual(1636, filter.Count);

            Assert.IsTrue(filter.RemoveProfanity("shit"));

            Assert.AreEqual(1635, filter.Count);
        }

        [TestMethod]
        public void RemoveDeletesAProfanityAndReturnsFalseIfProfanityDoesntExist()
        {
            var filter = new ProfanityFilter();

            Assert.AreEqual(1636, filter.Count);

            Assert.IsFalse(filter.RemoveProfanity("fluffy"));

            Assert.AreEqual(1636, filter.Count);
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsScunthorpeRangeMidSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("I live in Scunthorpe and it is full of twats", "cunt");

            Assert.AreEqual(result.Value.Item1, 10);
            Assert.AreEqual(result.Value.Item2, 20);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsScunthorpeRangeAtStartOfSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("Scunthorpe is my favourite place and it is full of cunts.", "cunt");

            Assert.AreEqual(result.Value.Item1, 0);
            Assert.AreEqual(result.Value.Item2, 10);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsScunthorpeRangeAtEndOfSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("I totally hate living in Scunthorpe.", "cunt");

            Assert.AreEqual(result.Value.Item1, 25);
            Assert.AreEqual(result.Value.Item2, 36);
            Assert.AreEqual(result.Value.Item3, "scunthorpe.");
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsScunthorpeRangeAtEndOfSentenceNoFullStop()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("I totally hate living in Scunthorpe", "cunt");

            Assert.AreEqual(result.Value.Item1, 25);
            Assert.AreEqual(result.Value.Item2, 35);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsCuntFromMidSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("You are a cunt flap.", "cunt");

            Assert.AreEqual(result.Value.Item1, 10);
            Assert.AreEqual(result.Value.Item2, 14);
            Assert.AreEqual(result.Value.Item3, "cunt");
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsCuntFromSingleWordString()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("cunt", "cunt");

            Assert.AreEqual(result.Value.Item1, 0);
            Assert.AreEqual(result.Value.Item2, 4);
            Assert.AreEqual(result.Value.Item3, "cunt");
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsNullIfWordNotFound()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("I am a banana and I like to jump.", "cunt");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsNullIfEmptyInputString()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity("", "cunt");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IsEnclosedProfanityReturnsNullIfNullInputString()
        {
            var filter = new ProfanityFilter();
            var result = filter.IsEnclosedProfanity(null, "cunt");

            Assert.IsNull(result);
        }
    }
}