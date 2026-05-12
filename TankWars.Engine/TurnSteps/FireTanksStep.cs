namespace TankWars.Engine.TurnSteps;

internal sealed class FireTanksStep : ITurnStep
{
    public void BeforeExecute(Game game) { }

    public void Execute(PlayerBot bot, Game game)
    {
        if (!bot.CurrentTurn.ShouldShoot)
        {
            return;
        }

        var tank = game.GetTankForBot(bot);
        
        var bullet = tank.Fire();
        if (bullet is null)
        {
            return;
        }

        game.Bullets.Add(bullet);
    }

    public void AfterExecute(Game game) { }
}