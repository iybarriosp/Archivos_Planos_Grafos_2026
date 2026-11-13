using Backend;

try
{
    // Load the city map.
    Graph<string> city = CityFile.Load("city.txt");

    Console.WriteLine("CITY MAP LOADED SUCCESSFULLY");
    Console.WriteLine("-----------------------------");
    Console.WriteLine($"Locations: {city.VertexCount}");
    Console.WriteLine($"Connections: {city.EdgeCount}");
}
catch (Exception error)
{
    Console.WriteLine($"Error: {error.Message}");
}

