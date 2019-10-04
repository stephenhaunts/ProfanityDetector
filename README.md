# ProfanityDetector

This is a C# (.NET Standard 2.0) library for detecting profanities within a text string. Thee profanith list was compiled from lists on the compiled from the internet that are allegedly used by social media sites for detecting profanities. This is useful if you want to detect anything naught in somee text and have those words reported.

The profanity list contains swearing, sexual acts, ratial slurs, sexist slurs and anything else that you can imagine.

If you are easily offended, then DO NOT open the file called ProfanityList.cs

Example Usage


## Check if a word is classed as a profanity

```csharp
// Return true is a bad word
var filter = new ProfanityFilter();
Assert.IsTrue(filter.IsProfanity("arsehole"));

// Return false if NOT a naughty word
var filter = new ProfanityFilter();
Assert.IsFalse(filter.IsProfanity("fluffy"));
```

## Return list of all profanities in a sentence

```csharp
var filter = new ProfanityFilter();
var swearList = filter.DetectAllProfanities("2 girls 1 cup is my favourite twatting video");
Assert.AreEqual(3, swearList.Count);
Assert.AreEqual("2 girls 1 cup", swearList[0]);
Assert.AreEqual("twatting", swearList[1]);
```
