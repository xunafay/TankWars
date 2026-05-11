using TankWars.API;
using TankWars.Engine.Models;

namespace TankWars.Engine;

internal sealed class Game
{
    internal World World { get; }
    internal List<IBot> Bots { get; init; }
    internal List<Tank> Tanks { get; }
    internal List<Bullet> Bullets { get; } = [];

    internal Game(World world, List<IBot> bots)
    {
        World = world;
        Bots = bots;
        Tanks = InitializeTanks();
    }

    private List<Tank> InitializeTanks()
    {
        return [];
    }
}