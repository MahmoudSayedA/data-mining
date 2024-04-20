using App.Models;

namespace App.Algorithms
{
    public class KMeans
    {
        public static List<List<Movie>> ClusterMovies(List<Movie> movies, int k, int maxIterations = 100)
        {
            List<List<Movie>> clusters = new();
            List<Movie> centroids = new();

            // Randomly initialize centroids
            Random rand = new();
            centroids = movies.OrderBy(x => rand.Next()).Take(k).ToList();

            for (int iter = 0; iter < maxIterations; iter++)
            {
                // Assign each movie to the nearest centroid
                clusters = new List<List<Movie>>();
                for (int i = 0; i < k; i++)
                {
                    clusters.Add(new List<Movie>());
                }

                foreach (var movie in movies)
                {
                    int closestCentroidIndex = 0;
                    double minDistance = double.MaxValue;

                    for (int i = 0; i < k; i++)
                    {
                        double distance = EuclideanDistance(movie.IMDBRating, centroids[i].IMDBRating);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            closestCentroidIndex = i;
                        }
                    }

                    clusters[closestCentroidIndex].Add(movie);
                }

                // Update centroids
                List<Movie> newCentroids = new();
                for (int i = 0; i < k; i++)
                {
                    if (clusters[i].Count > 0)
                    {
                        double avgRating = clusters[i].Average(m => m.IMDBRating);
                        Movie newCentroid = new Movie { IMDBRating = avgRating };
                        newCentroids.Add(newCentroid);
                    }
                    else
                    {
                        // If no movies assigned to this cluster, keep the previous centroid
                        newCentroids.Add(centroids[i]);
                    }
                }

                // Check for convergence
                bool converged = true;
                for (int i = 0; i < k; i++)
                {
                    if (centroids[i].IMDBRating != newCentroids[i].IMDBRating)
                    {
                        converged = false;
                        break;
                    }
                }

                if (converged)
                {
                    break;
                }
                else
                {
                    centroids = newCentroids;
                }
            }

            return clusters;
        }

        private static double EuclideanDistance(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x - y, 2));
        }

        public static List<Movie> FindOutliers(List<Movie> values)
        {
            List<Movie> outliers = new();

            // Sort the values in ascending order
            values.Sort();

            // Calculate quartiles
            double q1 = CalculatePercentile(values, 25);
            double q3 = CalculatePercentile(values, 75);

            // Calculate interquartile range (IQR)
            double iqr = q3 - q1;

            // Calculate lower and upper bounds for outliers
            double lowerBound = q1 - 1.5 * iqr;
            double upperBound = q3 + 1.5 * iqr;

            // Find outliers
            foreach (Movie value in values)
            {
                if (value.IMDBRating < lowerBound || value.IMDBRating > upperBound)
                {
                    outliers.Add(value);
                }
            }

            return outliers;
        }

        private static double CalculatePercentile(List<Movie> values, int percentile)
        {
            int n = values.Count;
            double position = (n + 1) * (percentile / 100.0);

            int lowerIndex = (int)Math.Floor(position);
            int upperIndex = (int)Math.Ceiling(position);

            if (lowerIndex == upperIndex)
            {
                return values[lowerIndex - 1].IMDBRating;
            }

            double lowerValue = values[lowerIndex - 1].IMDBRating;
            double upperValue = values[upperIndex - 1].IMDBRating;

            double interpolation = position - lowerIndex;

            return lowerValue + (upperValue - lowerValue) * interpolation;
        }
    }
}
