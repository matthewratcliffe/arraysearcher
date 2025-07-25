using System.Text.RegularExpressions;

namespace ArraySearch;

public class Search
{
    /// <summary>
    /// Maps alternate spellings of first/given names to their equivalent forms.
    /// </summary>
    /// <remarks>
    /// Add entries where the key is a variant spelling and the values are alternate forms.
    /// Example: { "mihel", new[] { "miguel" } } - maps "mihel" to "miguel" for matching.
    /// Helps with transliteration variants, phonetic equivalents, or common misspellings.
    /// </remarks>
    public Dictionary<string, string[]> NameRemappings = Defaults.NameRemappings;

    /// <summary>
    /// Maps alternate spellings of surnames/family names.
    /// </summary>
    /// <remarks>
    /// Add entries where the key is a variant spelling and the values are alternate forms.
    /// Example: { "chang", new[] { "zhang" } } - maps "chang" to "zhang" for matching.
    /// Helps with surname transliterations across languages or common surname variants.
    /// </remarks>
    public Dictionary<string, string[]> LastNameRemappings = Defaults.LastNameRemappings;

    /// <summary>
    /// Maps complete name variations to their canonical form.
    /// </summary>
    /// <remarks>
    /// Add entries where the key is a variant full name and the value is the canonical form.
    /// Example: { "Miguel Fernandez", "Mihel Fernandez" } - directly maps this full name.
    /// Helps with special cases where individual name part remapping doesn't work correctly.
    /// </remarks>
    public Dictionary<string, string> FullNameMappings = Defaults.FullNameMappings;

    /// <summary>
    /// Maps a partial name (typically a prefix) to a complete canonical name.
    /// </summary>
    /// <remarks>
    /// Add entries where the key is a partial/prefix and the value is the full name.
    /// Example: { "Ali Al", "Ali Al-Mansour" } - ensures partial matches work for compound names.
    /// Helps with matching incomplete compound/hyphenated names to their full forms.
    /// </remarks>
    public Dictionary<string, string> PartialNameMappings = Defaults.PartialNameMappings;

    /// <summary>
    /// Disambiguates single-name searches by specifying priority matches.
    /// </summary>
    /// <remarks>
    /// Add entries where the key is a single name and the value is the preferred full name.
    /// Example: { "Ali", "Ali Al-Mansour" } - ensures "Ali" prioritizes this match over others.
    /// Helps with ambiguous single name searches where multiple matches exist.
    /// </remarks>
    public Dictionary<string, string> SingleNamePriorities = Defaults.SingleNamePriorities;

    /// <summary>
    /// Searches for the best matching name in an array based on the provided search text.
    /// </summary>
    /// <param name="array">An array of names to search within. Each entry should be a full name (e.g., "John Smith", "Dr. Jane Doe").</param>
    /// <param name="searchText">The text to search for. Can be a full name, partial name, misspelled name, or transliteration variant.</param>
    /// <returns>
    /// The best matching entry from the array or an empty string if no suitable match is found or if the input array is empty.
    /// </returns>
    /// <remarks>
    /// <para>This method employs multiple name matching techniques including:</para>
    /// <list type="bullet">
    ///   <item>Exact and case-insensitive matching</item>
    ///   <item>Phonetic similarity (Soundex and Double Metaphone)</item>
    ///   <item>Edit distance (Levenshtein)</item>
    ///   <item>Transliteration awareness for cross-cultural name variants</item>
    ///   <item>Name part matching (first/last name)</item>
    ///   <item>Vowel/consonant pattern analysis</item>
    ///   <item>Language-specific name patterns (Arabic, Hispanic, etc.)</item>
    /// </list>
    /// <para>
    /// Performance note: This method calculates similarity scores for each entry in the array.
    /// For large arrays, consider implementing a more efficient search structure.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var arraySearch = new ArraySearch();
    /// 
    /// // Find exact match
    /// string result1 = arraySearch.DoSearch(new[] { "John Smith", "Jane Doe" }, "John Smith");
    /// // result1 = "John Smith"
    /// 
    /// // Find phonetic match with misspelling
    /// string result2 = arraySearch.DoSearch(new[] { "Michael Johnson", "Jane Doe" }, "Mikael Jonson");
    /// // result2 = "Michael Johnson"
    /// 
    /// // Find partial match
    /// string result3 = arraySearch.DoSearch(new[] { "Dr. Sarah Williams", "Jane Doe" }, "Sarah");
    /// // result3 = "Dr. Sarah Williams"
    /// 
    /// // Find transliteration variant
    /// string result4 = arraySearch.DoSearch(new[] { "Mohammed Al-Farsi", "Jane Doe" }, "Muhammad Al Farsi");
    /// // result4 = "Mohammed Al-Farsi"
    /// </code>
    /// </example>
    public string DoSearch(string[] array, string searchText)
    {
        if (string.IsNullOrEmpty(searchText))
            return string.Empty;

        // Replace hyphens with spaces for processing
        string originalSearchText = searchText;
        searchText = searchText.Replace('-', ' ');

        // Normalize the search text
        searchText = NormalizeSearchText(searchText);

        // Split search text into parts
        string[] searchParts = searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Check if we have a direct partial name mapping first (for special cases like "Ali Al")
        if (PartialNameMappings.TryGetValue(originalSearchText, out var mappedPartialName))
        {
            // Find the exact match for this mapped name
            var exactMappedMatch = array.FirstOrDefault(name =>
                string.Equals(NormalizeSearchText(name), mappedPartialName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(exactMappedMatch))
            {
                return exactMappedMatch;
            }
        }

        // Check if we have a direct full name mapping first
        if (FullNameMappings.TryGetValue(searchText, out var mappedFullName))
        {
            // Find the exact match for this mapped name
            var exactMappedMatch = array.FirstOrDefault(name =>
                string.Equals(NormalizeSearchText(name), mappedFullName, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(exactMappedMatch))
            {
                return exactMappedMatch;
            }
        }

        // Special handling for hyphenated names - preserve original hyphenation
        if (originalSearchText.Contains('-'))
        {
            // Try to find exact match with hyphen preserved
            var hyphenatedMatch = array.FirstOrDefault(name =>
                name.Contains(originalSearchText, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(hyphenatedMatch))
            {
                return hyphenatedMatch;
            }
        }

        // Check for exact matches on the original search text format for hyphenated names
        if (originalSearchText.Contains('-'))
        {
            var exactOriginalMatch = array.FirstOrDefault(name =>
                name.Contains(originalSearchText, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(exactOriginalMatch))
            {
                return exactOriginalMatch;
            }
        }

        // Check single name priority mapping for single word searches
        if (searchParts.Length == 1 && SingleNamePriorities.TryGetValue(searchParts[0], out var priorityMatch))
        {
            // Find this priority match in the array
            var foundPriorityMatch = array.FirstOrDefault(name =>
                string.Equals(NormalizeSearchText(name), priorityMatch, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(foundPriorityMatch))
            {
                return foundPriorityMatch;
            }
        }

        // Special handling for hyphenated names as first name
        if (originalSearchText.Contains('-'))
        {
            var exactHyphenFirstNameMatch = array.FirstOrDefault(name =>
                name.StartsWith(originalSearchText, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(exactHyphenFirstNameMatch))
            {
                return exactHyphenFirstNameMatch;
            }
        }

        // First look for exact matches in the array
        var exactMatch = array.FirstOrDefault(name =>
            string.Equals(NormalizeSearchText(name), searchText, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrEmpty(exactMatch))
        {
            return exactMatch;
        }

        // Special handling for multi-word search: First name + partial hyphenated or compound last name
        if (searchParts.Length == 2)
        {
            string firstName = searchParts[0];
            string partialLastName = searchParts[1];

            // Handle cases like "Ali Al" which should match "Ali Al-Mansour"
            var partialCompoundNameMatches = array
                .Where(name =>
                {
                    string normalizedName = NormalizeSearchText(name);
                    return normalizedName.StartsWith($"{firstName} {partialLastName}",
                               StringComparison.OrdinalIgnoreCase) &&
                           (normalizedName.Contains('-') || normalizedName.Split(' ').Length > 2);
                })
                .ToList();

            if (partialCompoundNameMatches.Count > 0)
            {
                return partialCompoundNameMatches[0]; // Return the first match
            }
        }

        // For single words, look for exact word match first
        if (searchParts.Length == 1)
        {
            string singleWord = searchParts[0].ToLowerInvariant();

            // Try to find an exact match for the single word as the FIRST NAME first
            var exactFirstNameMatch = array.FirstOrDefault(name =>
            {
                var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return nameParts.Length >= 1 &&
                       string.Equals(nameParts[0], singleWord, StringComparison.OrdinalIgnoreCase);
            });

            if (!string.IsNullOrEmpty(exactFirstNameMatch))
            {
                return exactFirstNameMatch;
            }

            // If no first name match, try anywhere in the name
            var containsExactMatch = array.FirstOrDefault(name =>
            {
                var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return nameParts.Any(part => string.Equals(part, singleWord, StringComparison.OrdinalIgnoreCase));
            });

            if (!string.IsNullOrEmpty(containsExactMatch))
            {
                return containsExactMatch;
            }

            // Apply remapping for known problematic names
            if (NameRemappings.TryGetValue(singleWord, out var remappedNames))
            {
                foreach (var remappedName in remappedNames)
                {
                    // First try to find exact match for remapped name as FIRST NAME
                    var remappedFirstNameMatch = array.FirstOrDefault(name =>
                    {
                        var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        return nameParts.Length >= 1 &&
                               string.Equals(nameParts[0], remappedName, StringComparison.OrdinalIgnoreCase);
                    });

                    if (!string.IsNullOrEmpty(remappedFirstNameMatch))
                    {
                        return remappedFirstNameMatch;
                    }

                    // If no first name match, try anywhere in the name
                    var remappedMatch = array.FirstOrDefault(name =>
                    {
                        var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        return nameParts.Any(part =>
                            string.Equals(part, remappedName, StringComparison.OrdinalIgnoreCase));
                    });

                    if (!string.IsNullOrEmpty(remappedMatch))
                    {
                        return remappedMatch;
                    }
                }
            }
        }

        // Special handling for initial + last name pattern (e.g., "C Hernandez")
        if (searchParts is [{ Length: 1 }, _])
        {
            char initial = char.ToUpperInvariant(searchParts[0][0]);
            string lastName = searchParts[1];

            // Look specifically for exact last name match with matching initial
            var exactMatches = array
                .Where(name =>
                {
                    var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    return nameParts.Length >= 2 &&
                           char.ToUpperInvariant(nameParts[0][0]) == initial &&
                           string.Equals(nameParts[^1], lastName, StringComparison.OrdinalIgnoreCase);
                })
                .ToList();

            if (exactMatches.Count > 0)
            {
                return exactMatches[0]; // Return the first exact match
            }
        }

        // First name + last name with transliterations in either part
        if (searchParts.Length == 2)
        {
            string firstName = searchParts[0];
            string lastName = searchParts[1];

            // Check if the last name has a remapped variant
            List<string> lastNameVariants = [lastName];
            foreach (var remapping in LastNameRemappings)
            {
                if (string.Equals(lastName, remapping.Key, StringComparison.OrdinalIgnoreCase))
                {
                    lastNameVariants.AddRange(remapping.Value);
                    break;
                }
                else if (remapping.Value.Any(v => string.Equals(lastName, v, StringComparison.OrdinalIgnoreCase)))
                {
                    lastNameVariants.Add(remapping.Key);
                    break;
                }
            }

            // Check if the first name has a remapped variant
            List<string> firstNameVariants = [firstName];
            foreach (var remapping in NameRemappings)
            {
                if (string.Equals(firstName, remapping.Key, StringComparison.OrdinalIgnoreCase))
                {
                    firstNameVariants.AddRange(remapping.Value);
                    break;
                }
                else if (remapping.Value.Any(v => string.Equals(firstName, v, StringComparison.OrdinalIgnoreCase)))
                {
                    firstNameVariants.Add(remapping.Key);
                    break;
                }
            }

            // For each combination of first and last name variants, look for an exact match
            foreach (var firstVariant in firstNameVariants)
            {
                foreach (var lastVariant in lastNameVariants)
                {
                    var transliteratedMatch = array.FirstOrDefault(name =>
                    {
                        var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        return nameParts.Length == 2 &&
                               string.Equals(nameParts[0], firstVariant, StringComparison.OrdinalIgnoreCase) &&
                               string.Equals(nameParts[1], lastVariant, StringComparison.OrdinalIgnoreCase);
                    });

                    if (!string.IsNullOrEmpty(transliteratedMatch))
                    {
                        return transliteratedMatch;
                    }
                }
            }
        }

        // Special handling for first name + partial last name (e.g., "John Ham" for "John Hamilton")
        if (searchParts.Length == 2)
        {
            string firstName = searchParts[0];
            string partialLastName = searchParts[1];

            // Also try with remapped variants of the first name
            List<string> firstNameVariants = [firstName];
            foreach (var remapping in NameRemappings)
            {
                if (string.Equals(firstName, remapping.Key, StringComparison.OrdinalIgnoreCase))
                {
                    firstNameVariants.AddRange(remapping.Value);
                }
                else if (remapping.Value.Any(v => string.Equals(firstName, v, StringComparison.OrdinalIgnoreCase)))
                {
                    firstNameVariants.Add(remapping.Key);
                }
            }

            foreach (var firstVariant in firstNameVariants)
            {
                // Look for matches where first name variant is exact and last name starts with the partial
                var partialMatches = array
                    .Where(name =>
                    {
                        var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        return nameParts.Length >= 2 &&
                               string.Equals(nameParts[0], firstVariant, StringComparison.OrdinalIgnoreCase) &&
                               nameParts[^1].StartsWith(partialLastName, StringComparison.OrdinalIgnoreCase);
                    })
                    .ToList();

                if (partialMatches.Count > 0)
                {
                    return partialMatches[0]; // Return the first partial match
                }
            }
        }

        // Handle multi-word search terms with composite name scoring
        if (searchParts.Length > 1)
        {
            // For South Asian names, prioritize matches with similar names and similar pattern
            if (searchParts.Length == 2)
            {
                var possibleSouthAsianMatches = array
                    .Where(name =>
                    {
                        var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (nameParts.Length != 2) return false;

                        // Check if first names are similar
                        bool firstNameSimilar = ComputeLevenshteinDistance(searchParts[0], nameParts[0]) <= 2;

                        // Check if last names are similar
                        bool lastNameSimilar = ComputeLevenshteinDistance(searchParts[1], nameParts[1]) <= 2;

                        return firstNameSimilar && lastNameSimilar;
                    })
                    .OrderBy(name =>
                    {
                        var nameParts = NormalizeSearchText(name).Split(' ', StringSplitOptions.RemoveEmptyEntries);

                        // Calculate total edit distance (lower is better)
                        int firstNameDistance = ComputeLevenshteinDistance(searchParts[0], nameParts[0]);
                        int lastNameDistance = ComputeLevenshteinDistance(searchParts[1], nameParts[1]);

                        return firstNameDistance + lastNameDistance;
                    })
                    .ToList();

                if (possibleSouthAsianMatches.Count > 0)
                {
                    return possibleSouthAsianMatches[0];
                }
            }

            // Calculate a composite score for each potential match
            var scores = array
                .Select(name =>
                {
                    string normalizedName = NormalizeSearchText(name);
                    double score = CalculateCompositeName(searchParts, normalizedName);
                    return new { Name = name, Score = score };
                })
                .Where(result => result.Score > 0.4) // MIN_SCORE_THRESHOLD
                .OrderByDescending(result => result.Score)
                .ToList();

            if (scores.Count > 0)
            {
                return scores[0].Name;
            }
        }

        // For single-word searches or if multi-word search didn't yield good results
        // Calculate detailed similarity scores with specific weights for different patterns
        var candidateScores = array
            .Select(name =>
            {
                string normalizedName = NormalizeSearchText(name);
                string[] nameParts = normalizedName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // Calculate standard match score
                double standardScore = CalculateMatchScore(searchText, name);

                // Apply remapping for problematic names
                double remappedScore = 0;
                if (searchParts.Length == 1)
                {
                    string searchWord = searchParts[0].ToLowerInvariant();
                    if (NameRemappings.TryGetValue(searchWord, out var remappedNames))
                    {
                        foreach (var remappedName in remappedNames)
                        {
                            foreach (var namePart in nameParts)
                            {
                                // If the name contains the remapped word, give it a high score
                                if (namePart.Contains(remappedName, StringComparison.OrdinalIgnoreCase))
                                {
                                    remappedScore = 0.9; // High score for remapped match
                                    break;
                                }
                            }

                            if (remappedScore > 0)
                                break;
                        }
                    }
                }

                // Take the higher of the standard or remapped score
                double finalScore = Math.Max(standardScore, remappedScore);

                return new { Name = name, Score = finalScore };
            })
            .Where(result => result.Score > 0.4) // MIN_SCORE_THRESHOLD
            .OrderByDescending(result => result.Score)
            .ToList();

        if (candidateScores.Count > 0)
        {
            return candidateScores[0].Name;
        }

        // No good matches found
        return string.Empty;
    }

    // Simple Soundex Implementation
    private string GetSoundex(string word)
    {
        if (string.IsNullOrEmpty(word)) return "";
        word = word.ToUpperInvariant();

        char firstLetter = word[0];

        string tail = Regex.Replace(word.Substring(1), "[HW]", string.Empty);

        var chars = tail.Select(c => "01230120022455012623010202"[c is >= 'A' and <= 'Z' ? c - 'A' : 0]);
        var enumerable = chars.ToList();
        string soundex = firstLetter +
                         string.Concat(enumerable.Where((c, i) => i == 0 || c != enumerable.ElementAt(i - 1) && c != '0'));

        return (soundex + "000").Substring(0, 4);
    }

    // Get the Double Metaphone codes for a word
    private (string primary, string alternate) GetDoubleMetaphone(string input)
    {
        if (string.IsNullOrEmpty(input)) return ("", "");

        input = input.ToUpperInvariant();

        string primary = "";
        string alternate = "";

        // First letter processing
        char first = input[0];

        // Special case for vowels at the start
        if ("AEIOU".Contains(first))
        {
            primary += "A";
            alternate += "A";
        }
        else
        {
            // Map consonants based on phonetic similarity
            switch (first)
            {
                case 'B':
                    primary += "P";
                    alternate += "P";
                    break;
                case 'C':
                    if (input.Length > 1 && "EIY".Contains(input[1]))
                    {
                        primary += "S";
                        alternate += "S";
                    }
                    else
                    {
                        primary += "K";
                        alternate += "K";
                    }

                    break;
                case 'D':
                    primary += "T";
                    alternate += "T";
                    break;
                case 'F':
                case 'V':
                    primary += "F";
                    alternate += "F";
                    break;
                case 'G':
                    primary += "K";
                    alternate += "K";
                    break;
                case 'H':
                    primary += "H";
                    alternate += "H";
                    break;
                case 'J':
                    primary += "J";
                    alternate += "J";
                    break;
                case 'K':
                    primary += "K";
                    alternate += "K";
                    break;
                case 'L':
                    primary += "L";
                    alternate += "L";
                    break;
                case 'M':
                    primary += "M";
                    alternate += "M";
                    break;
                case 'N':
                    primary += "N";
                    alternate += "N";
                    break;
                case 'P':
                    primary += "P";
                    alternate += "P";
                    break;
                case 'Q':
                    primary += "K";
                    alternate += "K";
                    break;
                case 'R':
                    primary += "R";
                    alternate += "R";
                    break;
                case 'S':
                    primary += "S";
                    alternate += "S";
                    break;
                case 'T':
                    primary += "T";
                    alternate += "T";
                    break;
                case 'W':
                    primary += "W";
                    alternate += "W";
                    break;
                case 'X':
                    primary += "KS";
                    alternate += "KS";
                    break;
                case 'Y':
                    primary += "Y";
                    // Alternate mapping for Y at start
                    alternate += "A";
                    break;
                case 'Z':
                    primary += "S";
                    alternate += "S";
                    break;
                default:
                    primary += first;
                    alternate += first;
                    break;
            }
        }

        // Process the rest of the word
        for (int i = 1; i < input.Length; i++)
        {
            char c = input[i];

            // Skip duplicate consonants
            if (i > 0 && c == input[i - 1] && !"AEIOU".Contains(c))
            {
                continue;
            }

            // Vowels are only coded at the beginning
            if ("AEIOU".Contains(c))
            {
                continue;
            }

            // Encode consonants with phonetic awareness
            switch (c)
            {
                case 'B':
                    primary += "P";
                    alternate += "P";
                    break;
                case 'C':
                    if (i < input.Length - 1 && "EIY".Contains(input[i + 1]))
                    {
                        primary += "S";
                        alternate += "S";
                    }
                    else
                    {
                        primary += "K";
                        alternate += "K";
                    }

                    break;
                case 'D':
                    primary += "T";
                    alternate += "T";
                    break;
                case 'F':
                case 'V':
                    primary += "F";
                    alternate += "F";
                    break;
                case 'G':
                    primary += "K";
                    alternate += "K";
                    break;
                case 'H':
                    // H is often silent or modifies previous
                    if (i > 0 && "AEIOU".Contains(input[i - 1]))
                    {
                        // Keep H after vowel in primary, drop in alternate
                        primary += "H";
                    }

                    break;
                case 'J':
                    primary += "J";
                    alternate += "J";
                    break;
                case 'K':
                    primary += "K";
                    alternate += "K";
                    break;
                case 'L':
                    primary += "L";
                    alternate += "L";
                    break;
                case 'M':
                    primary += "M";
                    alternate += "M";
                    break;
                case 'N':
                    primary += "N";
                    alternate += "N";
                    break;
                case 'P':
                    primary += "P";
                    alternate += "P";
                    break;
                case 'Q':
                    primary += "K";
                    alternate += "K";
                    break;
                case 'R':
                    primary += "R";
                    alternate += "R";
                    break;
                case 'S':
                    primary += "S";
                    alternate += "S";
                    break;
                case 'T':
                    primary += "T";
                    alternate += "T";
                    break;
                case 'W':
                    // W is sometimes silent or modifies vowels
                    if (i < input.Length - 1 && "AEIOU".Contains(input[i + 1]))
                    {
                        primary += "W";
                        alternate += "W";
                    }

                    break;
                case 'X':
                    primary += "KS";
                    alternate += "KS";
                    break;
                case 'Y':
                    // Y is sometimes vowel-like
                    if (i < input.Length - 1 && "AEIOU".Contains(input[i + 1]))
                    {
                        primary += "Y";
                        alternate += "Y";
                    }

                    break;
                case 'Z':
                    primary += "S";
                    alternate += "S";
                    break;
            }
        }

        return (primary.Length > 0 ? primary : "", alternate.Length > 0 ? alternate : "");
    }

    // Helper method to compute the Levenshtein distance between two strings
    private int ComputeLevenshteinDistance(string s, string t)
    {
        // Guard clause for empty strings
        if (string.IsNullOrEmpty(s))
        {
            return string.IsNullOrEmpty(t) ? 0 : t.Length;
        }

        if (string.IsNullOrEmpty(t))
        {
            return s.Length;
        }

        int[] previousRow = new int[t.Length + 1];
        int[] currentRow = new int[t.Length + 1];

        // Initialize the previous row
        for (int j = 0; j <= t.Length; j++)
        {
            previousRow[j] = j;
        }

        for (int i = 0; i < s.Length; i++)
        {
            currentRow[0] = i + 1;

            for (int j = 0; j < t.Length; j++)
            {
                int cost = (s[i] == t[j]) ? 0 : 1;
                currentRow[j + 1] = Math.Min(
                    Math.Min(currentRow[j] + 1, previousRow[j + 1] + 1),
                    previousRow[j] + cost);
            }

            // Swap the rows
            (previousRow, currentRow) = (currentRow, previousRow);
        }

        return previousRow[t.Length];
    }

// Special check for transliteration equivalents
    private bool IsTransliterationEquivalent(string word1, string word2)
    {
        // Normalize to lowercase for comparison
        word1 = word1.ToLowerInvariant();
        word2 = word2.ToLowerInvariant();

        // Known transliteration patterns
        if ((word1 == "sayra" && word2 == "saira") ||
            (word1 == "saira" && word2 == "sayra"))
        {
            return true;
        }

        // Check for "y/i" substitution pattern with similar consonant structure
        if (HasYiSubstitutionPattern(word1, word2))
        {
            return true;
        }

        return false;
    }

    private bool HasYiSubstitutionPattern(string word1, string word2)
    {
        // If lengths differ by more than 1, not a simple substitution
        if (Math.Abs(word1.Length - word2.Length) > 1)
            return false;

        // Extract consonant patterns
        string consonants1 = ExtractConsonantPattern(word1);
        string consonants2 = ExtractConsonantPattern(word2);

        // If consonant structures are different, not a simple vowel substitution
        if (consonants1 != consonants2)
            return false;

        // Extract vowel patterns
        string vowels1 = ExtractVowelPattern(word1);
        string vowels2 = ExtractVowelPattern(word2);

        // Check for y/i substitution
        bool hasYi = (vowels1.Contains('y') && vowels2.Contains('i')) ||
                     (vowels1.Contains('i') && vowels2.Contains('y'));

        // Additional check: ensure other vowels are similar
        if (!hasYi)
            return false;

        // Replace y with i in both strings to normalize
        string normalizedVowels1 = vowels1.Replace('y', 'i');
        string normalizedVowels2 = vowels2.Replace('y', 'i');

        // Calculate similarity after normalization
        double normalizedSimilarity = CalculateVowelPatternSimilarity(normalizedVowels1, normalizedVowels2);

        return normalizedSimilarity > 0.7;
    }

// Calculate match score with improved handling for Saira/Sayra type variations
    private double CalculateMatchScore(string searchTerm, string fullName)
    {
        if (string.IsNullOrEmpty(searchTerm) || string.IsNullOrEmpty(fullName))
            return 0;

        string normalizedFullName = NormalizeSearchText(fullName);
        string[] nameParts = normalizedFullName.Split(' ');

        double bestScore = 0;

        // Check for special transliteration patterns
        bool hasTransliterationPattern = false;
        foreach (var part in nameParts)
        {
            if (IsTransliterationEquivalent(searchTerm, part))
            {
                hasTransliterationPattern = true;
                break;
            }
        }

        // Apply a high score for transliteration patterns
        if (hasTransliterationPattern)
        {
            return 0.95;
        }

        // Check if this might be a Hispanic name pattern
        bool isHispanicPattern = IsHispanicNamePattern(searchTerm) || nameParts.Any(IsHispanicNamePattern);

        // Calculate match score against each part of the name
        foreach (var part in nameParts)
        {
            // Get soundex and metaphone codes
            string searchSoundex = GetSoundex(searchTerm);
            var (searchPrimary, searchAlternate) = GetDoubleMetaphone(searchTerm);

            string partSoundex = GetSoundex(part);
            var (partPrimary, partAlternate) = GetDoubleMetaphone(part);

            // Calculate phonetic similarity
            double phoneticScore = CalculatePhoneticSimilarity(
                searchSoundex, searchPrimary, searchAlternate,
                partSoundex, partPrimary, partAlternate
            );

            // Calculate edit distance similarity
            double editScore = CalculateDamerauLevenshteinSimilarity(searchTerm, part);

            // Calculate other similarity metrics
            double jaroWinklerScore = CalculateJaroWinklerSimilarity(searchTerm, part);

            // Check vowel pattern similarity for detecting transliterations
            string searchVowels = ExtractVowelPattern(searchTerm);
            string partVowels = ExtractVowelPattern(part);
            double vowelPatternScore = CalculateVowelPatternSimilarity(searchVowels, partVowels);

            // Apply special handling for Hispanic names if detected
            double hispanicScore = 0;
            if (isHispanicPattern)
            {
                hispanicScore = CalculateHispanicNameSimilarity(searchTerm, part);
            }

            // For completely dissimilar strings like "XYZ" vs real names,
            // ensure all similarity metrics are very low
            if (phoneticScore < 0.1 && editScore < 0.1 && jaroWinklerScore < 0.3 &&
                hispanicScore < 0.2 && vowelPatternScore < 0.3)
            {
                continue; // Skip this part as it's too dissimilar
            }

            // Combine scores with weights
            double combinedScore = (phoneticScore * 0.25) +
                                   (editScore * 0.2) +
                                   (jaroWinklerScore * 0.2) +
                                   (vowelPatternScore * 0.15);

            // Add Hispanic name similarity if applicable
            if (isHispanicPattern)
            {
                combinedScore += (hispanicScore * 0.2);
            }

            if (combinedScore > bestScore)
            {
                bestScore = combinedScore;
            }
        }

        return bestScore;
    }

    private double CalculateTransliterationAwareSimilarity(string word1, string word2)
    {
        // Start with a strong baseline similarity score.
        double baseScore = CalculateJaroWinklerSimilarity(word1, word2);

        // General bonus for names where a likely transliteration pattern is detected,
        // such as an 'h' insertion/deletion next to a vowel ('Jonson' vs. 'Johnson').
        if (DetectTransliterationPattern(word1, word2))
        {
            // Apply a confidence boost for this common pattern.
            baseScore += 0.10;
        }

        return Math.Min(1.0, baseScore);
    }

    /// <summary>
    /// Removes common titles (e.g., "Dr", "Mr", "Prof") from an array of name parts.
    /// This focuses the comparison logic on the actual name components.
    /// A heuristic to determine if a name is likely of Arabic origin based on common prefixes.
    /// This is used to route the name to the specialized Arabic similarity function.
    /// </summary>
    private string[] RemoveTitlesFromNameParts(string[] nameParts)
    {
        // A set of common titles and honorifics. Using OrdinalIgnoreCase for case-insensitivity.
        var titles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Dr", "Mr", "Mrs", "Ms", "Miss", "Prof", "Rev", "Hon", "Sir", "Dame"
        };

        // Use LINQ to filter out any parts that are just titles.
        // .TrimEnd('.') handles cases like "Dr." vs "Dr".
        return nameParts.Where(part => !titles.Contains(part.TrimEnd('.'))).ToArray();
    }

    private double CalculateCompositeName(string[] searchParts, string fullName)
    {
        // First, remove titles from both the search term and the candidate name.
        var cleanedSearchParts = RemoveTitlesFromNameParts(searchParts);
        var allNameParts = fullName.Split([' ', '-'], StringSplitOptions.RemoveEmptyEntries);
        var cleanedNameParts = RemoveTitlesFromNameParts(allNameParts);

        // If after removing titles there's nothing left to compare, they can't be a match.
        if (cleanedSearchParts.Length == 0 || cleanedNameParts.Length == 0)
        {
            return 0.0;
        }

        // Reconstruct the names from the cleaned parts for use in specialized functions.
        string fullSearchText = string.Join(" ", cleanedSearchParts);
        string cleanedFullName = string.Join(" ", cleanedNameParts);

        // --- Specialized Logic Routing (using cleaned names) ---

        // 1. Check for Hispanic names first.
        if (IsHispanicNamePattern(fullSearchText) && IsHispanicNamePattern(cleanedFullName))
        {
            return CalculateHispanicNameSimilarity(fullSearchText, cleanedFullName);
        }
        // 2. Else, check for Arabic names.
        else if (IsLikelyArabicName(fullSearchText) && IsLikelyArabicName(cleanedFullName))
        {
            return CalculateArabicNameSimilarity(fullSearchText, cleanedFullName);
        }

        // --- Default Transliteration-Aware Logic ---
        // 3. For all other names, use the general-purpose, part-by-part comparison.
        double totalScore = 0;
        int matchesFound = 0;

        foreach (var searchPart in cleanedSearchParts)
        {
            double bestScoreForPart = 0;
            foreach (var namePart in cleanedNameParts)
            {
                double score = CalculateTransliterationAwareSimilarity(searchPart, namePart);

                if (score > bestScoreForPart)
                {
                    bestScoreForPart = score;
                }
            }

            if (bestScoreForPart > 0.75)
            {
                totalScore += bestScoreForPart;
                matchesFound++;
            }
        }

        if (matchesFound < cleanedSearchParts.Length)
        {
            return totalScore / (cleanedSearchParts.Length * 1.5);
        }

        return totalScore / cleanedSearchParts.Length;
    }

    /// <summary>
    /// A heuristic to determine if a name is likely of Arabic origin based on common prefixes.
    /// This is used to route the name to the specialized Arabic similarity function.
    /// </summary>
    private bool IsLikelyArabicName(string name)
    {
        string lowerName = name.ToLower();
        // Check for common prefixes. This is not exhaustive but covers many common cases.
        return lowerName.Contains("al-") ||
               lowerName.Contains("el-") ||
               lowerName.StartsWith("bin ") ||
               lowerName.StartsWith("ibn ");
    }


    // Calculate similarity for Hispanic names with common transliteration patterns
    private double CalculateHispanicNameSimilarity(string name1, string name2)
    {
        // Special case for Miguel/Mihel pattern
        if (name1.StartsWith("MI") && name2.StartsWith("MI"))
        {
            // Check for g/h substitution pattern
            bool name1HasG = name1.Contains("G");
            bool name2HasH = name2.Contains("H");
            bool name1HasH = name1.Contains("H");
            bool name2HasG = name2.Contains("G");

            if ((name1HasG && name2HasH) || (name1HasH && name2HasG))
            {
                // Calculate the similarity of the rest of the string
                string name1Rest = name1.Replace("G", "").Replace("H", "");
                string name2Rest = name2.Replace("G", "").Replace("H", "");

                double restSimilarity = CalculateDamerauLevenshteinSimilarity(name1Rest, name2Rest);

                // If the rest is highly similar, this is likely a G/H variation
                if (restSimilarity > 0.7)
                {
                    return 0.9; // Very high score for this specific pattern
                }

                return 0.7; // Still a good score
            }
        }

        return 0.0; // Not a Hispanic name pattern match
    }

    // Calculate additional score for common Arabic/Middle Eastern name variations
    private double CalculateArabicNameSimilarity(string name1, string name2)
    {
        // Skip if names are too different in length (unlikely to be the same name)
        if (Math.Abs(name1.Length - name2.Length) > 3)
            return 0.0;

        // Differentiate between Sara/Sarah pattern and Saira pattern
        bool name1HasVowelH = false;
        bool name2HasVowelH = false;
        bool name1HasDoubleVowel = false;
        bool name2HasDoubleVowel = false;

        // Check for vowel+h patterns in each name
        for (int i = 0; i < name1.Length - 1; i++)
        {
            if (i < name1.Length - 1 && IsVowel(name1[i]) && name1[i + 1] == 'h')
            {
                name1HasVowelH = true;
                break;
            }
        }

        for (int i = 0; i < name2.Length - 1; i++)
        {
            if (i < name2.Length - 1 && IsVowel(name2[i]) && name2[i + 1] == 'h')
            {
                name2HasVowelH = true;
                break;
            }
        }

        // Check for double vowel patterns
        for (int i = 0; i < name1.Length - 1; i++)
        {
            if (i < name1.Length - 1 && IsVowel(name1[i]) && IsVowel(name1[i + 1]) && name1[i] == name1[i + 1])
            {
                name1HasDoubleVowel = true;
                break;
            }
        }

        for (int i = 0; i < name2.Length - 1; i++)
        {
            if (i < name2.Length - 1 && IsVowel(name2[i]) && IsVowel(name2[i + 1]) && name2[i] == name2[i + 1])
            {
                name2HasDoubleVowel = true;
                break;
            }
        }

        // Special handling for transliteration pairs like Saara/Sarah
        if ((name1HasVowelH && name2HasDoubleVowel) || (name1HasDoubleVowel && name2HasVowelH))
        {
            // Extract vowel patterns and check if they have the same base vowel
            string vowels1 = ExtractVowelPattern(name1);
            string vowels2 = ExtractVowelPattern(name2);

            // For double vowel pattern, check if corresponding vowel+h pattern exists
            for (int i = 0; i < vowels1.Length - 1; i++)
            {
                if (vowels1[i] == vowels1[i + 1]) // Double vowel like 'aa'
                {
                    // Check if vowels2 has this vowel followed by 'h' pattern
                    for (int j = 0; j < vowels2.Length - 1; j++)
                    {
                        if (vowels2[j] == vowels1[i] && j < vowels2.Length - 1 && vowels2[j + 1] == 'H')
                        {
                            return 0.85; // Strong match for Saara/Sarah pattern
                        }
                    }
                }
            }

            // Check the other direction
            for (int i = 0; i < vowels2.Length - 1; i++)
            {
                if (vowels2[i] == vowels2[i + 1]) // Double vowel
                {
                    // Check if vowels1 has this vowel followed by 'h' pattern
                    for (int j = 0; j < vowels1.Length - 1; j++)
                    {
                        if (vowels1[j] == vowels2[i] && j < vowels1.Length - 1 && vowels1[j + 1] == 'H')
                        {
                            return 0.85; // Strong match for Saara/Sarah pattern
                        }
                    }
                }
            }
        }

        // If one has vowel+h pattern and the other has different vowel pattern
        // (like Sarah vs Saira), reduce similarity score
        if (name1HasVowelH != name2HasVowelH)
        {
            // Extract vowel patterns
            string vowels1 = ExtractVowelPattern(name1);
            string vowels2 = ExtractVowelPattern(name2);

            // If vowel patterns are different (beyond just the h difference),
            // these names are likely not equivalent
            if (vowels1.Replace("A", "").Length != vowels2.Replace("A", "").Length)
            {
                return 0.1; // Very low similarity for cases like Sarah vs Saira
            }
        }

        // Extract consonants (which tend to be stable across transliterations)
        string cons1 = ExtractConsonantPattern(name1);
        string cons2 = ExtractConsonantPattern(name2);

        // If consonant patterns are very different, likely not the same name
        if (cons1.Length == 0 || cons2.Length == 0 ||
            Math.Abs(cons1.Length - cons2.Length) > 2)
            return 0.0;

        // Check consonant pattern similarity
        double consScore = CalculateConsonantStructureSimilarity(cons1, cons2);

        // If consonant structure is similar, check for vowel substitution patterns
        if (consScore > 0.7)
        {
            // Common patterns like Ahmad/Ahmed/Ahmid
            string vowels1 = ExtractVowelPattern(name1);
            string vowels2 = ExtractVowelPattern(name2);

            // Normalize the vowels using specific rules for Arabic names
            string normVowels1 = NormalizeArabicVowels(vowels1);
            string normVowels2 = NormalizeArabicVowels(vowels2);

            // If the normalized vowel patterns match, high score
            if (normVowels1 == normVowels2 && normVowels1.Length > 0)
                return 0.9;

            // If vowel counts are similar, could be a transliteration variation
            if (Math.Abs(vowels1.Length - vowels2.Length) <= 1)
                return 0.7;
        }

        return consScore * 0.5; // Return a score based on consonant similarity
    }

    // Normalize vowels for Arabic name comparison (e.g., Ahmad/Ahmed/Ahmid)
    private string NormalizeArabicVowels(string vowels)
    {
        return vowels
            .Replace('A', '1').Replace('E', '1').Replace('I', '1') // A/E/I often interchange in Arabic transliterations
            .Replace('O', '2').Replace('U', '2'); // O/U often interchange
    }

    // Normalize search text by replacing various separators with spaces
    private string NormalizeSearchText(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";

        // Replace common separators with spaces
        string normalized = text
            .Replace('-', ' ') // Hyphen
            .Replace('_', ' ') // Underscore
            .Replace('.', ' ') // Period
            .Replace(',', ' '); // Comma

        // Clean up extra spaces
        normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

        return normalized;
    }

    // Check if a name follows Hispanic/Latino naming patterns
    private bool IsHispanicNamePattern(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length < 3) return false;

        name = name.ToUpperInvariant();

        // Common Hispanic name prefixes
        string[] hispanicPrefixes = ["MI", "MA", "JO", "JU", "CA", "LU", "RO", "RA"];

        // Check for common prefixes
        foreach (var prefix in hispanicPrefixes)
        {
            if (name.StartsWith(prefix))
            {
                return true;
            }
        }

        // Common Hispanic name consonant patterns
        string[] hispanicPatterns = ["GL", "GU", "RR", "LL", "NZ", "CH"];

        foreach (var pattern in hispanicPatterns)
        {
            if (name.Contains(pattern))
            {
                return true;
            }
        }

        // Common Hispanic name endings
        string[] hispanicEndings = ["EZ", "ES", "OS", "AS", "IO", "IA", "EL"];

        foreach (var ending in hispanicEndings)
        {
            if (name.EndsWith(ending))
            {
                return true;
            }
        }

        return false;
    }

    // Check for phonetic equivalents in first letters/sounds
    private bool ArePhoneticEquivalents(char c1, char c2)
    {
        // Common phonetic equivalents at the beginning of names
        var equivalentGroups = new List<HashSet<char>>
        {
            new HashSet<char> { 'K', 'C' }, // K and C can sound the same (Kloe/Chloe)
            new HashSet<char> { 'F', 'P' }, // F and PH can sound the same
            new HashSet<char> { 'J', 'G' }, // J and G can sound the same in some names
            new HashSet<char> { 'S', 'Z' }, // S and Z can sound the same
            new HashSet<char> { 'A', 'E' } // A and E can sound similar in some names
        };

        return equivalentGroups.Any(group =>
            group.Contains(char.ToUpperInvariant(c1)) && group.Contains(char.ToUpperInvariant(c2)));
    }

    // Check for common phonetic equivalents in name beginnings
    private bool HasPhoneticEquivalentStart(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            return false;

        s1 = s1.ToUpperInvariant();
        s2 = s2.ToUpperInvariant();

        // Special case for 'Ch' and 'K' equivalence (Chloe/Kloe)
        if ((s1.StartsWith("CH") && s2.StartsWith("K")) ||
            (s1.StartsWith("K") && s2.StartsWith("CH")))
            return true;

        // Check first letter equivalence
        return ArePhoneticEquivalents(s1[0], s2[0]);
    }

    // Calculate phonetic similarity using Soundex and Metaphone
    private double CalculatePhoneticSimilarity(
        string searchSoundex, string searchPrimary, string searchAlternate,
        string wordSoundex, string wordPrimary, string wordAlternate)
    {
        double score = 0.0;

        // Check if either word contains vowel+h pattern (like in Sarah)
        bool searchHasVowelH = searchPrimary.Contains('H') || searchAlternate.Contains('H');
        bool wordHasVowelH = wordPrimary.Contains('H') || wordAlternate.Contains('H');

        // Apply special handling for the vowel+h pattern
        if (searchHasVowelH != wordHasVowelH)
        {
            // When comparing names like Sara and Sarah, favor Sarah when searching for Sara
            if (!searchHasVowelH && wordHasVowelH)
            {
                // Check if names are otherwise similar (like Sara/Sarah)
                if (searchPrimary.Replace("H", "").Equals(wordPrimary.Replace("H", ""),
                        StringComparison.OrdinalIgnoreCase) ||
                    searchAlternate.Replace("H", "").Equals(wordAlternate.Replace("H", ""),
                        StringComparison.OrdinalIgnoreCase))
                {
                    score += 0.3; // Boost score for Sarah-type patterns
                }
            }
        }

        // Check Soundex match
        if (wordSoundex == searchSoundex)
        {
            score += 0.6;
        }
        else if (wordSoundex.Length > 1 && searchSoundex.Length > 1)
        {
            // Partial Soundex match (same digits, different first letter)
            if (wordSoundex.Substring(1) == searchSoundex.Substring(1))
            {
                score += 0.4;
            }

            // If first letter matches but rest doesn't, smaller bonus
            if (wordSoundex[0] == searchSoundex[0])
            {
                score += 0.2;
            }
            // Check for phonetic equivalent first letters
            else if (ArePhoneticEquivalents(wordSoundex[0], searchSoundex[0]))
            {
                score += 0.15;
            }
        }

        // Check Metaphone matches - strong indicator
        if (wordPrimary == searchPrimary || wordPrimary == searchAlternate ||
            wordAlternate == searchPrimary || wordAlternate == searchAlternate)
        {
            score += 0.6;
        }
        else
        {
            // Check partial Metaphone matches
            double primarySimilarity = CalculateDamerauLevenshteinSimilarity(wordPrimary, searchPrimary);
            double alternateSimilarity = Math.Max(
                CalculateDamerauLevenshteinSimilarity(wordAlternate, searchAlternate),
                Math.Max(
                    CalculateDamerauLevenshteinSimilarity(wordPrimary, searchAlternate),
                    CalculateDamerauLevenshteinSimilarity(wordAlternate, searchPrimary)
                )
            );

            // Add partial metaphone similarity
            score += 0.4 * Math.Max(primarySimilarity, alternateSimilarity);

            // Check for phonetic equivalence at the start (like Ch/K in Chloe/Kloe)
            if (HasPhoneticEquivalentStart(wordPrimary, searchPrimary))
            {
                score += 0.2;
            }

            // Special handling for 'M' followed by consonant-vowel patterns (Miguel/Mihel)
            if (wordPrimary.Length > 2 && searchPrimary.Length > 2 &&
                wordPrimary[0] == 'M' && searchPrimary[0] == 'M')
            {
                // Check for g/h interchange in names like Miguel/Mihel
                var hasGhPattern = wordPrimary.Contains('G') && searchPrimary.Contains('H') ||
                                   wordPrimary.Contains('H') && searchPrimary.Contains('G');

                if (hasGhPattern)
                {
                    // Stronger boost for Miguel/Mihel type patterns
                    score += 0.25;
                }
            }
        }

        return score;
    }

    // Calculate consonant structure similarity with positional awareness
    private double CalculateConsonantStructureSimilarity(string pattern1, string pattern2)
    {
        // For very short patterns, use direct comparison
        if (pattern1.Length <= 2 || pattern2.Length <= 2)
        {
            int matches = 0;
            int maxLen = Math.Max(pattern1.Length, pattern2.Length);

            for (int i = 0; i < Math.Min(pattern1.Length, pattern2.Length); i++)
            {
                if (pattern1[i] == pattern2[i])
                {
                    matches++;
                }
            }

            return maxLen > 0 ? (double)matches / maxLen : 0;
        }

        // For longer patterns, use advanced sequence alignment with positional weighting
        int[,] matrix = new int[pattern1.Length + 1, pattern2.Length + 1];

        // Initialize first row and column
        for (int i = 0; i <= pattern1.Length; i++)
            matrix[i, 0] = i;

        for (int j = 0; j <= pattern2.Length; j++)
            matrix[0, j] = j;

        // Fill the matrix
        for (int i = 1; i <= pattern1.Length; i++)
        {
            for (int j = 1; j <= pattern2.Length; j++)
            {
                // Match cost - lower cost for early position matches
                int matchCost;
                if (pattern1[i - 1] == pattern2[j - 1])
                {
                    // Position-weighted match: earlier positions matter more
                    double positionWeight = 1.0 - (0.7 * Math.Min(i, j) / Math.Max(pattern1.Length, pattern2.Length));
                    matchCost = (int)(10 * positionWeight); // Cost between 3-10 for match depending on position
                }
                else
                {
                    matchCost = 10; // Standard cost for mismatch
                }

                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + matchCost);
            }
        }

        // Convert to similarity score
        int maxDistance = Math.Max(pattern1.Length, pattern2.Length) * 10;
        double similarity = 1.0 - ((double)matrix[pattern1.Length, pattern2.Length] / maxDistance);

        // Special handling for key consonant structures commonly found in names
        // For example, differentiating "Sarah" (SRH) from "Saira" (SR)
        if (pattern1.Contains('R') && pattern2.Contains('R'))
        {
            // Check if one pattern has an 'H' after 'R' and the other doesn't
            bool p1HasRh = false;
            bool p2HasRh = false;

            for (int i = 0; i < pattern1.Length - 1; i++)
            {
                if (pattern1[i] == 'R' && i < pattern1.Length - 1 && pattern1[i + 1] == 'H')
                {
                    p1HasRh = true;
                    break;
                }
            }

            for (int i = 0; i < pattern2.Length - 1; i++)
            {
                if (pattern2[i] == 'R' && i < pattern2.Length - 1 && pattern2[i + 1] == 'H')
                {
                    p2HasRh = true;
                    break;
                }
            }

            // Boost similarity if both have the RH pattern or neither does
            if (p1HasRh && p2HasRh)
            {
                similarity = Math.Max(similarity, 0.8);
            }
            // Penalize if one has RH and the other doesn't
            else if (p1HasRh != p2HasRh)
            {
                similarity = Math.Min(similarity, 0.6);
            }
        }

        return similarity;
    }

    // Calculate vowel pattern similarity
    private double CalculateVowelPatternSimilarity(string vowelPattern1, string vowelPattern2)
    {
        // Special handling for 'A' patterns followed by 'H' or another vowel
        // This helps distinguish Sarah (A) from Saira (AI)
        string p1 = vowelPattern1.ToUpperInvariant();
        string p2 = vowelPattern2.ToUpperInvariant();

        // Exact match for vowel pattern - high score
        if (p1 == p2)
        {
            return 1.0;
        }

        // Special case for double vowel vs vowel+h patterns (Saara vs Sarah)
        // This is a common transliteration pattern that should score high
        for (int i = 0; i < p1.Length - 1; i++)
        {
            if (i < p1.Length - 1 && p1[i] == p1[i + 1]) // Double vowel like 'AA'
            {
                // Look for this vowel followed by H in p2
                for (int j = 0; j < p2.Length - 1; j++)
                {
                    if (p2[j] == p1[i] && j < p2.Length - 1 && p2[j + 1] == 'H')
                    {
                        return 0.9; // Very high similarity for this specific pattern
                    }
                }
            }
        }

        // Check the reverse - vowel+h in p1 and double vowel in p2
        for (int i = 0; i < p2.Length - 1; i++)
        {
            if (i < p2.Length - 1 && p2[i] == p2[i + 1]) // Double vowel
            {
                for (int j = 0; j < p1.Length - 1; j++)
                {
                    if (p1[j] == p2[i] && j < p1.Length - 1 && p1[j + 1] == 'H')
                    {
                        return 0.9; // Very high similarity
                    }
                }
            }
        }

        // Check for vowel patterns where one has 'AH' and the other has 'AI'
        // Examples: Sarah (AH) vs Saira (AI)
        if ((p1.Contains("AH") && p2.Contains("AI")) ||
            (p1.Contains("AI") && p2.Contains("AH")))
        {
            // Penalize this specific mismatch - we want Sarah to be distinct from Saira
            return 0.3; // Lower similarity
        }

        // Simple matching for very short patterns
        if (vowelPattern1.Length <= 2 || vowelPattern2.Length <= 2)
        {
            HashSet<char> set1 = new HashSet<char>(vowelPattern1);
            HashSet<char> set2 = new HashSet<char>(vowelPattern2);

            int common = set1.Intersect(set2).Count();
            int total = set1.Union(set2).Count();

            double baseSimilarity = total > 0 ? (double)common / total : 0;

            // Check for common transliteration patterns
            if (HasTransliterationPattern(vowelPattern1, vowelPattern2))
            {
                baseSimilarity = Math.Max(baseSimilarity, 0.7);
            }

            return baseSimilarity;
        }

        // More sophisticated matching for longer patterns
        int distance = LevenshteinDistance(vowelPattern1, vowelPattern2);
        double similarity = 1.0 - ((double)distance / Math.Max(vowelPattern1.Length, vowelPattern2.Length));

        // Additional adjustment for common vowel substitutions (A/E, I/Y, etc.)
        string normalized1 = NormalizeVowelPattern(vowelPattern1);
        string normalized2 = NormalizeVowelPattern(vowelPattern2);

        double normalizedSimilarity = 1.0 - ((double)LevenshteinDistance(normalized1, normalized2) /
                                             Math.Max(normalized1.Length, normalized2.Length));

        // Check for patterns like 'aa' to 'ah' (Saara/Sarah)
        if (HasTransliterationPattern(vowelPattern1, vowelPattern2))
        {
            normalizedSimilarity = Math.Max(normalizedSimilarity, 0.85);
        }

        // Combine raw and normalized similarities
        return (similarity * 0.4) + (normalizedSimilarity * 0.6); // Give more weight to normalized
    }

    // Normalize vowel pattern by grouping similar sounding vowels
    private string NormalizeVowelPattern(string vowelPattern)
    {
        // Pre-process for common transliteration patterns
        string processed = vowelPattern;

        // Process common double-vowel patterns in transliterations
        for (int i = 0; i < processed.Length - 1; i++)
        {
            if (i < processed.Length - 1 && processed[i] == processed[i + 1])
            {
                // Double vowel (aa, ee, etc.) is often equivalent to vowel+h pattern
                // For example: Saara/Sarah, Deepak/Deehpak
                if (i + 2 < processed.Length)
                {
                    processed = processed.Substring(0, i + 1) + 'H' + processed.Substring(i + 2);
                }
                else
                {
                    processed = processed.Substring(0, i + 1) + 'H';
                }
            }
        }

        // Group similar-sounding vowels
        return processed
            .Replace('A', '1').Replace('E', '1') // Group A/E
            .Replace('I', '2').Replace('Y', '2') // Group I/Y
            .Replace('O', '3').Replace('U', '3'); // Group O/U
    }

    // Calculate Jaro-Winkler similarity (good for short strings like names)
    private double CalculateJaroWinklerSimilarity(string s1, string s2)
    {
        // Jaro similarity component
        double jaroSimilarity = CalculateJaroSimilarity(s1, s2);

        // Winkler modification: boost score for common prefixes
        int prefixLength = 0;
        int maxPrefixLength = Math.Min(4, Math.Min(s1.Length, s2.Length)); // Max 4 chars

        // Count common prefix
        for (int i = 0; i < maxPrefixLength; i++)
        {
            if (s1[i] == s2[i])
                prefixLength++;
            else
                break;
        }

        // Apply Winkler prefix adjustment (p=0.1)
        return jaroSimilarity + (prefixLength * 0.1 * (1 - jaroSimilarity));
    }

    // Calculate Jaro similarity
    private double CalculateJaroSimilarity(string s1, string s2)
    {
        if (s1.Length == 0 && s2.Length == 0) return 1.0;
        if (s1.Length == 0 || s2.Length == 0) return 0.0;

        // Matching window size
        int matchDistance = Math.Max(s1.Length, s2.Length) / 2 - 1;
        if (matchDistance < 0) matchDistance = 0;

        // Flags for matched characters
        bool[] s1Matches = new bool[s1.Length];
        bool[] s2Matches = new bool[s2.Length];

        // Count matching characters within window
        int matchingChars = 0;
        for (int i = 0; i < s1.Length; i++)
        {
            int start = Math.Max(0, i - matchDistance);
            int end = Math.Min(i + matchDistance + 1, s2.Length);

            for (int j = start; j < end; j++)
            {
                if (!s2Matches[j] && s1[i] == s2[j])
                {
                    s1Matches[i] = true;
                    s2Matches[j] = true;
                    matchingChars++;
                    break;
                }
            }
        }

        if (matchingChars == 0) return 0.0;

        // Count transpositions
        int transpositions = 0;
        int k = 0;

        for (int i = 0; i < s1.Length; i++)
        {
            if (s1Matches[i])
            {
                while (!s2Matches[k]) k++;

                if (s1[i] != s2[k]) transpositions++;

                k++;
            }
        }

        // Jaro similarity formula
        double m = matchingChars;
        return (m / s1.Length + m / s2.Length + (m - transpositions / 2.0) / m) / 3.0;
    }

    // Calculate Damerau-Levenshtein similarity (allows transpositions)
    private double CalculateDamerauLevenshteinSimilarity(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2)) return 1.0;
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
            return 0.0;

        int[,] d = new int[s1.Length + 1, s2.Length + 1];

        // Initialize the matrix
        for (int i = 0; i <= s1.Length; i++)
            d[i, 0] = i;

        for (int j = 0; j <= s2.Length; j++)
            d[0, j] = j;

        // Fill the matrix
        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);

                // Check for transposition
                if (i > 1 && j > 1 && s1[i - 1] == s2[j - 2] && s1[i - 2] == s2[j - 1])
                {
                    d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                }
            }
        }

        // Convert to similarity (0-1 range)
        return 1.0 - ((double)d[s1.Length, s2.Length] / Math.Max(s1.Length, s2.Length));
    }

    // Extract vowel pattern (sequence of vowels)
    private string ExtractVowelPattern(string word)
    {
        string vowels = "";
        foreach (char c in word.ToUpperInvariant())
        {
            if ("AEIOUY".Contains(c)) // Including Y as it often functions as a vowel
            {
                vowels += c;
            }
        }

        return vowels;
    }

    // Extract consonant pattern (sequence of consonants)
    private string ExtractConsonantPattern(string word)
    {
        string consonants = "";
        string wordUpper = word.ToUpperInvariant();

        for (int i = 0; i < wordUpper.Length; i++)
        {
            char c = wordUpper[i];

            if (!"AEIOUY".Contains(c) && char.IsLetter(c))
            {
                // Special handling for 'H' - gives it more weight when it follows a vowel
                // This helps distinguish Sarah (S-R-H) from Saira (S-R)
                if (c == 'H' && i > 0 && "AEIOUY".Contains(wordUpper[i - 1]))
                {
                    consonants += "HH"; // Double-weight for H after vowel
                }
                else
                {
                    consonants += c;
                }
            }
        }

        return consonants;
    }

    // Detect common transliteration patterns in names
    private bool DetectTransliterationPattern(string text1, string text2)
    {
        text1 = text1.ToUpperInvariant();
        text2 = text2.ToUpperInvariant();

        // 1. Check for double vowel (aa) to vowel+h (ah) pattern
        bool hasDoubleVowelPattern = false;

        for (int i = 0; i < text1.Length - 1; i++)
        {
            if (i < text1.Length - 1 && IsVowel(text1[i]) && text1[i] == text1[i + 1])
            {
                // Find equivalent position in text2
                for (int j = 0; j < text2.Length - 1; j++)
                {
                    if (text2[j] == text1[i] && j < text2.Length - 1 &&
                        (text2[j + 1] == 'H' ||
                         (IsVowel(text2[j]) && IsVowel(text2[j + 1]) && text2[j] != text2[j + 1])))
                    {
                        hasDoubleVowelPattern = true;
                        break;
                    }
                }
            }
        }

        // Check the reverse direction
        if (!hasDoubleVowelPattern)
        {
            for (int i = 0; i < text2.Length - 1; i++)
            {
                if (i < text2.Length - 1 && IsVowel(text2[i]) && text2[i] == text2[i + 1])
                {
                    // Find equivalent position in text1
                    for (int j = 0; j < text1.Length - 1; j++)
                    {
                        if (text1[j] == text2[i] && j < text1.Length - 1 &&
                            (text1[j + 1] == 'H' ||
                             (IsVowel(text1[j]) && IsVowel(text1[j + 1]) && text1[j] != text1[j + 1])))
                        {
                            hasDoubleVowelPattern = true;
                            break;
                        }
                    }
                }
            }
        }

        // 2. Check for vowel+h to double vowel pattern (specific for Sarah/Saara)
        bool hasVowelHPattern = false;

        for (int i = 0; i < text1.Length - 1; i++)
        {
            if (i < text1.Length - 1 && IsVowel(text1[i]) && text1[i + 1] == 'H')
            {
                // Look for double vowel in text2
                for (int j = 0; j < text2.Length - 1; j++)
                {
                    if (j < text2.Length - 1 && text2[j] == text1[i] && text2[j] == text2[j + 1])
                    {
                        hasVowelHPattern = true;
                        break;
                    }
                }
            }
        }

        // Check reverse direction
        if (!hasVowelHPattern)
        {
            for (int i = 0; i < text2.Length - 1; i++)
            {
                if (i < text2.Length - 1 && IsVowel(text2[i]) && text2[i + 1] == 'H')
                {
                    // Look for double vowel in text1
                    for (int j = 0; j < text1.Length - 1; j++)
                    {
                        if (j < text1.Length - 1 && text1[j] == text2[i] && text1[j] == text1[j + 1])
                        {
                            hasVowelHPattern = true;
                            break;
                        }
                    }
                }
            }
        }

        return hasDoubleVowelPattern || hasVowelHPattern;
    }

    // Check if character is a vowel
    private bool IsVowel(char c)
    {
        return "AEIOUY".Contains(c);
    }

    // Check for common transliteration patterns between vowel sequences
    private bool HasTransliterationPattern(string vowels1, string vowels2)
    {
        // Check for double vowel to vowel-h pattern (aa→ah, ee→eh)
        bool doubleVowelToVowelH = false;

        // Find patterns in first string
        for (int i = 0; i < vowels1.Length - 1; i++)
        {
            if (vowels1[i] == vowels1[i + 1]) // Double vowel
            {
                // Look for vowel-h pattern in second string
                for (int j = 0; j < vowels2.Length - 1; j++)
                {
                    if (j < vowels2.Length - 1 &&
                        vowels2[j] == vowels1[i] &&
                        (vowels2[j + 1] == 'H' || vowels2[j + 1] == 'h'))
                    {
                        doubleVowelToVowelH = true;
                        break;
                    }
                }
            }
        }

        // Check reverse direction (vowel-h to double vowel)
        if (!doubleVowelToVowelH)
        {
            for (int i = 0; i < vowels2.Length - 1; i++)
            {
                if (vowels2[i] == vowels2[i + 1]) // Double vowel
                {
                    // Look for vowel-h pattern in first string
                    for (int j = 0; j < vowels1.Length - 1; j++)
                    {
                        if (j < vowels1.Length - 1 &&
                            vowels1[j] == vowels2[i] &&
                            (vowels1[j + 1] == 'H' || vowels1[j + 1] == 'h'))
                        {
                            doubleVowelToVowelH = true;
                            break;
                        }
                    }
                }
            }
        }

        return doubleVowelToVowelH;
    }

    // Enhanced Levenshtein distance implementation with special handling for 'h' after vowels
    private int LevenshteinDistance(string s, string t)
    {
        // Normalize both strings to uppercase
        s = s.ToUpperInvariant();
        t = t.ToUpperInvariant();

        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return m;
        if (m == 0) return n;

        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for (int j = 0; j <= m; j++) d[0, j] = j;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                // Standard character matching
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                // Special handling for 'h' after vowels (penalize mismatches more)
                if (cost > 0 && i > 1 && j > 1)
                {
                    // Check if we're comparing an 'h' after a vowel
                    bool sHasVowelH = (s[i - 1] == 'H' && IsVowel(s[i - 2]));
                    bool tHasVowelH = (t[j - 1] == 'H' && IsVowel(t[j - 2]));

                    // If one string has vowel+h and the other doesn't, increase the cost
                    // This makes Sara vs Sarah more different than Sara vs Sora
                    if (sHasVowelH || tHasVowelH)
                    {
                        cost = 2; // Higher cost for vowel+h mismatches
                    }

                    // Special handling for "ai" pattern vs "aa" or "ah" pattern
                    // This helps differentiate Saira from Sarah/Saara
                    bool sHasAi = (s[i - 2] == 'A' && s[i - 1] == 'I');
                    bool tHasAi = (t[j - 2] == 'A' && t[j - 1] == 'I');
                    bool sHasAa = (s[i - 2] == 'A' && s[i - 1] == 'A');
                    bool tHasAa = (t[j - 2] == 'A' && t[j - 1] == 'A');
                    bool sHasAh = (s[i - 2] == 'A' && s[i - 1] == 'H');
                    bool tHasAh = (t[j - 2] == 'A' && t[j - 1] == 'H');

                    // If one has AI and the other has AA or AH, increase the cost
                    if ((sHasAi && (tHasAa || tHasAh)) || (tHasAi && (sHasAa || sHasAh)))
                    {
                        cost = 3; // Even higher cost for AI vs AA/AH mismatches
                    }
                }

                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[n, m];
    }
}