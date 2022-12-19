using System;
using System.Numerics;
using System.Collections.Generic;

using static MyMathLib.Arithmetic;

namespace Geostorm.Core
{

    public class EnemySpawnSystem : ISystem
    {
		// ------- PROPERTIES ------- //

        private float Timer;
        public  float SpawnRate;

		// ------- CONSTRUCTOR ------ //

        public EnemySpawnSystem(float spawnRate)
        {
            Timer     = 0f;
            SpawnRate = spawnRate;
        }

		// --------- METHODS -------- //
        public static Vector2 SpawnEnemy(in GameInputs inputs, in GameData data)
        {
            Random  random   = new();
            Vector2 result = new (random.Next(50, inputs.ScreenWidth  - 50),
                                  random.Next(50, inputs.ScreenHeight - 50));

            float minX = data.Player.Position.X - data.Player.CollisionRadius * 4f;
            float minY = data.Player.Position.Y - data.Player.CollisionRadius * 4f;
            float maxX = data.Player.Position.X + data.Player.CollisionRadius * 4f;
            float maxY = data.Player.Position.Y + data.Player.CollisionRadius * 4f;

            while (result.X >= minX && result.X <= maxX && result.Y >= minY && result.Y <= maxY)
                result = new (random.Next(50, inputs.ScreenWidth  - 50), random.Next(50, inputs.ScreenHeight - 50));

            return result;
        }

        public void Update(in GameInputs inputs, in GameData data, ref List<GameEvent> events)
        {
            if (Timer <= SpawnRate) Timer += 1f;

            if (Timer >= SpawnRate)
            {
                Random  random   = new();
                Vector2 spawnPos = SpawnEnemy(inputs, data);

                switch(random.Next(0, 3))
                {
                    case 0:
                        events.Add(new GruntSpawnedEvent(new Grunt(spawnPos, 40f, 5f, 1f)));
                        break;

                    case 1:
                        events.Add(new WandererSpawnedEvent(new Wanderer(spawnPos, 30f, 10f, 1f)));
                        break;

                    case 2:
                        events.Add(new RocketSpawnedEvent(new Rocket(spawnPos, 30f, 10f, 1f)));
                        break;

                    default:
                        break;
                }

                Timer = 0f;
            }
        }
    }
}
