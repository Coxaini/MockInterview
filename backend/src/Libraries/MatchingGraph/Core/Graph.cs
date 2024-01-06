namespace MatchingGraph.Core;

public class Graph : ICloneable
{
    private readonly Dictionary<Node, HashSet<Edge>> _incidenceList = new();

    public Graph()
    {
    }

    public Graph(IEnumerable<Node> nodes)
    {
        foreach (var node in nodes) AddNode(node);
    }

    public Graph(IEnumerable<Edge> edges)
    {
        foreach (var edge in edges) AddEdge(edge.First, edge.Second, edge.Weight);
    }

    public Graph(IEnumerable<(Node, Node, int)> edges)
    {
        foreach ((var first, var second, int weight) in edges) AddEdge(first, second, weight);
    }

    public int NodesCount => _incidenceList.Count;
    public int EdgesCount { get; private set; }

    public object Clone()
    {
        var graph = new Graph();

        foreach (var node in _incidenceList.Keys) graph.AddNode(node);

        foreach (var (node, edges) in _incidenceList)
        foreach (var edge in edges)
            graph.AddEdge(node, edge.First == node ? edge.Second : edge.First, edge.Weight);

        return graph;
    }

    public void AddNode(Node node)
    {
        if (!_incidenceList.ContainsKey(node)) _incidenceList[node] = new HashSet<Edge>();
    }

    public void AddEdge(Node first, Node second, int weight)
    {
        AddNode(first);
        AddNode(second);

        var edge = new Edge(first, second, weight);

        bool isFirstAdded = _incidenceList[first].Add(edge);
        bool isSecondAdded = _incidenceList[second].Add(edge);

        if (isFirstAdded && isSecondAdded) EdgesCount++;
    }

    public void RemoveEdge(Node first, Node second)
    {
        if (_incidenceList.TryGetValue(first, out var value))
            value
                .RemoveWhere(x => x.Second == second || x.First == second);

        if (_incidenceList.TryGetValue(second, out var value1))
            value1
                .RemoveWhere(x => x.Second == first || x.First == first);

        EdgesCount--;
    }

    public void RemoveNode(Node node)
    {
        if (!_incidenceList.TryGetValue(node, out var value)) return;

        foreach (var neighbour in value.Select(edge => edge.First == node ? edge.Second : edge.First))
        {
            int edgesRemoved = _incidenceList[neighbour].RemoveWhere(x => x.Second == node || x.First == node);
            EdgesCount -= edgesRemoved;
        }

        _incidenceList.Remove(node);
    }

    public IEnumerable<Node> GetNodes()
    {
        return _incidenceList.Keys;
    }

    public IEnumerable<Edge> GetAdjacentEdges(Node node)
    {
        return _incidenceList[node];
    }

    public IEnumerable<Node> GetAdjacentNodes(Node node)
    {
        return _incidenceList[node].Select(edge => edge.First == node ? edge.Second : edge.First);
    }
}