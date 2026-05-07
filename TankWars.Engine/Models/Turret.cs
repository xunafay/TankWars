namespace TankWars.Engine.Models;

internal sealed class Turret
{
    internal required TurretDirection Orientation { get; set; }

    internal required int CurrentCooldown { get; set; }
    internal required int CooldownPerShot { get; init; }
    internal required int RemainingAmmo { get; set; }
    internal required int BulletSpeed { get; init; }

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