namespace TankWars.Engine.TurnSteps;

internal sealed class RotateTanksStep : ITurnStep
{
    public void BeforeExecute(Game game) { }

    public void Execute(PlayerBot bot, Game game)
    {
        var orientation = bot.CurrentTurn.RotateTankTo;
        if (!orientation.HasValue)
        {
            return;
        }

        var tank = game.GetTankForBot(bot);
        tank.Rotate(orientation.Value);
    }

    public void AfterExecute(Game game) { }
}