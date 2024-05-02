using App.Algorithms;
using App.Helpers;
using App.Models;

//namespace app;

//public class Program
//{
//    private static void TestAprioriAlgorithms()
//    {
//        //var transactions = new List<List<string>>
//        //{
//        //    new List<string> { "Bread", "Milk", "Eggs" },
//        //    new List<string> { "Bread", "Butter", "Salamon" },
//        //    new List<string> { "Milk", "Eggs", "Butter", "Meet", "Meet", "Meet" },
//        //    new List<string> { "Bread", "Milk", "Eggs", "Butter" }
//        //};
//        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bakery.csv");
//        var transactions = FileReader.ReadTransactions(path);
//        Console.Write("Min Support: ");
//        _ = double.TryParse(Console.ReadLine(), out double minSupport);
//        Console.Write("Min Confidence: ");
//        _ = double.TryParse(Console.ReadLine(), out double minConfidence);
//        var result = AprioriAlgorithm.Apriori(transactions, minSupport, minConfidence);
//        var frequentItemsets = result.Item1;
//        var associationRules = result.Item2;
//        Console.WriteLine("Frequent Itemsets:");
//        foreach (var itemset in frequentItemsets)
//        {
//            Console.WriteLine(string.Join(", ", itemset.Key) + ": " + itemset.Value);
//        }
//        Console.WriteLine("\n\nAssociation Rules:");
//        foreach (var rule in associationRules)
//        {
//            Console.WriteLine(string.Join(", ", rule.Item1) + " => " + string.Join(", ", rule.Item2));
//        }
//    }
//    // Example usage
//    public static void Main(string[] args)
//    {
//        //TestAprioriAlgorithms();
//        var testData = new List<Movie>
//        {
//            new Movie("The Godfather", 1972, 175, 9.2, 100, 2002655, "Crime, Drama", "Francis Ford Coppola", "Marlon Brando", "$134.97M"),
//            new Movie("The Godfather Part II", 1974, 202, 9.0, 90, 1358608, "Crime, Drama", "Francis Ford Coppola", "Al Pacino", "$57.30M"),
//            new Movie("Ordinary People", 1980, 124, 7.7, 86, 56476, "Drama", "Robert Redford", "Donald Sutherland", "$54.80M"),
//            new Movie("Lawrence of Arabia", 1962, 218, 8.3, 100, 313044, "Adventure, Biography, Drama", "David Lean", "Peter O'Toole", "$44.82M"),
//            new Movie("Straw Dogs", 1971, 113, 7.4, 73, 64331, "Crime, Drama, Thriller", "Sam Peckinpah", "Dustin Hoffman", "NA"),
//            new Movie("Close Encounters of the Third Kind", 1977, 138, 7.6, 90, 216050, "Drama, Sci-Fi", "Steven Spielberg", "Richard Dreyfuss", "$132.09M"),
//            new Movie("Once Upon a Time in the West", 1968, 166, 8.5, 82, 348110, "Western", "Sergio Leone", "Henry Fonda", "$5.32M"),
//            new Movie("The Dirty Dozen", 1967, 150, 7.7, 73, 78858, "Action, Adventure, War", "Robert Aldrich", "Lee Marvin", "$45.30M"),
//            new Movie("Rosemary's Baby", 1968, 137, 8.0, 96, 234034, "Drama, Horror", "Roman Polanski", "Mia Farrow", "NA"),
//            new Movie("Cabaret", 1972, 124, 5.8, 80, 59119, "Drama, Music, Musical", "Bob Fosse", "Liza Minnelli", "$42.77M"),
//             new Movie("Once Upon", 1968, 166, 3.5, 82, 348110, "Western", "Sergio Leone", "Henry Fonda", "$5.32M"),
//            new Movie("Dozen", 1967, 150, 7.1, 73, 78858, "Action, Adventure, War", "Robert Aldrich", "Lee Marvin", "$45.30M"),
//            new Movie("Rosemary", 1968, 137, 6.0, 96, 234034, "Drama, Horror", "Roman Polanski", "Mia Farrow", "NA"),
//            new Movie("Cat", 1972, 124, 5.8, 80, 59119, "Drama, Music, Musical", "Bob Fosse", "Liza Minnelli", "$42.77M"),
//             new Movie("Once We Are", 1968, 166, 4.5, 82, 348110, "Western", "Sergio Leone", "Henry Fonda", "$5.32M"),
//            new Movie("The Dirty house", 1967, 150, 6.7, 73, 78858, "Action, Adventure, War", "Robert Aldrich", "Lee Marvin", "$45.30M"),
//            new Movie("Mama And Baby", 1968, 137, 7.2, 96, 234034, "Drama, Horror", "Roman Polanski", "Mia Farrow", "NA"),
//            new Movie("Django", 1972, 124, 8.7, 80, 59119, "Drama, Music, Musical", "Bob Fosse", "Liza Minnelli", "$42.77M")
//        };

//        int k = 5; // Number of clusters
//        List<List<Movie>> clusters = KMeans.ClusterMovies(testData, k);

//        // Print clusters
//        for (int i = 0; i < clusters.Count; i++)
//        {
//            Console.WriteLine($"Cluster {i + 1}:");
//            foreach (var movie in clusters[i])
//            {
//                Console.WriteLine($"{movie.Name} ({movie.IMDBRating})");
//            }
//            Console.WriteLine();
//        }
//    }
//}
