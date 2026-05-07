namespace TankWars.Engine.Models;

internal sealed class Bullet
{
    internal required string OwnerId { get; init; }
    internal required int Speed { get; init; }
    internal required Coordinate Position { get; set; }
    internal required TurretDirection Orientation { get; init; }
}