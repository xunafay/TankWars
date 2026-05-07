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
    extension(TankDirection directon)
    {
        internal bool IsOpposite(TankDirection otherDirecton) => otherDirecton switch
        {
            TankDirection.North => directon == TankDirection.South,
            TankDirection.South => directon == TankDirection.North,
            TankDirection.East => directon == TankDirection.West,
            TankDirection.West => directon == TankDirection.East,
            _ => throw new ArgumentException(null, nameof(otherDirecton))
        };
    }
}