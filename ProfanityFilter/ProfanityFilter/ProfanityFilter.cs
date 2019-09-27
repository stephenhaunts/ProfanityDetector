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
        List<string> _profanities;

        public ProfanityFilter()
        {
            _profanities = new List<string>(_wordList);
        }

        /// <summary>
        /// Check whether a specific word is in the profanity list.
        /// </summary>
        /// <param name="word">The word to check in the profanity list.</param>
        /// <returns>True if the word is considered a profanity, False otherwise.</returns>
        public bool IsProfanity(string word)
        {
            return !string.IsNullOrEmpty(word) && _profanities.Contains(word.ToLower());
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

            if (_profanities.Contains(sentence))
            {
                return sentence;
            }

            foreach (var profanity in words)
            {
                if (_profanities.Contains(profanity.ToLower()))
                {
                    return profanity;
                }
            }

            foreach (var profanity in _profanities)
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
            var swearList = words.Where(profanity => _profanities.Contains(profanity)).ToList();

            if (_profanities.Contains(sentence))
            {
                swearList.Add(sentence);
            }

            swearList.AddRange(_profanities.Where(sentence.Contains));

            return new ReadOnlyCollection<string>(swearList.Distinct().ToList());
        }

        /// <summary>
        /// Add a custom profanity to the list.
        /// </summary>
        /// <param name="profanity">The profanity to add.</param>
        public void AddProfanity(string profanity)
        {
            if (string.IsNullOrEmpty(profanity))
            {
                throw new ArgumentNullException(nameof(profanity));
            }

            _profanities.Add(profanity);            
        }
    }
}