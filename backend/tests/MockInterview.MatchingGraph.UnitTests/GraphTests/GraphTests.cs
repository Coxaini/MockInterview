using MatchingGraph.Core;
using Xunit;

namespace MockInterview.MatchingGraph.UnitTests.GraphTests;

public class GraphTests
{
    [Fact]
    public void AddNode_IncreasesNodeCount()
    {
        var graph = new Graph();
        var node = new Node();

        graph.AddNode(node);

        Assert.Equal(1, graph.NodesCount);
    }

    [Fact]
    public void AddEdge_IncreasesEdgesCount()
    {
        var graph = new Graph();
        var node1 = new Node();
        var node2 = new Node();

        graph.AddEdge(node1, node2, 1);

        Assert.Equal(1, graph.EdgesCount);
    }

    [Fact]
    public void RemoveNode_DecreasesNodeCount()
    {
        var graph = new Graph();
        var node = new Node();
        graph.AddNode(node);

        graph.RemoveNode(node);

        Assert.Equal(0, graph.NodesCount);
    }

    [Fact]
    public void RemoveEdge_DecreasesEdgesCount()
    {
        var graph = new Graph();
        var node1 = new Node();
        var node2 = new Node();
        graph.AddEdge(node1, node2, 1);

        graph.RemoveEdge(node1, node2);

        Assert.Equal(0, graph.EdgesCount);
    }

    [Fact]
    public void GetNodes_ReturnsAllNodes()
    {
        var graph = new Graph();
        var node1 = new Node();
        var node2 = new Node();
        graph.AddNode(node1);
        graph.AddNode(node2);

        var nodes = graph.GetNodes();

        Assert.Contains(node1, nodes);
        Assert.Contains(node2, nodes);
    }

    [Fact]
    public void GetAdjacentEdges_ReturnsCorrectEdges()
    {
        var graph = new Graph();
        var node1 = new Node();
        var node2 = new Node();
        var node3 = new Node();
        graph.AddEdge(node1, node2, 1);
        graph.AddEdge(node1, node3, 2);

        var edges = graph.GetAdjacentEdges(node1);

        Assert.Equal(2, edges.Count());
    }

    [Fact]
    public void GetAdjacentNodes_ReturnsCorrectNodes()
    {
        var graph = new Graph();
        var node1 = new Node();
        var node2 = new Node();
        var node3 = new Node();
        graph.AddEdge(node1, node2, 1);
        graph.AddEdge(node1, node3, 2);

        var nodes = graph.GetAdjacentNodes(node1);

        Assert.Contains(node2, nodes);
        Assert.Contains(node3, nodes);
    }
}