using MatchingGraph.Core;

namespace MatchingGraph.Algorithms;

public static class PathGrowingAlgorithm
{
    public static IEnumerable<(Node, Node)> FindMaximumWeightMatching(Graph graph)
    {
        List<Edge> M1 = new(graph.NodesCount / 2);
        List<Edge> M2 = new(graph.NodesCount / 2);

        var g = (Graph)graph.Clone();
        var i = 0;

        var firstNodeAdjacency = g.GetAdjacentEdges(g.GetNodes().First()).ToList();
        bool isGraphEven = g.NodesCount % 2 == 0;
        Node node = new();

        while (g.EdgesCount > 0)
        {
            node = g.GetNodes().First();

            while (g.GetAdjacentNodes(node).Any())
            {
                var heaviestEdge = g.GetAdjacentEdges(node).MaxBy(x => x.Weight);

                if (i % 2 == 0)
                    M1.Add(heaviestEdge);
                else
                    M2.Add(heaviestEdge);

                i++;
                g.RemoveNode(node);
                node = heaviestEdge.First == node ? heaviestEdge.Second : heaviestEdge.First;
            }
        }

        if (isGraphEven)
        {
            Edge? firstNAdj = firstNodeAdjacency.FirstOrDefault(x => x.First == node || x.Second == node);
            if (firstNAdj is not null) M2.Add(firstNAdj.Value);
        }

        var maxMatching = MaxByWeight(M1, M2);

        return maxMatching.Select(edge => (edge.First, edge.Second)).ToList();
    }

    private static List<Edge> MaxByWeight(List<Edge> m1, List<Edge> m2)
    {
        return m1.Sum(x => x.Weight) > m2.Sum(x => x.Weight) ? m1 : m2;
    }
}