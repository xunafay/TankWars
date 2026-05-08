namespace TankWars.Engine.Models;

internal sealed class Tank
{
    internal string Id { get; private init; }

    internal int Health { get; set; }
    internal int MaxHealth { get; private init; }

    internal Coordinate Position { get; set; }
    internal TankDirection Orientation { get; private set; }

    internal Turret Turret { get; private init; }

    internal bool IsDestroyed => Health <= 0;

    internal Tank(string id, int maxHealth, Coordinate startingPosition, TankDirection initialOrientation, Turret turret)
    {
        Id = id;
        MaxHealth = maxHealth;
        Health = MaxHealth;
        Position = startingPosition;
        Orientation = initialOrientation;
        Turret = turret;
    }

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