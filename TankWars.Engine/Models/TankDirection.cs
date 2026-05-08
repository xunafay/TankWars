namespace TankWars.Engine.Models;

internal enum TankDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

internal static class TankDirectionExtensions
{
    extension(TankDirection direction)
    {
        internal bool IsOpposite(TankDirection otherDirection) => otherDirection switch
        {
            TankDirection.North => direction == TankDirection.South,
            TankDirection.South => direction == TankDirection.North,
            TankDirection.East => direction == TankDirection.West,
            TankDirection.West => direction == TankDirection.East,
            _ => throw new ArgumentException(null, nameof(otherDirection))
        };
    }
}