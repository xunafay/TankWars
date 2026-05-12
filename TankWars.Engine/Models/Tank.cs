using TankWars.Engine.Extensions;

namespace TankWars.Engine.Models;

internal sealed class Tank
{
    internal int Id { get; private init; }

    internal int Health
    {
        get;
        set => field = Math.Clamp(value, 0, MaxHealth);
    }

    internal int MaxHealth { get; private init; }

    internal Coordinate Position { get; set; }
    internal CardinalDirection Orientation { get; private set; }

    internal Turret Turret { get; private init; }

    internal bool HasTurned { get; set; } = false;
    internal bool HasMoved { get; set; } = false;

    internal bool IsDestroyed => Health == 0;

    internal Tank(int id, int maxHealth, Coordinate startingPosition, CardinalDirection initialOrientation, Turret turret)
    {
        Id = id;
        MaxHealth = maxHealth;
        Health = MaxHealth;
        Position = startingPosition;
        Orientation = initialOrientation;
        Turret = turret;
    }

    internal bool Rotate(CardinalDirection newOrientation)
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
            || !Turret.TryFire())
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