using System.Text;

namespace app;

public class FileReader
{
    public static async Task<List<List<string>>> ReadTransactions(Stream file, int records = 100)
    {
        List<List<string>> transactions = new();
        List<string> currentTransaction = new();
        string? previousId = null;

        using (var reader = new StreamReader(file, Encoding.UTF8))
        {
            if (reader == null)
            {
                throw new FileNotFoundException();
            }
            string? line;
            int i = 1;
            // Read and discard the header line
            await reader.ReadLineAsync();
            while (i++ <= records && (line = await reader.ReadLineAsync()) != null)
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
