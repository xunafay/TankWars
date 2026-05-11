using TankWars.Engine.Models;

namespace TankWars.Engine;

internal sealed class World
{
    internal required Tile[][] Tiles { get; init; }
}