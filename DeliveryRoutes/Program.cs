
using System;
using System.Collections.Generic;
using Backend;

try
{
    // Load the city map and packages.
    Graph<string> city = CityFile.Load("city.txt");
    List<Package> packages = PackageFile.Load("packages.txt", city);

    Console.WriteLine("CITY MAP LOADED SUCCESSFULLY");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Locations: {city.VertexCount}");
    Console.WriteLine($"Connections: {city.EdgeCount}");

    Console.WriteLine();

    Console.WriteLine("PACKAGES LOADED SUCCESSFULLY");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Total packages: {packages.Count}");
    Console.WriteLine($"Total tasks: {packages.Count * 2}");

    Console.WriteLine();

    // Create the first delivery route.
    List<Stop> route = RoutePlanner.CreateRoute(
        city, packages, "D");

    Console.WriteLine("DELIVERY ROUTE");
    Console.WriteLine("-----------------------------");

    string currentLocation = "D";
    double totalDistance = 0;

    int stopNumber = 1;

    foreach (Stop stop in route)
    {
        var (distances, previous) =
            ShortestPaths.Dijkstra(city, currentLocation);

        double distance = distances[stop.Location];

        List<string> path = ShortestPaths.GetPath(
            currentLocation, stop.Location, previous);

        if (path.Count == 0 || double.IsPositiveInfinity(distance))
        {
            throw new Exception(
                $"No route from {currentLocation} to {stop.Location}.");
        }

        totalDistance += distance;

        Console.WriteLine(
            $"{stopNumber}. {stop.Action} package {stop.PackageId} at {stop.Location}");

        Console.WriteLine(
            $"   Path: {string.Join(" -> ", path)}");

        Console.WriteLine(
            $"   Distance: {distance} meters");

        Console.WriteLine();

        currentLocation = stop.Location;
        stopNumber++;
    }

    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Completed tasks: {route.Count}");
    Console.WriteLine(
        $"TOTAL DISTANCE: {totalDistance / 1000.0:F2} km");
}
catch (Exception error)
{
    Console.WriteLine($"Error: {error.Message}");
}
