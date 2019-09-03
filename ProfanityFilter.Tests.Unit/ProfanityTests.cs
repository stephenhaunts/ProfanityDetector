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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProfanityFilter;

namespace ProfanityFilter.Tests.Unit
{
    [TestClass]
    public class ProfanityTests
    {
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
            
            Assert.AreEqual(4, swearList.Count);
            Assert.AreEqual("twat", swearList[0]);
            Assert.AreEqual("dick", swearList[1]);
        }
        
        [TestMethod]
        public void DetectAllProfanitiesReturns2SwearWordsforMixedCase()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("You are a complete tWat and a DiCk.");
            
            Assert.AreEqual(4, swearList.Count);
            Assert.AreEqual("twat", swearList[0]);
            Assert.AreEqual("dick", swearList[1]);
        }
        
        [TestMethod]
        public void DetectAllProfanitiesReturns1SwearPhrase()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("2 girls 1 cup");
            
            Assert.AreEqual(2, swearList.Count);
            Assert.AreEqual("2 girls 1 cup", swearList[0]);
            Assert.AreEqual("2 girls 1 cup", swearList[1]);
        }
    }
}