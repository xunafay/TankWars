namespace TankWars.Engine.Models;

internal sealed class Tank
{
    internal required string Id { get; init; }

    internal required int Health { get; set; }
    internal required int MaxHealth { get; init; }

    internal required int X { get; set; }
    internal required int Y { get; set; }
    internal required TankDirection Direction { get; set; }

    internal required Turret Turret { get; init; }

    internal bool IsDestroyed => Health <= 0;
}