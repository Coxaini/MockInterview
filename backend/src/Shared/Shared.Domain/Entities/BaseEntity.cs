namespace Shared.Domain.Entities;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; init; }

    public bool Equals(BaseEntity? other)
    {
        return other != null && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is BaseEntity other && Equals(other);
    }

    public static bool operator ==(BaseEntity left, BaseEntity right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BaseEntity? left, BaseEntity? right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}