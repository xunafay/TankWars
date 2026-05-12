namespace TankWars.Engine.TurnSteps;

internal interface ITurnStep
{
    void BeforeExecute(Game game);
    void Execute(PlayerBot bot, Game game);
    void AfterExecute(Game game);
}