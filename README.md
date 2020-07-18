![.NET Core](https://github.com/stephenhaunts/ProfanityDetector/workflows/.NET%20Core/badge.svg?branch=main)

# ProfanityDetector

This is a C# (.NET Standard 2.0) library for detecting profanities within a text string. The profanity list was compiled from lists from the internet that is allegedly used by social media sites for detecting profanities (although I can't confirm that). A library like this is useful if you want to detect anything profane in some text and have those words reported.

_The profanity list contains swearing, sexual acts, racial slurs, sexist slurs, and anything else that you can imagine. If you are easily offended, then **DO NOT** open the file called ProfanityList.cs_

In this readme I will cover the following:

* Using the Library via Nuget
* Basic Usage
* The Scunthorpe Problem
* Profanityh allow listing
* Adding and Removing Profanties 
* Replacing the Profanitiy List
* Frequently Asked Questions

# Release notes

0.1.7 : Contains a community contributed bug fix to a regular expression.

0.1.6 : Contains a community contribution to add a new method, ContainsProfanity, that will check for a profanity without the Scunthorpe checking.

0.1.5 : Update API to remove references to a white list and instead rename it to an allow list. If you install this newget package you will need to update the method class references for the white list. No functionality has changed.

0.1.4 : Fixed issues #5 and #6


# Using the Library via Nuget 

If you do not wish to download or clone this repository, then you can consume the profanity detector via [Nuget](https://www.nuget.org/packages/Profanity.Detector/).

To install via the package manager use the command (assuming version 0.1.7 of the library)

Install-Package Profanity.Detector -Version 0.1.7

Or via the command line

dotnet add package Profanity.Detector --version 0.1.7

# Example Usage

_In all the example code, to avoid using too much profane language, I have censored some of the words with an '@' symbol. All the example code is shown correctly, without censorship, in the [unit tests](https://github.com/stephenhaunts/ProfanityDetector/tree/master/ProfanityFilter.Tests.Unit)._

The following are some examples of the primary usage of the library. You first need to either download or clone the code from this repository and include it in your project or include the NuGet package [Profanity.Detector](https://www.nuget.org/packages/Profanity.Detector/)

## Check if a word is classed as a profanity

The simplest scenario is to check if a word exists in the profanity list. This is done with a call to IsProfanity, and this performs a case insensitive lookup in the profanity list.

```csharp
// Return true is a bad word
var filter = new ProfanityFilter();
Assert.IsTrue(filter.IsProfanity("@rsehole"));

// Return false if NOT a naughty word
var filter = new ProfanityFilter();
Assert.IsFalse(filter.IsProfanity("fluffy"));
```

## Return list of all profanities in a sentence

The second scenario is returning a list of profanities from the supplied string. This method will attempt to remove any false positives from the string. For example, to quote the standard Scunthorpe problem without removing the false positive, the word Scunthorpe would report the word "c@nt" as this is contained within the phrase Scunthorpe (characters 2 to 5). This library will detect if the profanity is inside another word and filter it out if the enclosed word is also not a profanity.

```csharp
var filter = new ProfanityFilter();
var swearList = filter.DetectAllProfanities("2 girls 1 cup is my favourite tw@tting video");
Assert.AreEqual(3, swearList.Count);
Assert.AreEqual("2 girls 1 cup", swearList[0]);
Assert.AreEqual("tw@tting", swearList[1]);
```

## Censoring a Sentence

The third scenario is to provide a string containing text that potentially has profane language, and censors the text by replacing naughty words with a designated character like an '*'.

```csharp
var filter = new ProfanityFilter();

var censored = filter.CensorString("Mary had a little sh@t lamb who was a little f@cker.");
var result = "Mary had a little **** lamb who was a little ******.";

Assert.AreEqual(censored, result);
```

# The Scunthorpe Problem

A common problem with the profanity detector is solving what is called the [Scunthorpe Problem](https://en.wikipedia.org/wiki/Scunthorpe_problem). This is where you get a false-positive result from a profanity detector because a profanity pattern is found inside a non-profane word. For example, with "Scunthorpe" (which is a town in the United Kingdom), it will get reported as containing the word "c@nt". What this profanity detector library will do is allow you to guard against this problem in two ways. The first is by using an allow list of words that are to be excluded from the profanity detector. This is covered in the next section.

The second solution is to be a bit more intelligent about how we check in the string. What this library will do, in the Scunthorpe example, is it will first detect the word "c*nt" in the line. Then the library will seek backward and forward in the string to identify if that profanity is enclosed within another word. If it is, that enclosed word is checked against the profanity list. If that word is not in the list, which Scunthorpe isn't, then the word is ignored. If that enclosed word is in the profanity list, then it will be reported as so.

# Allow listing

If there is a word in the profanity list that you don't consider a profanity, and you want to allow it through, you can add that word to an allow list. If that word appears in the input string, it will be ignored. In the example below, we have the sentence, "You are a complete twat and a total tit."). In this example, we want to say that the word "tit" is acceptable, so it gets added to the allow list, this means the only reported profanity for that sentence is the word "tw@t".

```csharp
var filter = new ProfanityFilter();
filter.AllowList.Add("tit");

var swearList = filter.DetectAllProfanities("You are a complete tw@t and a total tit.", true);

Assert.AreEqual(1, swearList.Count);
Assert.AreEqual("tw@t", swearList[0]);  
```

# Adding and Removing Profanties

There are a vast amount of words in the default list. The default list was put together from multiple lists online, so I, the author of this library, didn't physically write the list. If you feel that a word or words in the list are not what you consider to be profanity, you can remove them via code, like in the following example. In the example, we first check that "sh@t" is a profanity, and this returns true. Then we remove "sh@t" from the list and check if it is a profanity again. This time it returns true as we have removed it. 

```csharp
var filter = new ProfanityFilter();

Assert.IsTrue(filter.IsProfanity("sh@t"));
filter.RemoveProfanity("sh@t");

Assert.IsFalse(filter.IsProfanity("sh@t"));  
```

There may also be an occasion where there is a word you want to include to the list that is not on the default list. This can be quickly done as in the following example. In this example, we have deemed the word "fluffy" to be a profanity. We first check if it is a profanity, which returns false. Then we add "fluffy" to the list of profanities and check again, which will return true.

```csharp
var filter = new ProfanityFilter();
Assert.IsFalse(filter.IsProfanity("fluffy"));

filter.AddProfanity("fluffy");
Assert.IsTrue(filter.IsProfanity("fluffy")); 
```

You can also add an array of words to the list if you want to add them in one go. This is demonstrated by the following example. Here we are adding three new words to the list as an array.

```csharp
string[] _wordList =
{
"wibble",
"bibble",
"bobble"
};

var filter = new ProfanityFilter();
filter.AddProfanity(_wordList);
```

You can also directly add a List<string> instead of an array.
  
```csharp
string[] _wordList =
{
  "wibble",
  "bibble",
  "bobble"
};

var filter = new ProfanityFilter();
filter.AddProfanity(new List<string>(_wordList));
```

# Replacing the Profanitiy List

While developing this library, I had many people reach out to me to say that their companies maintain a signed off and curated a list of profanities.  Those companies have to check for these specific words and, therefore, can't use the default list build into this Profanity Detector. This is a great suggestion, so I have tweaked the library to allow completely overriding the default list and adding your own.

In this first example, we pass in an array of words into the ProfanityFilter constructor. This will stop the default list from being loaded and only insert these three words. This now means the profanity filter only contains three words, wibble, bibble, and bobble.

```csharp
string[] _wordList =
{
  "wibble",
  "bibble",
  "bobble"
};

IProfanityFilter filter = new ProfanityFilter(_wordList);
Assert.AreEqual(3, filter.Count);
```

You can also insert the new word list as a List<string>.
  
```csharp
string[] _wordList =
{
  "wibble",
  "bibble",
  "bobble"
};

IProfanityFilter filter = new ProfanityFilter(new List<string>(_wordList));
Assert.AreEqual(3, filter.Count);
```

Another way you can do this is to construct the ProfanityFilter with the default constructor that loads the default list, but then manually clear the list and insert your own array or List<string>.

```csharp
string[] _wordList =
{
  "wibble",
  "bibble",
  "bobble"
};

IProfanityFilter filter = new ProfanityFilter();
filter.Clear();
Assert.AreEqual(3, filter.Count);
```

# Frequently Asked Questions

**(Q)** Why does word (x) appear in the list, I don't consider it a profanity?

**(A)** The default list is compiled from lists I found on the internet that is allegedly used by some social media companies. On my first inspection of the list, I did remove some words that I thought were not profane (in my opinion). I may have missed some as the list is *HUGE*. It could also be that what is profane to one person, is not to another. 

If you spot something that you want to challenge, raise an issue, and I will take a look. In the meantime, if there is a word that you don't agree with being on the list, you can manually add it to the allow list, as demonstrated above, or insert your own list.


**(Q)** Why have a profanity filter in the first place? Freedom of speech should not include censorship.

**(A)** I also agree with freedom of speech and don't necessarily like censorship, except content to children, or hate speech.  In a lot of organizations, there are requirements to check for profanities in a user's input. If you are working in this type of environment, and a lot of companies do this, then you have to implement it; which is why this library exists.


**(Q)** My company has their own signed off list of profanities that needs to be censored on our system. Therefore I can't use the default list. Can I use my own?

**(A)** Of course, many people asked for this, so you can insert your own array/list of profanities by passing them into the ProfanityFilter constructor. See the example earlier in this readme file.


**(Q)** What is the user license to use the code for this library?

**(A)** The code in the Profanity Detector is released under a [Permissive MIT license](https://github.com/stephenhaunts/ProfanityDetector/blob/master/LICENSE). This means you can do what you like with the code. I am not charging for the code, and you are free to clone and modify the code as you wish. This also means I am not liable for any of this code, and it is provided as-is for you to use. While I am not responsible for the use of this code, if you do find an issue, please do raise a GitHub issue and I will take a look. Or you can fix it yourself and raise a pull request.

**(Q)** I am from Germany (or another country), do you support profanities in languages other than English?

**(A)** The current version of the profanity list only supports English profanities. If you have a list already in other languages, then you can load that list into the Profanity Detector. I would like to support multiple language profanities in the future, so if you know of any robust lists of these words in different languages, then please let me know.
