namespace TankWars.Engine.Models;

internal sealed class Turret
{
    internal required TurretDirection Direction { get; set; }

    internal required int CurrentCooldown { get; set; }
    internal required int CooldownPerShot { get; init; }
    internal required int RemainingAmmo { get; set; }
}