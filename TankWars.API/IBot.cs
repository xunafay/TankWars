namespace TankWars.API;

public interface IBot
{
    CardinalDirection GetTankRotation();
    OrdinalDirection GetTurretRotation();
}