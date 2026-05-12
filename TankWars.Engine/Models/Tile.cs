namespace TankWars.Engine.Models;

internal sealed class Tile
{
    internal TileType Type { get; private set; }
    internal Coordinate Coordinate { get; init; }

    internal int Health
    {
        get; 
        set
        {
            field = Math.Clamp(value, 0, MaxHealth);
            if (field == 0)
            {
                Type = TileType.Grass;
            }
        }
    }

    internal int MaxHealth { get; private init; }

    internal bool IsTraversable => Type is TileType.Grass;

    internal Tile(TileType type, Coordinate coordinate)
    {
        Type = type;
        Coordinate = coordinate;

        MaxHealth = 50;
        Health = MaxHealth;
    }
}