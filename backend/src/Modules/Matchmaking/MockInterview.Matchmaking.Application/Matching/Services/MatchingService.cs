using MatchingGraph.Algorithms;
using MatchingGraph.Core;
using MockInterview.Matchmaking.Domain.Models.Interviews;

namespace MockInterview.Matchmaking.Application.Matching.Services;

public class MatchingService : IMatchingService
{
    public IEnumerable<(InterviewOrder, InterviewOrder)> GetBestMatches(IList<InterviewOrder> interviewOrders)
    {
        var graph = new Graph();
        var nodes = interviewOrders.Select((_, i) => new Node(i)).ToList();

        for (var i = 0; i < nodes.Count; i++)
        for (int j = i + 1; j < nodes.Count; j++)
        {
            var edge = new Edge(nodes[i], nodes[j], GetEdgeWeight(interviewOrders[i], interviewOrders[j]));
            graph.AddEdge(nodes[i], nodes[j], edge.Weight);
        }

        var matching = PathGrowingAlgorithm.FindMaximumWeightMatching(graph);

        return matching.Select(edge => (interviewOrders[edge.Item1.Id], interviewOrders[edge.Item2.Id]));
    }

    private static int GetEdgeWeight(InterviewOrder interviewOrder1, InterviewOrder interviewOrder2)
    {
        return interviewOrder1.Technologies.Intersect(interviewOrder2.Technologies).Count();
    }
}