namespace Backend;

// Represents a connection between two vertices.
public readonly record struct Edge<T>(T From, T To, double Weight = 1)
{
    public override string ToString()
    {
        return $"{From} -> {To} ({Weight})";
    }
}

