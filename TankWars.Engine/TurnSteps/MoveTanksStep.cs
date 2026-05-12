using TankWars.Engine.Extensions;

namespace TankWars.Engine.TurnSteps;

internal sealed class MoveTanksStep : ITurnStep
{
    private readonly Dictionary<Tank, Coordinate> _nextPositionForTanks = [];

    public void BeforeExecute(Game game)
    {
        foreach (var tank in game.Tanks)
        {
            _nextPositionForTanks.Add(tank, tank.Position);
        }
    }

    public void Execute(PlayerBot bot, Game game)
    {
        var newOrientation = bot.CurrentTurn.MoveTankTo;
        if (!newOrientation.HasValue)
        {
            return;
        }

        var tank = game.GetTankForBot(bot);
        if (tank.HasTurned
            || tank.Orientation.IsPerpendicular(newOrientation.Value))
        {
            return;
        }

        var nextPosition = CalculateNextPosition(tank, newOrientation.Value);
        if (!CanMoveToPosition(nextPosition, game))
        {
            return;
        }

        _nextPositionForTanks[tank] = nextPosition;
    }

    public void AfterExecute(Game game)
    {
        foreach (var (tank, nextPosition) in _nextPositionForTanks)
        {
            if (_nextPositionForTanks.Count(kvp => kvp.Value == nextPosition) == 1)
            {
                tank.Position = nextPosition;
                tank.HasMoved = true;
            }
        }
    }

    private static bool CanMoveToPosition(Coordinate nextPosition, Game game)
    {
        return game.World.IsInsideOfWorld(nextPosition)
            && game.World.GetTile(nextPosition).IsTraversable;
    }

    private static Coordinate CalculateNextPosition(Tank tank, CardinalDirection newOrientation) => newOrientation switch
    {
        CardinalDirection.North => tank.Position with { Y = tank.Position.Y + 1 },
        CardinalDirection.South => tank.Position with { Y = tank.Position.Y - 1 },
        CardinalDirection.East => tank.Position with { X = tank.Position.X + 1 },
        CardinalDirection.West => tank.Position with { X = tank.Position.X - 1 },
        _ => throw new ArgumentException(null, nameof(newOrientation)),
    };
}