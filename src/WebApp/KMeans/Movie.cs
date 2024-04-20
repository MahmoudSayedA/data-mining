
namespace WebApp.KMeans
{
    public class Movie
    {
        public string Name { get; set; } = string.Empty;
        public double IMDBRating { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Movie other = (Movie)obj;
            return Name == other.Name && IMDBRating == other.IMDBRating;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IMDBRating);
        }
    }

}
