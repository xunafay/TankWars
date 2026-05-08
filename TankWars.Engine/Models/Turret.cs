namespace TankWars.Engine.Models;

internal sealed class Turret
{
    internal TurretDirection Orientation { get; private set; }

    internal int CurrentCooldown { get; private set; }
    internal int CooldownPerShot { get; private init; }
    internal int RemainingAmmo { get; private set; }
    internal int BulletSpeed { get; private init; }

    internal Turret(TurretDirection initialOrientation, int cooldownPerShot, int remainingAmmo, int bulletSpeed)
    {
        Orientation = initialOrientation;
        CurrentCooldown = 0;
        CooldownPerShot = cooldownPerShot;
        RemainingAmmo = remainingAmmo;
        BulletSpeed = bulletSpeed;
    }

    internal void Rotate(TurretDirection newOrientation) 
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