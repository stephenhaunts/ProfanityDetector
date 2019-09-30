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
using System.Globalization;
using System.Linq;
using ProfanityFilter.Interfaces;

namespace ProfanityFilter
{
    /// <summary>
    /// 
    /// This class will detect profanity and racial slurs contained within some text and return an indication flag.
    /// All words are treated as case insensitive.
    ///
    /// </summary>
    public partial class ProfanityFilter : IProfanityFilter
    {
        List<string> _profanities;
        readonly IWhiteList _whiteList;

        /// <summary>
        /// Return the white list;
        /// </summary>
        public IWhiteList WhiteList => _whiteList;


        public ProfanityFilter()
        {
            _profanities = new List<string>(_wordList);
            _whiteList = new WhiteList();
  
        }

        public ProfanityFilter(IWhiteList whiteList)
        {
            _profanities = new List<string>(_wordList);
            _whiteList = whiteList ?? throw new ArgumentNullException(nameof(whiteList));

        }

        /// <summary>
        /// Check whether a specific word is in the profanity list. IsProfanity will first
        /// check if the word exists on the whitelist. If it is on the whitelist, then false
        /// will be returned.
        /// </summary>
        /// <param name="word">The word to check in the profanity list.</param>
        /// <returns>True if the word is considered a profanity, False otherwise.</returns>
        public bool IsProfanity(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }

            // Check if the word is in the whitelist.
            if (_whiteList.Contains(word.ToLower(CultureInfo.InvariantCulture)))
            {
                return false;
            }

            return _profanities.Contains(word.ToLower());
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

            List<string> postWhiteList = new List<string>();
            foreach (string word in words)
            {
                if (!_whiteList.Contains(word.ToLower(CultureInfo.InvariantCulture)))
                {
                    postWhiteList.Add(word);
                }
            }

            foreach (var profanity in postWhiteList)
            {
                if (_profanities.Contains(profanity.ToLower()))
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