﻿namespace Shared.Core.Extensions;

public static class ReadOnlyListExtensions
{
    public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
    {
        var i = 0;
        foreach (var element in self)
        {
            if (Equals(element, elementToFind))
                return i;
            i++;
        }

        return -1;
    }
}