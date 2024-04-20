namespace App.Helpers;

/// <summary>
/// List equality comparer to compare by list value
/// </summary>
public class ListEqualityComparer : IEqualityComparer<List<string>>
{
    public bool Equals(List<string> x, List<string> y)
    {
        if (x == null && y == null)
            return true;
        if (x == null && y != null)
            return false;
        if (x != null && y == null)
            return false;
        if (x.Count != y.Count) return false;
        x.Sort();
        y.Sort();
        for (int i = 0; i < x.Count; i++)
        {
            if (x[i] != y[i]) return false;
        }
        return true;
    }

    public int GetHashCode(List<string> obj)
    {
        // Generate a hash code based on the contents of the list
        int hash = 17;
        obj.Sort();
        foreach (var item in obj)
        {
            hash = hash * 23 + item.GetHashCode();
        }
        return hash;
    }
}
