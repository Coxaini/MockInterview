namespace MatchingGraph.Core;

public readonly struct Edge : IEquatable<Edge>
{
    public Edge(Node first, Node second, int weight)
    {
        First = first;
        Second = second;
        Weight = weight;
    }

    public Node First { get; init; }
    public Node Second { get; init; }

    public int Weight { get; init; }

    public static bool operator ==(Edge left, Edge right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Edge left, Edge right)
    {
        return !left.Equals(right);
    }

    public bool Equals(Edge other)
    {
        return ((First.Equals(other.First) && Second.Equals(other.Second)) ||
                (First.Equals(other.Second) && Second.Equals(other.First))) && Weight == other.Weight;
    }

    public override bool Equals(object? obj)
    {
        return obj is Edge other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;

            hash = hash * 23 + First.GetHashCode();
            hash = hash * 23 + Second.GetHashCode();
            hash = hash * 23 + Weight.GetHashCode();

            return hash;
        }
    }
}