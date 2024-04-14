using app;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public required IFormFile InputFile { get; set; }

        [BindProperty]
        public int Records { get; set; }

        [BindProperty]
        public double MinSupport { get; set; }

        [BindProperty]
        public double MinConfidence { get; set; }
        public Dictionary<List<string>, double>? FrequentItemsets { get; set; }
        public List<Tuple<List<string>, List<string>>>? AssociationRules { get; set; }

        public void OnGet()
        {
            // Optional: Code to execute when the page is initially loaded
        }
        public async Task OnPostAsync()
        {
            if (InputFile == null || InputFile.Length == 0)
                throw new FileNotFoundException();
            
            var transactions = new List<List<string>>();
            using (var memoryStream = new MemoryStream())
            {
                await InputFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                transactions = await FileReader.ReadTransactions(memoryStream);
                // Perform further processing with transactions
            }
            
            var results = AprioriAlgorithm.Apriori(transactions, MinSupport/transactions.Count, MinConfidence/transactions.Count);
            FrequentItemsets = results.Item1;
            AssociationRules = results.Item2;
        }

    }
}
