namespace TankWars.Engine.TurnSteps;

internal sealed class RotateTurretsStep : ITurnStep
{
    public void Execute(Game game)
    {
        foreach (var bot in game.Bots)
        {
            var orientation = bot.Implementation.GetTurretRotation();
            var tank = game.GetTankForBot(bot);
            tank.Turret.Rotate(orientation);
        }
    }
}