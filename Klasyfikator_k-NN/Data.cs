namespace Klasyfikator_k_NN;

public class Data : IComparable<Data>
{
    public double[] Values { get; set; }
    public string Label { get; set; }
    
    public double Distance { get; set; }

    public int CompareTo(Data? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Distance.CompareTo(other.Distance);
    }
}

