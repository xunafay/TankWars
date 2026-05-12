namespace TankWars.Engine.Models;

internal sealed class PlayerBot
{
    internal IBot Implementation { get; init; }
    internal int Id { get; init; }

    internal TurnResult CurrentTurn { get; private set; } = null!;

    internal PlayerBot(IBot implementation, int id)
    {
        Implementation = implementation;
        Id = id;
    }

    internal void DoTurn() => CurrentTurn = Implementation.DoTurn();
}