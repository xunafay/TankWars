namespace TankWars.Engine.Models;

internal sealed class Bullet
{
    internal required int OwnerId { get; init; }
    internal required int Speed { get; init; }
    internal required Coordinate Position { get; set; }
    internal required OrdinalDirection Orientation { get; init; }
}