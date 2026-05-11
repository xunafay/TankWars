using TankWars.API;

namespace TankWars.Engine.Extensions;

internal static class CardinalDirectionExtensions
{
    extension(CardinalDirection direction)
    {
        internal bool IsOpposite(CardinalDirection otherDirection) => otherDirection switch
        {
            CardinalDirection.North => direction == CardinalDirection.South,
            CardinalDirection.South => direction == CardinalDirection.North,
            CardinalDirection.East => direction == CardinalDirection.West,
            CardinalDirection.West => direction == CardinalDirection.East,
            _ => throw new ArgumentException(null, nameof(otherDirection))
        };
    }
}
