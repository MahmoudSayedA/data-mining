namespace Algorithms
{
    class DecisionTree
    {
        private class Node
        {
            public string Attribute { get; set; }
            public Dictionary<string, Node> Children { get; set; }
            public string Label { get; set; }
        }

        private Node root;

        public void Train(List<List<string>> data, string[] attributes)
        {
            root = BuildTree(data, attributes);
        }

        private Node BuildTree(List<List<string>> data, string[] attributes)
        {
            //if (data.Length == 0)
            //    return null;

            string majorityClass = GetMajorityClass(data);
            if (data.All(d => d.Last() == data[0].Last()))
                return new Node { Label = data[0].Last() };

            if (attributes.Length == 0)
                return new Node { Label = majorityClass };

            string bestAttribute = ChooseBestAttribute(data, attributes);
            Node node = new Node { Attribute = bestAttribute, Children = new Dictionary<string, Node>() };

            string[] attributeValues = data.Select(d => d[Array.IndexOf(data[0], bestAttribute)]).Distinct().ToArray();
            foreach (string value in attributeValues)
            {
                List<List<string>> subset = data.Where(d => d[Array.IndexOf(data[0], bestAttribute)] == value)
                                        .Select(d => d.Where((x, i) => i != Array.IndexOf(data[0], bestAttribute)).ToArray())
                                        .ToArray();
                string[] newAttributes = attributes.Where(a => a != bestAttribute).ToArray();
                node.Children.Add(value, BuildTree(subset, newAttributes));
            }

            return node;
        }

        private string GetMajorityClass(List<List<string>> data)
        {
            var classCounts = data.GroupBy(d => d.Last()).ToDictionary(g => g.Key, g => g.Count());
            return classCounts.OrderByDescending(kv => kv.Value).First().Key;
        }

        private string ChooseBestAttribute(List<List<string>> data, string[] attributes)
        {
            double entropy = CalculateEntropy(data);
            Dictionary<string, double> informationGains = new Dictionary<string, double>();

            foreach (string attribute in attributes)
            {
                int attributeIndex = Array.IndexOf(data[0], attribute);
                var attributeValues = data.Select(d => d[attributeIndex]).Distinct();

                double weightedEntropy = 0;
                foreach (string value in attributeValues)
                {
                    List<List<string>> subset = data.Where(d => d[attributeIndex] == value)
                                            .Select(d => d.Where((x, i) => i != attributeIndex).ToArray())
                                            .ToArray();
                    weightedEntropy += ((double)subset.Length / data.Length) * CalculateEntropy(subset);
                }

                informationGains.Add(attribute, entropy - weightedEntropy);
            }

            return informationGains.OrderByDescending(kv => kv.Value).First().Key;
        }

        private double CalculateEntropy(List<List<string>> data)
        {
            var classCounts = data.GroupBy(d => d.Last()).ToDictionary(g => g.Key, g => g.Count());
            double entropy = 0;

            foreach (var count in classCounts.Values)
            {
                double probability = (double)count / data.Length;
                entropy -= probability * Math.Log(probability, 2);
            }

            return entropy;
        }

        public string Predict(string[] instance)
        {
            return Predict(instance, root);
        }

        private string Predict(string[] instance, Node node)
        {
            if (node.Children == null)
                return node.Label;

            string attributeValue = instance[Array.IndexOf(instance, node.Attribute)];
            if (!node.Children.ContainsKey(attributeValue))
                return node.Children.First().Value.Label;

            return Predict(instance, node.Children[attributeValue]);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<List<string>> data =
            {
                new string[] { "Female", "80.0", "0", "1", "never", "25.19", "6.6", "140", "0" },
                new string[] { "Female", "54.0", "0", "0", "No Info", "27.32", "6.6", "80", "0" },
                new string[] { "Male", "28.0", "0", "0", "never", "27.32", "5.7", "158", "0" },
                new string[] { "Female", "36.0", "0", "0", "current", "23.45", "5.0", "155", "0" }
            };

            string[] attributes = { "gender", "age", "hypertension", "heart_disease", "smoking_history", "bmi", "HbA1c_level", "blood_glucose_level" };

            DecisionTree decisionTree = new DecisionTree();
            decisionTree.Train(data, attributes);

            string[] instance = { "Male", "60.0", "1", "0", "never", "25.0", "7.0", "180" };
            string prediction = decisionTree.Predict(instance);
            Console.WriteLine($"Prediction: {prediction}");

            Console.ReadLine();
        }
    }
}
