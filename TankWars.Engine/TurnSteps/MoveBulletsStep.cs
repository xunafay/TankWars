namespace TankWars.Engine.TurnSteps;

internal sealed class MoveBulletsStep : ITurnStep
{
    private const int BulletDamage = 25;

    public void BeforeExecute(Game game) { }

    public void Execute(PlayerBot bot, Game game) { }

    public void AfterExecute(Game game)
    {
        foreach (var bullet in game.Bullets.ToList())
        {
            for (int i = 0; i < bullet.Speed; i++)
            {
                bullet.Position = CalculateNextPosition(bullet);

                if (!game.World.IsInsideOfWorld(bullet.Position))
                {
                    game.Bullets.Remove(bullet);
                    break;
                }

                if (game.Tanks.FirstOrDefault(t => t.Position == bullet.Position) is Tank impactedTank)
                {
                    impactedTank.Health -= BulletDamage;
                    game.Bullets.Remove(bullet);
                    break;
                }

                var tile = game.World.GetTile(bullet.Position);
                if (!tile.IsTraversable)
                {
                    tile.Health -= BulletDamage;
                    game.Bullets.Remove(bullet);
                    break;
                }
            }
        }
    }

    private static Coordinate CalculateNextPosition(Bullet bullet) => bullet.Orientation switch
    {
        OrdinalDirection.North => bullet.Position with { Y = bullet.Position.Y + 1 },
        OrdinalDirection.South => bullet.Position with { Y = bullet.Position.Y - 1 },
        OrdinalDirection.East => bullet.Position with { X = bullet.Position.X + 1 },
        OrdinalDirection.West => bullet.Position with { X = bullet.Position.X - 1 },
        OrdinalDirection.NorthEast => bullet.Position with { X = bullet.Position.X + 1, Y = bullet.Position.Y + 1 },
        OrdinalDirection.NorthWest => bullet.Position with { X = bullet.Position.X - 1, Y = bullet.Position.Y + 1 },
        OrdinalDirection.SouthEast => bullet.Position with { X = bullet.Position.X + 1, Y = bullet.Position.Y - 1 },
        OrdinalDirection.SouthWest => bullet.Position with { X = bullet.Position.X - 1, Y = bullet.Position.Y - 1 },
        _ => throw new ArgumentException(null, nameof(bullet.Orientation)),
    };
}