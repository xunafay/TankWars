namespace TankWars.Engine.Models;

internal sealed class Turret
{
    internal OrdinalDirection Orientation { get; private set; }

    internal int CurrentCooldown
    {
        get;
        set => field = Math.Clamp(value, 0, CooldownPerShot);
    }

    internal int CooldownPerShot { get; private init; }
    internal int RemainingAmmo
    {
        get; 
        set => field = Math.Clamp(value, 0, 100);
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

    internal bool TryFire()
    {
        if (RemainingAmmo == 0
            || CurrentCooldown > 0)
        {
            return false;
        }

        CurrentCooldown = CooldownPerShot;
        RemainingAmmo--;

        return true;
    }
}