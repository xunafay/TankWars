namespace TankWars.API;

public sealed class TurnResult
{
    /// <summary>
    /// Direction to rotate the tank to, if null then the tank will not rotate
    /// </summary>
    public CardinalDirection? RotateTankTo { get; set; } = null;

    /// <summary>
    /// Direction to move the tank to, if null then the tank will not move
    /// </summary>
    public CardinalDirection? MoveTankTo { get; set; } = null;

    /// <summary>
    /// Direction to rotate the turret to, if null then the turret will not rotate
    /// </summary>
    public OrdinalDirection? RotateTurretTo { get; set; } = null;

    /// <summary>
    /// Set whether or not the tank should shoot during its turn
    /// </summary>
    public bool ShouldShoot { get; set; } = false;
}