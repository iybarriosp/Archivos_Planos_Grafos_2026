
using System;
using System.Collections.Generic;
using System.IO;

namespace Backend;

public static class CityFile
{
    // Loads the city map from a text file.
    public static Graph<string> Load(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                $"City file not found: {path}");
        }

        Graph<string> city = new Graph<string>(directed: true);
        HashSet<string> streets = new HashSet<string>();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            // Ignore empty lines and comments.
            if (line == "" || line.StartsWith("#"))
                continue;

            string[] parts = line.Split(',');

            if (parts.Length != 4)
                throw new Exception($"Invalid format at line {i + 1}.");

            string from = parts[0].Trim();
            string to = parts[1].Trim();
            string distanceText = parts[2].Trim();
            string direction = parts[3].Trim();

            if (from == "" || to == "")
                throw new Exception($"Empty location at line {i + 1}.");

            if (!int.TryParse(distanceText, out int distance) || distance <= 0)
                throw new Exception($"Invalid distance at line {i + 1}.");

            if (!direction.Equals("TwoWay", StringComparison.OrdinalIgnoreCase)
                && !direction.Equals("OneWay", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception($"Invalid direction at line {i + 1}.");
            }

            // The same physical street cannot appear twice.
            string first = string.CompareOrdinal(from, to) < 0 ? from : to;
            string second = string.CompareOrdinal(from, to) < 0 ? to : from;
            string streetKey = first + "|" + second;

            if (!streets.Add(streetKey))
                throw new Exception($"Repeated street at line {i + 1}.");

            city.AddEdge(from, to, distance);

            if (direction.Equals("TwoWay", StringComparison.OrdinalIgnoreCase))
                city.AddEdge(to, from, distance);
        }

        return city;
    }
}
