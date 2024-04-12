class Program
{
    static void Main()
    {
        // Example transactions
        var transactions = new List<HashSet<string>>
        {
            new() { "Bread", "Milk", "Eggs" },
            new() { "Bread", "Butter" },
            new() { "Milk", "Eggs", "Butter" },
            new() { "Bread", "Milk", "Eggs", "Butter" }
        };

        double minSupport = 0.5;
        double minConfidence = 0.5;

        var frequentItemsets = Apriori(transactions, minSupport);
        var associationRules = GenerateAssociationRules(frequentItemsets, minConfidence);

        Console.WriteLine("Frequent Itemsets:");
        foreach (var itemset in frequentItemsets)
        {
            Console.WriteLine(string.Join(", ", itemset));
        }

        Console.WriteLine("\nAssociation Rules:");
        foreach (var rule in associationRules)
        {
            Console.WriteLine($"{string.Join(", ", rule.Item1)} => {string.Join(", ", rule.Item2)}");
        }
    }

    static List<HashSet<string>> Apriori(List<HashSet<string>> transactions, double minSupport)
    {
        // Generate candidate itemsets of size 1
        var candidates = transactions
            .SelectMany(t => t)
            .Distinct()
            .Select(item => new HashSet<string> { item })
            .ToList();

        var frequentItemsets = new List<HashSet<string>>();

        int k = 1;
        while (candidates.Any())
        {
            // Calculate support for each candidate
            var supportCounts = new Dictionary<HashSet<string>, int>();
            foreach (var transaction in transactions)
            {
                foreach (var candidate in candidates)
                {
                    if (transaction.IsSupersetOf(candidate))
                    {
                        supportCounts.TryGetValue(candidate, out int count);
                        supportCounts[candidate] = count + 1;
                    }
                }
            }

            // Prune candidates based on minimum support
            candidates = candidates.Where(c => supportCounts.TryGetValue(c, out int count) && count >= minSupport * transactions.Count).ToList();

            // Add frequent itemsets of size k
            frequentItemsets.AddRange(candidates);

            // Generate candidates of size k+1
            candidates = GenerateCandidates(candidates, k);

            k++;
        }

        return frequentItemsets;
    }

    static List<HashSet<string>> GenerateCandidates(List<HashSet<string>> itemsets, int k)
    {
        var candidates = new List<HashSet<string>>();
        foreach (var itemset1 in itemsets)
        {
            foreach (var itemset2 in itemsets)
            {
                if (itemset1.Take(k - 1).SequenceEqual(itemset2.Take(k - 1)) && !itemset1.Skip(k - 1).Take(1).SequenceEqual(itemset2.Skip(k - 1).Take(1)))
                {
                    var candidate = new HashSet<string>(itemset1);
                    candidate.UnionWith(itemset2);
                    if (!candidates.Any(c => c.SetEquals(candidate)))
                    {
                        candidates.Add(candidate);
                    }
                }
            }
        }
        return candidates;
    }

    static List<Tuple<HashSet<string>, HashSet<string>>> GenerateAssociationRules(List<HashSet<string>> frequentItemsets, double minConfidence)
    {
        var associationRules = new List<Tuple<HashSet<string>, HashSet<string>>>();
        foreach (var itemset in frequentItemsets)
        {
            if (itemset.Count > 1)
            {
                for (int i = 1; i < itemset.Count; i++)
                {
                    var antecedents = itemset.GetCombinations(i);
                    foreach (var antecedent in antecedents)
                    {
                        var consequent = new HashSet<string>(itemset);
                        consequent.ExceptWith(antecedent);
                        double supportAntecedent = frequentItemsets.Count(fs => fs.SetEquals(antecedent));
                        double supportRule = frequentItemsets.Count(fs => fs.SetEquals(itemset));
                        double confidence = supportRule / supportAntecedent;
                        if (confidence >= minConfidence)
                        {
                            associationRules.Add(new Tuple<HashSet<string>, HashSet<string>>((HashSet<string>)antecedent, consequent));
                        }
                    }
                }
            }
        }
        return associationRules;
    }
    
    // Helper method to get combinations of a set
}
static class Extention
{
    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this HashSet<T> set, int length)
    {
        if (length == 0)
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }

        foreach (var element in set)
        {
            var remaining = new HashSet<T>(set);
            remaining.Remove(element);

            foreach (var combination in GetCombinations(remaining, length - 1))
            {
                yield return combination.Prepend(element);
            }
        }
    }

}
