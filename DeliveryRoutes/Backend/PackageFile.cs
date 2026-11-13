
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Backend;

public static class PackageFile
{
    // Loads and validates package tasks from a text file.
    public static List<Package> Load(string path, Graph<string> city)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Package file not found: {path}");

        Dictionary<int, string> pickups = new();
        Dictionary<int, string> deliveries = new();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            // Ignore comments and empty lines.
            if (line == "" || line.StartsWith("#"))
                continue;

            string[] parts = line.Split(',');

            if (parts.Length != 3)
                throw new Exception($"Invalid format at line {i + 1}.");

            string location = parts[0].Trim();
            string action = parts[1].Trim();
            string packageText = parts[2].Trim();

            if (!city.ContainsVertex(location))
                throw new Exception($"Unknown location at line {i + 1}: {location}");

            if (!int.TryParse(packageText, out int id) || id <= 0)
                throw new Exception($"Invalid package number at line {i + 1}.");

            if (action.Equals("Pickup", StringComparison.OrdinalIgnoreCase))
            {
                if (!pickups.TryAdd(id, location))
                    throw new Exception($"Repeated pickup for package {id}, line {i + 1}.");
            }
            else if (action.Equals("Delivery", StringComparison.OrdinalIgnoreCase))
            {
                if (!deliveries.TryAdd(id, location))
                    throw new Exception($"Repeated delivery for package {id}, line {i + 1}.");
            }
            else
            {
                throw new Exception($"Invalid action at line {i + 1}: {action}");
            }
        }

        List<Package> packages = new();

        foreach (int id in pickups.Keys.OrderBy(id => id))
        {
            if (!deliveries.ContainsKey(id))
                throw new Exception($"Package {id} has no delivery.");

            packages.Add(new Package(id, pickups[id], deliveries[id]));
        }

        foreach (int id in deliveries.Keys)
        {
            if (!pickups.ContainsKey(id))
                throw new Exception($"Package {id} has no pickup.");
        }

        return packages;
    }
}
