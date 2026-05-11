namespace TankWars.Engine.Models;

internal sealed class PlayerBot
{
    internal IBot Implementation { get; init; }
    internal int Id { get; init; }

    internal PlayerBot(IBot implementation, int id)
    {
        Implementation = implementation;
        Id = id;
    }
}