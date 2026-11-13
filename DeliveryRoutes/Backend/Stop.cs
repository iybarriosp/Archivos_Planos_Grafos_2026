
namespace Backend;

// Represents a pickup or delivery stop.
public readonly record struct Stop(
    int PackageId,
    string Location,
    string Action
)
{
    // Checks whether this stop is a pickup.
    public bool IsPickup =>
        Action == "Pickup";

    // Checks whether this stop is a delivery.
    public bool IsDelivery =>
        Action == "Delivery";
}
