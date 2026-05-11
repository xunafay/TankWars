namespace TankWars.Engine.TurnSteps;

internal sealed class RotateTanksStep : ITurnStep
{
    public void Execute(Game game)
    {
        foreach (var bot in game.Bots)
        {
            var orientation = bot.Implementation.GetTankRotation();
            var tank = game.GetTankForBot(bot);
            tank.Rotate(orientation);
        }
    }
}