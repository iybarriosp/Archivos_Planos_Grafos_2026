
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend;

public static class RoutePlanner
{
    // Creates a route using the nearest available stop.
    public static List<Stop> CreateRoute(
        Graph<string> city,
        List<Package> packages,
        string start)
    {
        List<Stop> route = new();

        HashSet<int> pickedUp = new();
        HashSet<int> delivered = new();

        string currentLocation = start;

        // Continue until all packages have been delivered.
        while (delivered.Count < packages.Count)
        {
            var (distances, _) =
                ShortestPaths.Dijkstra(city, currentLocation);

            List<Stop> availableStops = new();

            foreach (Package package in packages)
            {
                // A package must be picked up before delivery.
                if (!pickedUp.Contains(package.Id))
                {
                    availableStops.Add(
                        new Stop(package.Id, package.Pickup, "Pickup"));
                }
                else if (!delivered.Contains(package.Id))
                {
                    availableStops.Add(
                        new Stop(package.Id, package.Delivery, "Delivery"));
                }
            }

            // Choose the closest available stop.
            Stop nextStop = availableStops
                .OrderBy(stop => distances[stop.Location])
                .ThenBy(stop => stop.PackageId)
                .First();

            if (double.IsPositiveInfinity(distances[nextStop.Location]))
            {
                throw new Exception(
                    $"Cannot reach location {nextStop.Location}.");
            }

            route.Add(nextStop);

            if (nextStop.IsPickup)
            {
                pickedUp.Add(nextStop.PackageId);
            }
            else
            {
                delivered.Add(nextStop.PackageId);
            }

            currentLocation = nextStop.Location;
        }

        return route;
    }
}
