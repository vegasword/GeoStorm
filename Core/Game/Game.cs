using System;
using System.Numerics;
using System.Collections.Generic;

using static System.MathF;

using Geostorm.Renderer;

namespace Geostorm.Core
{
    public class Game : IGameEventListener
    {
        // ------- PROPERTIES ------- //

        public GameData         Data;

        public EnemySpawnSystem EnemySpawnSystem;
        public CollisionSystem  CollisionSystem;

        public List<GameEvent>  Events;

		// ------- CONSTRUCTOR ------ //

        public Game(in GameInputs inputs, in int highscore) { NewGame(inputs, highscore); }

        // --------- METHODS -------- //

        public void NewGame(in GameInputs inputs, in int highscore)
        {
            Data             = new(inputs, highscore);
            EnemySpawnSystem = new(35f);
            CollisionSystem  = new();
            Events           = new();
        }

        public void Update(ref GameInputs inputs, ref Graphics scene, ref Audio audio)
        {
            inputs.Update();

            if (!Data.IsGameOver)
            { 
                Data.Update(inputs, ref Events);
                EnemySpawnSystem.Update(inputs, Data, ref Events);
                CollisionSystem.Update(inputs, Data, ref Events);
            }
            else if (Data.IsGameOver && !inputs.ResetGame && Data.Player.Score > Data.Highscore)
            {
                Data.Highscore = Data.Player.Score;
            }
            else if (inputs.ResetGame)
            {
                int highscoreTmp = Data.Highscore;

                // Reset all objets.
                Data             = null;
                EnemySpawnSystem = null;
                CollisionSystem  = null;
                Events           = null;
                GC.Collect();
                NewGame(inputs, highscoreTmp);

                scene.Camera.fovy = 100f;

                Data.IsGameOver = false;
            }

            scene.Update(Data, Events);
            audio.Update();
            audio.HandleEvents(Events);

            HandleEvents(Events);
            Events.Clear();
        }

        public void Render(Graphics render) { render.Draw(Data); }

		// --------- HERITED -------- //

        public void HandleEvents(in List<GameEvent> events)
        {
            foreach (GameEvent e in events)
            {
                switch (e)
                {
                    case PlayerShootEvent playerShootEvent:         Data.Bullets.Add(playerShootEvent.Bullet);        break;
                    case BulletDestroyedEvent bulletDestroyedEvent: Data.Bullets.Remove(bulletDestroyedEvent.Bullet); break;
                    case GruntSpawnedEvent gruntSpawnedEvent:       Data.Enemies.Add(gruntSpawnedEvent.Grunt);        break;
                    case WandererSpawnedEvent gruntSpawnedEvent:    Data.Enemies.Add(gruntSpawnedEvent.Wanderer);     break;
                    case RocketSpawnedEvent gruntSpawnedEvent:      Data.Enemies.Add(gruntSpawnedEvent.Rocket);       break;
                    case PlayerKilledEvent:                         Data.IsGameOver = true;                           break;

                    case EnemyKilledEvent enemyKilledEvent:
                        // Score and xp gain.
                        Data.Player.Score += enemyKilledEvent.Enemy.ScorePoint;
                        Data.Player.XP    += enemyKilledEvent.Enemy.XPGain;

                        // Remove the two entities.
                        Data.Enemies.Remove(enemyKilledEvent.Enemy);
                        Data.Bullets.Remove(enemyKilledEvent.Bullet);

                        // Add particles.
                        Random random = new();
                        for (int i = 0; i < Data.ParticlesNumber; i++)
                            Data.Particles.Add(new(enemyKilledEvent.Enemy.Position,
                                                   random.Next(0, 360) * PI / 180f,
                                                   random.Next(1, 5), 20f));
                        break;

                    case PlayerLevelUp:
                        if (EnemySpawnSystem.SpawnRate >= 1f) EnemySpawnSystem.SpawnRate -= 0.2f;
                        else EnemySpawnSystem.SpawnRate /= 10f;
                        break;

                    case ParticleDestroyedEvent particleDestroyedEvent: Data.Particles.Remove(particleDestroyedEvent.Particle); break;

                    default: break;
                }
            }
        }
    }
}
