namespace TankWars.Engine.Models;

internal sealed class Turret
{
    internal OrdinalDirection Orientation { get; private set; }

    internal int CurrentCooldown
    {
        get;
        set
        {
            field = value;
            if (field < 0)
            {
                field = 0;
            }
        }
    }

    internal int CooldownPerShot { get; private init; }
    internal int RemainingAmmo
    {
        get; 
        set
        {
            field = value;
            if (field < 0)
            {
                field = 0;
            }
        }
    }
    
    internal int BulletSpeed { get; private init; }

    internal Turret(OrdinalDirection initialOrientation, int cooldownPerShot, int startingAmmo, int bulletSpeed)
    {
        Orientation = initialOrientation;
        CurrentCooldown = 0;
        CooldownPerShot = cooldownPerShot;
        RemainingAmmo = startingAmmo;
        BulletSpeed = bulletSpeed;
    }

    internal void Rotate(OrdinalDirection newOrientation)
        => Orientation = newOrientation;

    internal bool Fire()
    {
        if (RemainingAmmo == 0
            && CurrentCooldown > 0)
        {
            return false;
        }

        CurrentCooldown = CooldownPerShot;
        RemainingAmmo--;

        return true;
    }
}