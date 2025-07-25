# Array Search

[![Build and Publish](https://github.com/matthewratcliffe/arraysearcher/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/matthewratcliffe/arraysearcher/actions/workflows/nuget-publish.yml)
[![NuGet Version](https://img.shields.io/nuget/v/ArraySearch)](https://www.nuget.org/packages/ArraySearch)

## Overview

This project is designed to implement various string comparison and normalization algorithms. It includes functionalities such as phonetic encoding, similarity scoring, transliteration detection, and more. The primary focus is on handling names with different linguistic characteristics, including Arabic and Hispanic name patterns.

## Features

- **Phonetic Encoding:** Implementations of Soundex, Double Metaphone, and custom phonetic algorithms.
- **String Similarity Algorithms:** Includes Levenshtein Distance, Jaro-Winkler, and Damerau-Levenshtein similarity calculations.
- **Transliteration Detection:** Identifies and processes transliterations in names.
- **Normalization:** Handles vowel and consonant pattern normalization for accurate comparisons.
- **Specialized Name Handling:** Detects and scores similarities in Arabic and Hispanic names.

## Key Functions

- `DoSearch`: Searches through an array of strings using a specified search text.
- `GetSoundex` / `GetDoubleMetaphone`: Phonetic encoding functions.
- `ComputeLevenshteinDistance`: Computes the Levenshtein distance between two strings.
- `IsTransliterationEquivalent`: Checks if two words are equivalent under transliteration rules.
- `CalculateMatchScore`: Calculates a match score for a search term against a full name.
- `NormalizeArabicVowels` / `NormalizeSearchText`: Normalizes text for comparison.

## Technologies Used

- **Programming Language:** C# 13.0
- **Framework Targeting:** .NET 9.0

## Usage

### Using from NuGet

To use this project via NuGet, follow these steps:

1. Install the package via the NuGet Package Manager in your development environment or by using the following command in the Package Manager Console:

   ```powershell
   Install-Package ArraySearch -Version <version-number>
   ```

2. Add the namespace to your code:

   ```csharp
   using ArraySearch;
   ```

3. Instantiate and use the `Search` class as needed.

### Programmatic Usage

To integrate this project into your applications, reference the provided codebase and utilize its classes and methods for string comparison and normalization tasks.

## Usage Example
```csharp
string[] names = { "John Doe", "Jane Smith", "Johann Schmidt" };
string searchText = "John";
var result = search.DoSearch(names, searchText);
Console.WriteLine(result);
```
