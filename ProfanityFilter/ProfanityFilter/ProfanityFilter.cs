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
        /// Return the number of profanities in the system.
        /// </summary>
        public int Count
        {
            get
            {
                return _profanities.Count;
            }
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
            sentence = sentence.Replace(".", "");
            sentence = sentence.Replace(",", "");

            var words = sentence.Split(' ');
            var postWhiteList = FilterWordListByWhiteList(words);

            foreach (var profanity in postWhiteList)
            {
                if (_profanities.Contains(profanity.ToLower()))
                {
                    return profanity;
                }
            }

            return string.Empty;
        }

        public ReadOnlyCollection<string> DetectAllProfanities(string sentence)
        {
            return DetectAllProfanities(sentence, false);
        }

        /// <summary>
        /// For a given sentence, return a list of all the detected profanities.
        /// </summary>
        /// <param name="sentence">The sentence to check for profanities.</param>
        /// <returns>A read only list of detected profanities.</returns>
        public ReadOnlyCollection<string> DetectAllProfanities(string sentence, bool removePartialMatches)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return new ReadOnlyCollection<string>(new List<string>());
            }

            sentence = sentence.ToLower();
            sentence = sentence.Replace(".", "");
            sentence = sentence.Replace(",", "");

            var words = sentence.Split(' ');
            var postWhiteList = FilterWordListByWhiteList(words);
            List<string> swearList = new List<string>();

            // Catch whether multi-word profanities are in the white list filtered sentence.
            AddMultiWordProfanities(swearList, ConvertWordListToSentence(postWhiteList));

            // Deduplicate any partial matches, ie, if the word "twatting" is in a sentence, don't include "twat".
            if (removePartialMatches)
            {
                swearList.RemoveAll(x => swearList.Any(y => x != y && y.Contains(x)));
            }

            return new ReadOnlyCollection<string>(swearList.Distinct().ToList());
        }

        public string CensorString(string sentence)
        {
            return CensorString(sentence, '*');
        }

        public string CensorString(string sentence, char censorCharacter)
        {
            if (string.IsNullOrEmpty(sentence))
            {
                return string.Empty;
            }

            string noPunctuation = sentence;
            noPunctuation = noPunctuation.ToLower();
            noPunctuation = noPunctuation.Replace(".", "");
            noPunctuation = noPunctuation.Replace(",", "");

            var words = sentence.Split(' ');
            var postWhiteList = FilterWordListByWhiteList(words);
            List<string> swearList = new List<string>();

            // Catch whether multi-word profanities are in the white list filtered sentence.
            AddMultiWordProfanities(swearList, ConvertWordListToSentence(postWhiteList));

            string censored = sentence;

            foreach (string word in swearList.OrderByDescending(x => x.Length))
            {
                censored = censored.Replace(word, CreateCensoredString(word, censorCharacter));
            }

            return censored;
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

        public bool RemoveProfanity(string profanity)
        {
            if (string.IsNullOrEmpty(profanity))
            {
                throw new ArgumentNullException(nameof(profanity));
            }

            return _profanities.Remove(profanity.ToLower(CultureInfo.InvariantCulture));
        }

        public void Clear()
        {
            _profanities.Clear();
        }

        private List<string> FilterWordListByWhiteList(string[] words)
        {
            List<string> postWhiteList = new List<string>();
            foreach (string word in words)
            {
                if (!_whiteList.Contains(word.ToLower(CultureInfo.InvariantCulture)))
                {
                    postWhiteList.Add(word);
                }
            }

            return postWhiteList;
        }

        private static string ConvertWordListToSentence(List<string> postWhiteList)
        {
            // Reconstruct sentence excluding whitelisted words.
            string postWhiteListSentence = string.Empty;

            foreach (string w in postWhiteList)
            {
                postWhiteListSentence = postWhiteListSentence + w + " ";
            }

            return postWhiteListSentence;
        }

        private void AddMultiWordProfanities(List<string> swearList, string postWhiteListSentence)
        {
            swearList.AddRange(
                from string profanity in _profanities
                where postWhiteListSentence.Contains(profanity)
                select profanity);
        }

        private static string CreateCensoredString(string word, char censorCharacter)
        {
            string censoredWord = string.Empty;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != ' ')
                {
                    censoredWord += censorCharacter;
                }
                else
                {
                    censoredWord += ' ';
                }
            }

            return censoredWord;
        }
    }
}