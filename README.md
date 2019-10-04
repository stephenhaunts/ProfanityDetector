# ProfanityDetector

This is a C# (.NET Standard 2.0) library for detecting profanities within a text string. The profanity list was compiled from lists on the compiled from the internet that are allegedly used by social media sites for detecting profanities. This is useful if you want to detect anything naught in somee text and have those words reported.

The profanity list contains swearing, sexual acts, ratial slurs, sexist slurs and anything else that you can imagine.

If you are easily offended, then DO NOT open the file called ProfanityList.cs

In this readme I will cover the following:

* Using the Library via Nuget
* Basic Usage
* The Scunthorpe Problem
* Whitelisting
* Adding and Removing Profanties
* Replacing the Profanitiy List


# Using the Library via Nuget

If you do not wish to download or clone this repository, then you can consume the profanity detector via [Nuget](https://www.nuget.org/packages/Profanity.Detector/).

To install via the package manager use the command (assuming version 0.1.0 of the library)

Install-Package Profanity.Detector -Version 0.1.0

Or via the command line

dotnet add package Profanity.Detector --version 0.1.0

# Example Usage

The following are some example of the basic usage of the library. You first need to either download or clone the code from this repository and include it in your project, or include the nuget package [Profanity.Detector](https://www.nuget.org/packages/Profanity.Detector/)

## Check if a word is classed as a profanity

The simplest scenario is to check if a word exists in the profanity list. This is done with a call to IsProfanity, and this performs a case insitive lookup in the profanity list.

```csharp
// Return true is a bad word
var filter = new ProfanityFilter();
Assert.IsTrue(filter.IsProfanity("arsehole"));

// Return false if NOT a naughty word
var filter = new ProfanityFilter();
Assert.IsFalse(filter.IsProfanity("fluffy"));
```

## Return list of all profanities in a sentence

The second scenario is returning a list of profanities from supplied string. This method will attempt remove any false positives from the string. For example, to quote the standard scunthope problem without removing the false positive, the word scunthorpe would report the word "c*nt" as this is contained within the word scunthorpe. This library will detect if a profanity is inside another word and filter it out if the enclosed word is also not a profanity.

```csharp
var filter = new ProfanityFilter();
var swearList = filter.DetectAllProfanities("2 girls 1 cup is my favourite twatting video");
Assert.AreEqual(3, swearList.Count);
Assert.AreEqual("2 girls 1 cup", swearList[0]);
Assert.AreEqual("twatting", swearList[1]);
```

## Censoring a Sentence

The third scenario is to provide a string containing text tha potentialy has profane language, and censor the text by replacing naughty words with a designated character like an '*'.

```csharp
var filter = new ProfanityFilter();

var censored = filter.CensorString("Mary had a little shit lamb who was a little fucker.");
var result = "Mary had a little **** lamb who was a little ******.";

Assert.AreEqual(censored, result);
```

# The Scunthorpe Problem

A common problem with profanity detector is solving what is called the [Scunthorpe Problem](https://en.wikipedia.org/wiki/Scunthorpe_problem).

# Whitelisting

TBC

# Adding and Removing Profanties

TBC

# Replacing the Profanitiy List

TBC
