//using MoreLinq;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
namespace app;

/// <summary>
/// List equality comparer to compare by list value
/// </summary>
public class ListEqualityComparer : IEqualityComparer<List<string>>
{
    public bool Equals(List<string> x, List<string> y)
    {
        if (x == null && y == null)
            return true;
        if (x == null && y != null)
            return false;
        if (x != null && y == null)
            return false;
        if(x.Count != y.Count) return false;
        x.Sort();
        y.Sort();
        for(int i= 0; i<x.Count; i++) {
            if (x[i] != y[i]) return false;
        }
        return true;
    }

    public int GetHashCode(List<string> obj)
    {
        // Generate a hash code based on the contents of the list
        int hash = 17;
        obj.Sort();
        foreach (var item in obj)
        {
            hash = hash * 23 + item.GetHashCode();
        }
        return hash;
    }
}

public class AprioriAlgorithm
{

    /// <summary>
    /// Function to generate candidate itemsets "i'st - itemsets" of size k
    /// </summary>
    /// <param name="itemsets"></param>
    /// <param name="k"></param>
    /// <returns>new i'st itemsets</returns>
    public static List<List<string>> GenerateNItemsets(List<List<string>> itemsets, int k)
    {
        if (itemsets.Count < k)
        {
            HashSet<string> temp =new();
            foreach (var list in itemsets)
            {
                foreach (var item in list)
                {
                    temp.Add(item);
                }
            }
            return new List<List<string>>() {temp.ToList()};

        }

        HashSet<List<string>> candidates = new(new ListEqualityComparer());
        foreach (var itemset in itemsets)
        {
            candidates.Add(itemset);
        }
        
        // generate combinations
        if(candidates.Count < k)
            return new List<List<string>>();

        var combinations = candidates.Subsets(k).ToList();
        HashSet<List<string>> res = new();
        foreach (var com in combinations)
        {
            var hash = new HashSet<string>();
            foreach (var list in com)
            {
                foreach (var item in list)
                    hash.Add(item);

            }
            if(hash.Count == k)
                res.Add(hash.ToList());
        }
        //var combinations = GenerateCombinations(candidates, k);
        return res.ToList();
    }

    /// <summary>
    /// Helper function to get combinations of items
    /// </summary>
    /// <param name="items"></param>
    /// <param name="k"></param>
    /// <returns>combination of items</returns>
    public static List<List<string>> GenerateCombinations(HashSet<List<string>> items, int k)
    {
        if (k == 1)
            return items.ToList();

        HashSet<List<string>> combinations = new(new ListEqualityComparer());
        int n = items.Count;

        // Convert the HashSet to a list for indexing
        List<List<string>> itemList = new(items);

        // generate combinations
        for (int i = 0; i <= n - k; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                HashSet<string> combination = new();
                foreach (var val in itemList[i])
                    combination.Add(val);
                foreach (var val in itemList[j])
                    combination.Add(val);

                if(combination.Count == k)
                    combinations.Add(combination.ToList());
            }
        }

        return combinations.ToList();
    }

    /// <summary>
    /// Function to calculate support for each candidate itemset
    /// </summary>
    /// <param name="transactions"></param>
    /// <param name="candidates"></param>
    /// <returns></returns>
    public static Dictionary<List<string>, double> CalculateSupport(List<List<string>> transactions, List<List<string>> candidates)
    {
        // calc support count for each item in itemsets
        Dictionary<List<string>, int> supportCounts = new();
        foreach (var transaction in transactions)
        {
            foreach (var candidate in candidates)
            {
                if (IsSubset(candidate, transaction))
                {
                    if (!supportCounts.ContainsKey(candidate))
                    {
                        supportCounts[candidate] = 0;
                    }
                    supportCounts[candidate]++;
                }
            }
        }
        int numTransactions = transactions.Count;
        // calc support 
        Dictionary<List<string>, double> support = supportCounts.ToDictionary(kvp => kvp.Key, kvp => (double)kvp.Value / numTransactions);
        return support;
    }

    // Function to check if one set is a subset of another
    public static bool IsSubset(List<string> candidate, List<string> transaction)
    {
        var candidateSet = new HashSet<string>(candidate);
        return candidateSet.IsSubsetOf(transaction);
    }

    // Main method to perform Apriori algorithm
    public static Tuple<Dictionary<List<string>, double>, List<Tuple<List<string>, List<string>>>> Apriori(List<List<string>> transactions, double minSupport, double minConfidence)
    {
        List<List<string>> itemsets = transactions.SelectMany(t => t).Distinct().Select(item => new List<string> { item }).ToList();
        Dictionary<List<string>, double> frequentItemsets = new();///
        int k = 1;
        while (itemsets.Any())
        {
            List<List<string>> candidates = GenerateNItemsets(itemsets, k);
            Dictionary<List<string>, double> support = CalculateSupport(transactions, candidates);
            Dictionary<List<string>, double> nFrequentItemsets = support.Where(kv => kv.Value >= minSupport).ToDictionary(kv => kv.Key, kv => kv.Value);
            if (!nFrequentItemsets.Any())
            {
                break;
            }
            frequentItemsets = frequentItemsets.Concat(nFrequentItemsets).ToDictionary(kv => kv.Key, kv => kv.Value);
            itemsets = nFrequentItemsets.Keys.ToList();
            k++;
        }
        List<Tuple<List<string>, List<string>>> associationRules = GenerateAssociationRules(frequentItemsets, minConfidence);
        return Tuple.Create(frequentItemsets, associationRules);
    }

    // Function to generate association rules from frequent itemsets
    public static List<Tuple<List<string>, List<string>>> GenerateAssociationRules(Dictionary<List<string>, double> frequentItemsets, double minConfidence)
    {
        List<Tuple<List<string>, List<string>>> associationRules = new List<Tuple<List<string>, List<string>>>();
        foreach (var itemset in frequentItemsets.Keys)
        {
            if (itemset.Count > 1)
            {
                for (int i = 1; i < itemset.Count; i++)
                {
                    //var combinations = GenerateCombinations(new HashSet<List<string>>() { itemset }, i);
                    var combinations = itemset.Subsets(i).ToList();
                    foreach (var antecedent in combinations)
                    {
                        var consequent = itemset.Except(antecedent).ToList();
                        var rule = Tuple.Create(antecedent.ToList(), consequent);
                        associationRules.Add(rule);
                    }
                }
            }
        }
        return associationRules.Where(rule => CalculateConfidence(frequentItemsets, rule) >= minConfidence).ToList();
    }

    // Function to calculate confidence for association rules
    public static double CalculateConfidence(Dictionary<List<string>, double> frequentItemsets, Tuple<List<string>, List<string>> associationRule)
    {
        var antecedent = associationRule.Item1;
        var consequent = associationRule.Item2;
        var supportAntecedent = 0.0;
        var supportRule = 0.0;
        foreach (var item in frequentItemsets)
        {
            var comparable = new ListEqualityComparer();
            if (comparable.Equals(item.Key, antecedent))
                supportAntecedent = item.Value;

            if(comparable.Equals(item.Key, antecedent.Concat(consequent).ToList()))
                supportRule = item.Value;
        }
        return supportRule / supportAntecedent;
    }

    // Example usage
    static void Main(string[] args)
    {
        var transactions = new List<List<string>>
        {
            new List<string> { "Bread", "Milk", "Eggs" },
            new List<string> { "Bread", "Butter" },
            new List<string> { "Milk", "Eggs", "Butter" },
            new List<string> { "Bread", "Milk", "Eggs", "Butter" }
        };
        var minSupport = 0.50;
        var minConfidence = 0.50;
        var result = Apriori(transactions, minSupport, minConfidence);
        var frequentItemsets = result.Item1;
        var associationRules = result.Item2;
        Console.WriteLine("Frequent Itemsets:");
        foreach (var itemset in frequentItemsets)
        {
            Console.WriteLine(string.Join(", ", itemset.Key) + ": " + itemset.Value);
        }
        Console.WriteLine("Association Rules:");
        foreach (var rule in associationRules)
        {
            Console.WriteLine(string.Join(", ", rule.Item1) + " => " + string.Join(", ", rule.Item2));
        }
    }
}
