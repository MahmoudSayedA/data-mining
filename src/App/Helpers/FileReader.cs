namespace App.Helpers;

public class FileReader
{
    public static List<List<string>> ReadTransactions(string filePath)
    {
        List<List<string>> transactions = new();
        List<string> currentTransaction = new();
        string? previousId = null;

        using (var reader = new StreamReader(filePath))
        {
            if (reader == null)
            {
                throw new FileNotFoundException();
            }
            string? line;
            int i = 1;
            // Read and discard the header line
            reader.ReadLine();
            while (i <= 100 && (line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(',');
                if (items.Length >= 2)
                {
                    string currentId = items[0];
                    if (previousId == null)
                    {
                        // Add the Items column to the current transaction
                        currentTransaction.Add(items[1]);
                    }
                    else if (currentId != previousId)
                    {
                        // Add the Items column to the new transaction
                        transactions.Add(currentTransaction);
                        currentTransaction = new() { items[1] };
                    }
                    else
                    {
                        // Add the Items column to the current transaction
                        currentTransaction.Add(items[1]);
                    }
                    previousId = currentId;
                }
            }

            // Add the last transaction
            if (currentTransaction.Count > 0)
            {
                transactions.Add(currentTransaction);
            }
        }

        return transactions;
    }
}
