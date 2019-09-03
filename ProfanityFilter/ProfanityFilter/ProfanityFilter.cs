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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProfanityFilter
{
    /// <summary>
    /// 
    /// This class will detect profanity and racial slurs contained within some text and return an indication flag.
    ///
    /// </summary>
    public partial class ProfanityFilter : IProfanityFilter
    {
        /// <summary>
        /// Check whether a specific word is in the profanity list.
        /// </summary>
        /// <param name="word">The word to check in the profanity list.</param>
        /// <returns>True if the word is considered a profanity, False otherwise.</returns>
        public bool IsProfanity(string word)
        {
            return !string.IsNullOrEmpty(word) && _wordList.Contains(word.ToLower());
        }

        /// <summary>
        /// For a given sentence, report the first profanity detected in the sentence.
        /// </summary>
        /// <param name="sentence">The sentence to check for profanities.</param>
        /// <returns>The profanity that has been detected.</returns>
        public string StringContainsFirstProfanity(string sentence)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return string.Empty;
            }

            sentence = sentence.ToLower();
            var words = sentence.Split(' ');

            if (_wordList.Contains(sentence))
            {
                return sentence;
            }

            foreach (var profanity in words)
            {
                if (_wordList.Contains(profanity.ToLower()))
                {
                    return profanity;
                }
            }

            foreach (var profanity in _wordList)
            {
                if (sentence.Contains(profanity.ToLower()))
                {
                    return profanity;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// For a given sentence, return a list of all the detected profanities.
        /// </summary>
        /// <param name="sentence">The sentence to check for profanities.</param>
        /// <returns>A read only list of detected profanities.</returns>
        public ReadOnlyCollection<string> DetectAllProfanities(string sentence)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return new ReadOnlyCollection<string>(new List<string>());
            }

            sentence = sentence.ToLower();
            sentence = sentence.Replace(".", "");
            sentence = sentence.Replace(",", "");

            var words = sentence.Split(' ');
            var swearList = words.Where(profanity => _wordList.Contains(profanity)).ToList();

            if (_wordList.Contains(sentence))
            {
                swearList.Add(sentence);
            }

            return new ReadOnlyCollection<string>(swearList);
        }
    }
}