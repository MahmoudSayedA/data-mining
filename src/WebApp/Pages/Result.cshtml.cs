using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;

namespace WebApp.Pages
{
    public class ResultModel : PageModel
    {
        public Dictionary<List<string>, double>? FrequentItemsets { get; set; }
        public List<Tuple<List<string>, List<string>>>? AssociationRules { get; set; }

        public void OnGet()
        {
            // Deserialize the results data from TempData
            var frequentItemsetsJson = TempData["FrequentItemsets"] as string;
            var associationRulesJson = TempData["AssociationRules"] as string;

            // Check if TempData contains the required data
            if (!string.IsNullOrEmpty(frequentItemsetsJson) && !string.IsNullOrEmpty(associationRulesJson))
            {
                // Deserialize JSON back into original data structures
                FrequentItemsets = JsonConvert.DeserializeObject<Dictionary<List<string>, double>>(frequentItemsetsJson);
                AssociationRules = JsonConvert.DeserializeObject<List<Tuple<List<string>, List<string>>>>(associationRulesJson);
            }
            else
            {
                // Handle case where data is not found in TempData
                // Redirect to an error page or display an error message
                // You can also add a check for null and handle the null case accordingly
            }
        }
    }
}
