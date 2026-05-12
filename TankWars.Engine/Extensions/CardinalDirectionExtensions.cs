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

        internal bool IsPerpendicular(CardinalDirection otherDirection) => otherDirection switch
        {
            CardinalDirection.North or CardinalDirection.South 
                => direction is CardinalDirection.East or CardinalDirection.West,
            CardinalDirection.East or CardinalDirection.West
                => direction is CardinalDirection.North or CardinalDirection.South,
            _ => throw new ArgumentException(null, nameof(otherDirection))
        };
    }
}
