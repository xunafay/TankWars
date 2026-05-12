namespace TankWars.API;

public interface IBot
{
    CardinalDirection GetTankRotation();
    CardinalDirection MoveTank();
    OrdinalDirection GetTurretRotation();
}