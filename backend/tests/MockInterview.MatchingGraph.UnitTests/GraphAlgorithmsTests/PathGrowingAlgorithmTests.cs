using MatchingGraph.Algorithms;
using MatchingGraph.Core;
using Xunit;

namespace MockInterview.MatchingGraph.UnitTests.GraphAlgorithmsTests;

public class PathGrowingAlgorithmTests
{
    [Fact]
    public void FindMaximumWeightMatching_Should_ReturnTwoEdges_When_GraphHasFourNodes()
    {
        var nodes = new List<Node>();
        var edges = new List<Edge>();
        for (var i = 0; i < 4; i++) nodes.Add(new Node(i));

        for (var i = 0; i < nodes.Count; i++)
        for (int j = i + 1; j < nodes.Count; j++)
            edges.Add(new Edge(nodes[i], nodes[j], i + j));

        var graph = new Graph(edges);
        var matching = PathGrowingAlgorithm.FindMaximumWeightMatching(graph);

        Assert.Equal(2, matching.Count());
    }
}