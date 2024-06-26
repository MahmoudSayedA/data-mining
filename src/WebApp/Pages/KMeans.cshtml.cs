using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;
using WebApp.KMeans;

namespace WebApp.Pages
{
    public class KMeansModel : PageModel
    {
        [BindProperty]
        public IFormFile? InputFile { get; set; }

        public int NumOfRecords { get; set; } = 2000;
        [BindProperty]
        public double MaxRecords { get; set; }

        [BindProperty]
        public int NumClusters { get; set; }
        public List<List<Movie>>? ClustersResults { get; set; }
        public List<Movie>? Outliers {  get; set; }
        public void OnGet()
        {
        }

        public async Task OnPostAsync()
        {
            if (InputFile != null && InputFile.Length > 0)
            {
                if (InputFile == null || InputFile.Length == 0)
                    throw new FileNotFoundException();

                var movies = new List<Movie>();
                using (var memoryStream = new MemoryStream())
                {
                    await InputFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    int toBeRead = (int)(MaxRecords/100 * NumOfRecords);
                    movies = await CsvFileReader.ReadMovies(memoryStream, (int)toBeRead);
                }
                var outliers = KMeans.KMeans.FindOutliers(movies);
                Outliers = outliers;
                var filteredMovies = movies.Where(movie => !outliers.Contains(movie)).ToList();
                var clusters = KMeans.KMeans.ClusterMovies(filteredMovies, NumClusters);
                ClustersResults = clusters;
            }
        }
    }
}
