namespace MatchingGraph.Core;

public struct Node : IEquatable<Node>
{
    public Node(int id)
    {
        Id = id;
    }

    public int Id { get; init; }

    public bool Equals(Node other)
    {
        return Id == other.Id;
    }

    public static bool operator ==(Node left, Node right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Node left, Node right)
    {
        return !left.Equals(right);
    }

    public override bool Equals(object? obj)
    {
        return obj is Node other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}