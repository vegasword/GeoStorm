using System;
using System.Numerics;
using System.Collections.Generic;

using static MyMathLib.Arithmetic;

namespace Geostorm.Core
{
    public class CollisionSystem : ISystem
    {
        public static bool CollisionDetector(Vector2 aPos, float aRadius, Vector2 bPos, float bRadius)
        {
            return SqPow(aPos.X - bPos.X) + SqPow(aPos.Y - bPos.Y) <= SqPow(bRadius + aRadius);
        }

        public void Update(in GameInputs inputs, in GameData data, ref List<GameEvent> events)
        {
            // Bullets / Enemies collisions.
            foreach(Bullet bullet in data.Bullets)
                foreach(Enemy enemy in data.Enemies)
                    if (enemy.HasSpawned &&
                        CollisionDetector(bullet.Position, bullet.CollisionRadius,
                                          enemy.Position,  enemy.CollisionRadius))
                        events.Add(new EnemyKilledEvent(enemy, bullet));

            // Player / Enemies collision.
            foreach(Enemy enemy in data.Enemies)
                if (enemy.HasSpawned &&
                    CollisionDetector(data.Player.Position, data.Player.CollisionRadius,
                                      enemy.Position,       enemy.CollisionRadius))
                    events.Add(new PlayerKilledEvent(data.Player, enemy));
        }
    }
}
