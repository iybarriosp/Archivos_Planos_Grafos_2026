
using System;
using System.Collections.Generic;

namespace Backend;

public static class ShortestPaths
{
    // Finds the shortest distances from a starting location.
    public static (Dictionary<T, double> distances,
                   Dictionary<T, T> previous)
        Dijkstra<T>(Graph<T> graph, T start) where T : notnull
    {
        Dictionary<T, double> distances = new();
        Dictionary<T, T> previous = new();

        PriorityQueue<T, double> queue = new();

        // Initially, all distances are infinite.
        foreach (T vertex in graph.Vertices)
        {
            distances[vertex] = double.PositiveInfinity;
        }

        if (!graph.ContainsVertex(start))
        {
            throw new Exception(
                $"Starting location '{start}' does not exist.");
        }

        distances[start] = 0;
        queue.Enqueue(start, 0);

        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(
                out T? current,
                out double currentDistance))
            {
                break;
            }

            if (current is null)
                continue;

            // Ignore outdated entries.
            if (currentDistance > distances[current])
                continue;

            foreach (Edge<T> edge in graph.EdgesFrom(current))
            {
                double newDistance =
                    distances[current] + edge.Weight;

                if (newDistance < distances[edge.To])
                {
                    distances[edge.To] = newDistance;
                    previous[edge.To] = current;

                    queue.Enqueue(edge.To, newDistance);
                }
            }
        }

        return (distances, previous);
    }

    // Rebuilds the shortest path between two locations.
    public static List<T> GetPath<T>(
        T start,
        T end,
        Dictionary<T, T> previous) where T : notnull
    {
        List<T> path = new();
        T current = end;

        path.Add(current);

        while (!current.Equals(start))
        {
            if (!previous.TryGetValue(
                current,
                out T? previousVertex)
                || previousVertex is null)
            {
                return new List<T>();
            }

            current = previousVertex;
            path.Add(current);
        }

        path.Reverse();

        return path;
    }
}
