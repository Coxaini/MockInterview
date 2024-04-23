using System.Collections;
using MatchingGraph.Core;

namespace MockInterview.MatchingGraph.UnitTests;

public static class GraphHelper
{
    public static IEnumerable<Node> GenerateNodes(int count)
    {
        for (var i = 0; i < count; i++) yield return new Node(i);
    }
}