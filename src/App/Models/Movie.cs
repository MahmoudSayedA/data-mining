namespace App.Models
{
    public class Movie
    {
        public Movie(string? name, int releaseYear, int duration, double imdbRating, int metascore, int votes, string? genre, string? director, string? cast, string? gross)
        {
            Name = name;
            ReleaseYear = releaseYear;
            Duration = duration;
            IMDBRating = imdbRating;
            Metascore = metascore;
            Votes = votes;
            Genre = genre;
            Director = director;
            Cast = cast;
            Gross = gross;
        }
        public Movie() { }

        public string? Name { get; set; }
        public int ReleaseYear { get; set; }
        public int Duration { get; set; }
        public double IMDBRating { get; set; }
        public int Metascore { get; set; }
        public int Votes { get; set; }
        public string? Genre { get; set; }
        public string? Director { get; set; }
        public string? Cast { get; set; }
        public string? Gross { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Movie other = (Movie)obj;
            return Name == other.Name && IMDBRating == other.IMDBRating;
        }
    }
}
