using TankWars.Engine.Models;

namespace TankWars.Engine;

internal sealed class Game
{
    internal World World { get; }
    internal List<Tank> Tanks { get; }
    internal List<Bullet> Bullets { get; } = [];

    internal Game(World world, List<Tank> tanks)
    {
        World = world;
        Tanks = tanks;
    }
}