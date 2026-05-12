namespace TankWars.Engine.Models;

internal sealed class Tile
{
    internal TileType Type { get; init; }
    internal Coordinate Coordinate { get; init; }

    internal bool IsTraversable => Type is TileType.Grass;
}