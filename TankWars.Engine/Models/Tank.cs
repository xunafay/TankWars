namespace TankWars.Engine.Models;

internal sealed class Tank
{
    internal required string Id { get; init; }

    internal required int Health { get; set; }
    internal required int MaxHealth { get; init; }

    internal required Coordinate Position { get; set; }
    internal required TankDirection Orientation { get; set; }

    internal required Turret Turret { get; init; }

    internal bool IsDestroyed => Health <= 0;

    internal bool Rotate(TankDirection newOrientation)
    {
        if (IsDestroyed 
            || newOrientation.IsOpposite(Orientation))
        {
            return false;
        }

        Orientation = newOrientation;
        return true;
    }

    internal Bullet? Fire()
    {
        if (IsDestroyed
            || !Turret.Fire())
        {
            return null;
        }

        return new Bullet()
        {
            OwnerId = Id,
            Orientation = Turret.Orientation,
            Position = Position,
            Speed = Turret.BulletSpeed
        };
    }
}