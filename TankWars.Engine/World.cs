using TankWars.Engine.Models;

namespace TankWars.Engine;

internal sealed class World
{
    internal int Width { get; init; }
    internal int Height { get; init; }
    internal required Tile[][] Tiles { get; init; }

    internal Tile GetTile(Coordinate position) => Tiles[position.Y][position.X];

    internal bool IsInsideOfWorld(Coordinate position) 
        => position.X >= 0 && position.X <= Width
        && position.Y >= 0 && position.Y <= Height;
}