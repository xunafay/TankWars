namespace TankWars.Engine.TurnSteps;

internal sealed class RotateTurretsStep : ITurnStep
{
    public void BeforeExecute(Game game) { }

    public void Execute(PlayerBot bot, Game game)
    {
        var orientation = bot.CurrentTurn.RotateTurretTo;
        if (!orientation.HasValue)
        {
            return;
        }

        var tank = game.GetTankForBot(bot);
        tank.Turret.Rotate(orientation.Value);
    }

    public void AfterExecute(Game game) { }
}