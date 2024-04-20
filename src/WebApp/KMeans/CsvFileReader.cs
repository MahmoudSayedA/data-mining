
namespace WebApp.KMeans
{
    public class CsvFileReader
    {

        public static async Task<List<Movie>> ReadMovies(Stream stream, int numOfRecords)
        {
            List<Movie> movies = new();

            // Read the lines of the CSV file
            using (var reader = new StreamReader(stream))
            {
                // Skip the header line
                await reader.ReadLineAsync();

                // Read each line of the file
                int recordsRead = 0;
                while (!reader.EndOfStream && recordsRead < numOfRecords)
                {
                    // Read the line
                    string? line = await reader.ReadLineAsync();
                    if(line == null) continue;
                    // Split the line by comma
                    string[] columns = line.Split(',');

                    // Extract the Name and IMDB Rating columns
                    // using data analytics skills
                    if (columns.Length >= 4 && double.TryParse(columns[3], out double imdbRating) && imdbRating >= 0 && imdbRating <= 10)
                    {
                        string name = columns[0];
                        // Add the movie to the list
                        movies.Add(new Movie { Name = name, IMDBRating = imdbRating });
                        recordsRead++;
                    }
                }
            }

            return movies;
        }
    }
}
