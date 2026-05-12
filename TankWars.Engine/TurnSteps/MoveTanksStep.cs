using TankWars.Engine.Extensions;
using TankWars.Engine.Models;

namespace TankWars.Engine.TurnSteps;

internal sealed class MoveTanksStep : ITurnStep
{
    public void Execute(Game game)
    {
        foreach (var bot in game.Bots)
        {
            var newOrientation = bot.Implementation.MoveTank();

            var tank = game.GetTankForBot(bot);
            if (tank.Orientation.IsPerpendicular(newOrientation))
            {
                continue;
            }

            var nextPosition = CalculateNextPosition(tank, newOrientation);
            if (!CanMoveToPosition(nextPosition, game))
            {
                continue;
            }

            tank.Position = nextPosition;
        }
    }

    private static bool CanMoveToPosition(Coordinate nextPosition, Game game)
    {
        return game.World.IsInsideOfWorld(nextPosition)
            && game.World.GetTile(nextPosition).IsTraversable
            && !game.Tanks.Any(t => t.Position == nextPosition);
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