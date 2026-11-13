
using Backend;

try
{
    // Load the city map.
    Graph<string> city = CityFile.Load("city.txt");

    Console.WriteLine("CITY MAP LOADED SUCCESSFULLY");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Locations: {city.VertexCount}");
    Console.WriteLine($"Connections: {city.EdgeCount}");

    Console.WriteLine();

    // Load the packages.
    List<Package> packages = PackageFile.Load("packages.txt", city);

    Console.WriteLine("PACKAGES LOADED SUCCESSFULLY");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Total packages: {packages.Count}");
    Console.WriteLine($"Total tasks: {packages.Count * 2}");

    Console.WriteLine();

    // Show the first three packages.
    foreach (Package package in packages.Take(3))
    {
        Console.WriteLine($"Package {package.Id}");
        Console.WriteLine($"Pickup: {package.Pickup}");
        Console.WriteLine($"Delivery: {package.Delivery}");
        Console.WriteLine();
    }
}
catch (Exception error)
{
    Console.WriteLine($"Error: {error.Message}");
}
