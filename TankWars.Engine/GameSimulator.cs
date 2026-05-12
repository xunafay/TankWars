using TankWars.Engine.TurnSteps;

namespace TankWars.Engine;

internal sealed class GameSimulator
{
    private readonly Game _game;

    internal bool IsFinished { get; private set; }

    internal GameSimulator(World world, List<IBot> bots)
    {
        _game = new(world, bots);
        IsFinished = false;
    }

    private static ITurnStep[] GetSteps() =>
    [
        new ResetTankStatesStep(),
        new RotateTanksStep(),
        new MoveTanksStep(),
        new RotateTurretsStep(),
        new FireTanksStep(),
        new MoveBulletsStep(),
    ];

    internal bool DoTurn()
    {
        if (IsFinished)
        {
            return true;
        }

        foreach (var bot in _game.AliveBots)
        {
            bot.DoTurn();
        }

        foreach (var step in GetSteps())
        {
            step.BeforeExecute(_game);

            foreach (var bot in _game.AliveBots)
            {
                step.Execute(bot, _game);
            }

            step.AfterExecute(_game);
        }

        CheckGameState();
        return IsFinished;
    }

    private void CheckGameState()
    {
        var destroyedCount = _game.Tanks.Count(v => v.IsDestroyed);
        if (destroyedCount >= _game.Tanks.Count - 1)
        {
            IsFinished = true;
        }
    }
}