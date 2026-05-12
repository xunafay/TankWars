namespace TankWars.Engine.TurnSteps;

internal sealed class ResetTankStatesStep : ITurnStep
{
    public void BeforeExecute(Game game) { }

    public void Execute(PlayerBot bot, Game game)
    {
        var tank = game.GetTankForBot(bot);

        tank.HasTurned = false;
        tank.HasMoved = false;

        tank.Turret.CurrentCooldown--;
    }

    public void AfterExecute(Game game) { }
}