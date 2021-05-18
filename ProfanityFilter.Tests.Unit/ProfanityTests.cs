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
using ProfanityDetector;
using ProfanityDetector.Interfaces;

namespace ProfanityDetector.Tests.Unit
{
    [TestClass]
    public class ProfanityTests
    {
        [TestMethod]
        public void ConstructorSetsAllowList()
        {
            IProfanityFilter filter = new ProfanityFilter();
            Assert.IsNotNull(filter.AllowList);
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
        public void IsProfanityReturnsFalseForWordOnTheAllowList()
        {
            var filter = new ProfanityFilter();
            Assert.IsTrue(filter.IsProfanity("shitty"));

            filter.AllowList.Add("shitty");

            Assert.IsFalse(filter.IsProfanity("shitty"));
        }

        [TestMethod]
        public void IsProfanityReturnsFalseForWordOnTheAllowListWithMixedCase()
        {
            var filter = new ProfanityFilter();
            Assert.IsTrue(filter.IsProfanity("shitty"));

            filter.AllowList.Add("shitty");

            Assert.IsFalse(filter.IsProfanity("ShiTty"));
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
        public void DetectAllProfanitiesReturns2SwearWordsWithCommas()
        {
            var filter = new ProfanityFilter();
            var swearList = filter.DetectAllProfanities("You are, a complete twat, and a @dick:");

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

            Assert.AreEqual(2, swearList.Count);
            Assert.AreEqual("2 girls 1 cup", swearList[0]);
            Assert.AreEqual("twatting", swearList[1]);
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
            filter.AllowList.Add("scunthorpe");
            filter.AllowList.Add("penistone");

            var swearList = filter.DetectAllProfanities("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.");

            Assert.AreEqual(4, swearList.Count);
            Assert.AreEqual("fucking", swearList[0]);
            Assert.AreEqual("cock", swearList[1]);
            Assert.AreEqual("fuck", swearList[2]);
            Assert.AreEqual("shit", swearList[3]);
        }

        [TestMethod]
        public void DetectAllProfanitiesBlocksAllowlist()
        {
            var filter = new ProfanityFilter();
            filter.AllowList.Add("tit");

            var swearList = filter.DetectAllProfanities("You are a complete twat and a total tit.", true);

            Assert.AreEqual(1, swearList.Count);
            Assert.AreEqual("twat", swearList[0]);
        }


        [TestMethod]
        public void DetectAllProfanitiesScunthorpeWithDuplicatesTurnedOff()
        {
            var filter = new ProfanityFilter();
            filter.AllowList.Add("scunthorpe");
            filter.AllowList.Add("penistone");

            var swearList = filter.DetectAllProfanities("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.", true);

            Assert.AreEqual(3, swearList.Count);
            Assert.AreEqual("cock", swearList[1]);
            Assert.AreEqual("fucking", swearList[0]);
            Assert.AreEqual("shit", swearList[2]);
        }

        [TestMethod]
        public void DetectAllProfanitiesScunthorpeWithDuplicatesTurnedOffAndNoAllowList()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.", true);

            Assert.AreEqual(3, swearList.Count);
            Assert.AreEqual("cock", swearList[1]);
            Assert.AreEqual("fucking", swearList[0]);
            Assert.AreEqual("shit", swearList[2]);
        }

        [TestMethod]
        public void DetectAllProfanitiesMultipleScunthorpes()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("Scunthorpe Scunthorpe", true);

            Assert.AreEqual(0, swearList.Count);
        }

        [TestMethod]
        public void DetectAllProfanitiesMultipleScunthorpesSingleCunt()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("Scunthorpe cunt Scunthorpe", true);

            Assert.AreEqual(1, swearList.Count);
            Assert.AreEqual("cunt", swearList[0]);
        }

        [TestMethod]
        public void DetectAllProfanitiesMultipleScunthorpesMultiCunt()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("Scunthorpe cunt Scunthorpe cunt", true);

            Assert.AreEqual(1, swearList.Count);
            Assert.AreEqual("cunt", swearList[0]);
        }

        [TestMethod]
        public void DetectAllProfanitiesScunthorpePenistone()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("ScUnThOrPePeNiStOnE", true);

            Assert.AreEqual(0, swearList.Count);
        }

        [TestMethod]
        public void DetectAllProfanitiesScunthorpePenistoneOneKnob()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("ScUnThOrPePeNiStOnE KnOb", true);

            Assert.AreEqual(1, swearList.Count);
            Assert.AreEqual("knob", swearList[0]);
        }

        [TestMethod]
        public void DetectAllProfanitiesLongerSentence()
        {
            var filter = new ProfanityFilter();

            var swearList = filter.DetectAllProfanities("You are a stupid little twat, and you like to blow your load in an alaskan pipeline.", true);

            Assert.AreEqual(4, swearList.Count);
            Assert.AreEqual("alaskan pipeline", swearList[0]);
            Assert.AreEqual("blow your load", swearList[1]);
            Assert.AreEqual("stupid", swearList[2]);
            Assert.AreEqual("twat", swearList[3]);
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
            filter.AllowList.Add("scunthorpe");
            filter.AllowList.Add("penistone");

            var censored = filter.CensorString("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.", '*');
            var result = "I ******* live in Scunthorpe and it is a **** place to live. I would much rather live in penistone you great big **** ****.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOutNoAllowList()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("I fucking live in Scunthorpe and it is a shit place to live. I would much rather live in penistone you great big cock fuck.", '*');
            var result = "I ******* live in Scunthorpe and it is a **** place to live. I would much rather live in penistone you great big **** ****.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOutNoAllowListMixedCase()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("I Fucking Live In Scunthorpe And It Is A Shit Place To Live. I Would Much Rather Live In Penistone You Great Big Cock Fuck.", '*');
            var result = "I ******* Live In Scunthorpe And It Is A **** Place To Live. I Would Much Rather Live In Penistone You Great Big **** ****.";

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
        public void CensoredStringReturnsStringWithSingleScunthorpe()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("Scunthorpe");
            var result = "Scunthorpe";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithSingleScunthorpeMixedCase()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("ScUnThOrPe");
            var result = "ScUnThOrPe";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithSingleScunthorpeMixedCase2()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("ScUnThOrPePeNiStOnE");
            var result = "ScUnThOrPePeNiStOnE";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithSingleScunthorpeAllLower()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("scunthorpe");
            var result = "scunthorpe";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringDoubleCunt()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("cunt cunt");
            var result = "**** ****";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithScunthorpeBasedDoubleCunt()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("scunthorpe cunt");
            var result = "scunthorpe ****";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithDoubleScunthorpeBasedDoubleCunt()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("scunthorpe scunthorpe cunt");
            var result = "scunthorpe scunthorpe ****";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithMultiScunthorpeBasedMultiCunt()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("cunt scunthorpe cunt scunthorpe cunt");
            var result = "**** scunthorpe **** scunthorpe ****";

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
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut2WithSlashes()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("2 girls 1 cup, is my favourite twatting video.", '/');
            var result = "/ ///// / ///, is my favourite //////// video.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut2WithQuotes()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("2 girls 1 cup, is my favourite twatting video.", '\"');
            var result = "\" \"\"\"\"\" \" \"\"\", is my favourite \"\"\"\"\"\"\"\" video.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsStringWithProfanitiesBleepedOut2WithExclaimationMark()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("2 girls 1 cup, is my favourite twatting video.", '!');
            var result = "! !!!!! ! !!!, is my favourite !!!!!!!! video.";

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
        public void CensoredStringReturnsCensoredStringWithTrailingSpace()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("Hello you little fuck ");
            var result = "Hello you little **** ";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringThatIsOnlySpaces()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("     ");
            var result = "     ";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringThatisOnlyNonAlphaNumericCharacters()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("!@£$*&^&$%^$£%£$@£$£@$£$%%^");
            var result = "!@£$*&^&$%^$£%£$@£$£@$£$%%^";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a motherfucker1", '*', true);
            var result = "You are a *************";

            Assert.AreEqual(censored, result);
        }


        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker2()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a motherfucker123", '*', true);
            var result = "You are a ***************";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker3()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a 1motherfucker", '*', true);
            var result = "You are a *************";

            Assert.AreEqual(censored, result);
        }


        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker4()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a 123motherfucker", '*', true);
            var result = "You are a ***************";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker5()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a 123motherfucker123", '*', true);
            var result = "You are a ******************";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker6()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("motherfucker1", '*', true);
            var result = "*************";

            Assert.AreEqual(censored, result);
        }


        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker7()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("motherfucker1  ", '*', true);
            var result = "*************  ";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker8()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("  motherfucker1", '*', true);
            var result = "  *************";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker9()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("  motherfucker1  ", '*', true);
            var result = "  *************  ";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker10()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a motherfucker1 and a fucking twat3.", '*', true);
            var result = "You are a ************* and a ******* *****.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker11()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("You are a motherfucker1 and a 'fucking twat3'.", '*', true);
            var result = "You are a ************* and a '******* *****'.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringMotherfucker12()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("I've had 10 beers, and you are a motherfucker1 and a 'fucking twat3'.", '*', true);
            var result = "I've had 10 beers, and you are a ************* and a '******* *****'.";

            Assert.AreEqual(censored, result);
        }

        [TestMethod]
        public void CensoredStringReturnsCensoredStringonEmptytString()
        {
            var filter = new ProfanityFilter();

            var censored = filter.CensorString("");
            var result = "";

            Assert.AreEqual(censored, result);
        }


        [TestMethod]
        public void GetCompleteWordReturnsScunthorpeRangeMidSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("I live in Scunthorpe and it is full of twats", "cunt");

            Assert.AreEqual(result.Value.Item1, 10);
            Assert.AreEqual(result.Value.Item2, 20);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void GetCompleteWordReturnsScunthorpeRangeAtStartOfSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("Scunthorpe is my favourite place and it is full of cunts.", "cunt");

            Assert.AreEqual(result.Value.Item1, 0);
            Assert.AreEqual(result.Value.Item2, 10);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void GetCompleteWordReturnsScunthorpeRangeAtEndOfSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("I totally hate living in Scunthorpe.", "cunt");

            Assert.AreEqual(result.Value.Item1, 25);
            Assert.AreEqual(result.Value.Item2, 35);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void GetCompleteWordReturnsScunthorpeRangeAtEndOfSentenceNoFullStop()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("I totally hate living in Scunthorpe", "cunt");

            Assert.AreEqual(result.Value.Item1, 25);
            Assert.AreEqual(result.Value.Item2, 35);
            Assert.AreEqual(result.Value.Item3, "scunthorpe");
        }

        [TestMethod]
        public void GetCompleteWordReturnsCuntFromMidSentence()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("You are a cunt flap.", "cunt");

            Assert.AreEqual(result.Value.Item1, 10);
            Assert.AreEqual(result.Value.Item2, 14);
            Assert.AreEqual(result.Value.Item3, "cunt");
        }

        [TestMethod]
        public void GetCompleteWordReturnsCuntFromSingleWordString()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("cunt", "cunt");

            Assert.AreEqual(result.Value.Item1, 0);
            Assert.AreEqual(result.Value.Item2, 4);
            Assert.AreEqual(result.Value.Item3, "cunt");
        }

        [TestMethod]
        public void GetCompleteWordReturnsCuntFromSingleWordStringDoubleCunt()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("cunt cunt", "cunt");

            Assert.AreEqual(result.Value.Item1, 0);
            Assert.AreEqual(result.Value.Item2, 4);
            Assert.AreEqual(result.Value.Item3, "cunt");
        }

        [TestMethod]
        public void GetCompleteWordReturnsNullIfWordNotFound()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("I am a banana and I like to jump.", "cunt");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCompleteWordReturnsNullIfEmptyInputString()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord("", "cunt");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCompleteWordReturnsNullIfNullInputString()
        {
            var filter = new ProfanityFilter();
            var result = filter.GetCompleteWord(null, "cunt");

            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void ContainsProfanityReturnsFalseIfNullOrEmptyInputString(string input)
        {
            var filter = new ProfanityFilter();
            var result = filter.ContainsProfanity(input);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsProfanityReturnsTrueWhenProfanityExists()
        {
            var filter = new ProfanityFilter();
            var result = filter.ContainsProfanity("Scunthorpe");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsProfanityReturnsTrueWhenMultipleProfanitiesExist()
        {
            var filter = new ProfanityFilter();
            var result = filter.ContainsProfanity("Scuntarsefuckhorpe");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsProfanityReturnsFalseWhenMultipleProfanitiesExistAndAreAllowed()
        {
            var filter = new ProfanityFilter();
            filter.AllowList.Add("cunt");
            filter.AllowList.Add("arse");

            var result = filter.ContainsProfanity("Scuntarsehorpe");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsProfanityReturnsFalseWhenProfanityDoesNotExist()
        {
            var filter = new ProfanityFilter();
            var result = filter.ContainsProfanity("Ireland");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsProfanityReturnsFalseWhenProfanityIsAaa()
        {
            var filter = new ProfanityFilter();
            var result = filter.ContainsProfanity("aaa");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsProfanityReturnsTrueWhenProfanityIsADollarDollar()
        {
            var filter = new ProfanityFilter();
            var result = filter.ContainsProfanity("a$$");

            Assert.IsTrue(result);
        }
    }
}