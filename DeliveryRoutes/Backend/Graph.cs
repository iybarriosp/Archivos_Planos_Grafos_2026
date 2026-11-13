
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend;

// Represents a graph using an adjacency list.
public class Graph<T> where T : notnull
{
    private readonly Dictionary<T, List<Edge<T>>> _adjacency = new();

    public bool IsDirected { get; }
    public int EdgeCount { get; private set; }

    public Graph(bool directed = false)
    {
        IsDirected = directed;
    }

    public IEnumerable<T> Vertices => _adjacency.Keys;

    public int VertexCount => _adjacency.Count;

    // Checks if a location exists.
    public bool ContainsVertex(T vertex)
    {
        return _adjacency.ContainsKey(vertex);
    }

    // Adds a new location to the graph.
    public bool AddVertex(T vertex)
    {
        if (ContainsVertex(vertex))
            return false;

        _adjacency[vertex] = new List<Edge<T>>();
        return true;
    }

    // Adds a connection between two locations.
    public void AddEdge(T from, T to, double weight = 1)
    {
        AddVertex(from);
        AddVertex(to);

        if (HasEdge(from, to))
            return;

        _adjacency[from].Add(new Edge<T>(from, to, weight));

        if (!IsDirected && !from.Equals(to))
        {
            _adjacency[to].Add(new Edge<T>(to, from, weight));
        }

        EdgeCount++;
    }

    // Checks if a connection exists.
    public bool HasEdge(T from, T to)
    {
        return _adjacency.TryGetValue(from, out var edges)
            && edges.Any(e => e.To.Equals(to));
    }

    // Gets the connections leaving a location.
    public IReadOnlyList<Edge<T>> EdgesFrom(T vertex)
    {
        if (!_adjacency.TryGetValue(vertex, out var edges))
        {
            throw new KeyNotFoundException(
                $"Vertex '{vertex}' is not in the graph.");
        }

        return edges;
    }
}
