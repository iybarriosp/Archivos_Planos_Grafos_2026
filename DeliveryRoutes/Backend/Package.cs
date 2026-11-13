
namespace Backend;

// Represents a package with its pickup and delivery locations.
public readonly record struct Package(
    int Id,
    string Pickup,
    string Delivery
);
