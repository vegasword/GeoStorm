using System;
using System.Numerics;
using System.Collections.Generic;

namespace Geostorm.Core
{
    public class GameData 
    {
        // ------- PROPERTIES ------- //

        public bool IsGameOver = false;

        public int Highscore { get; set; }

        public readonly int ParticlesNumber = 50;

        public Player Player;

        public List<Enemy>    Enemies;
        public List<Bullet>   Bullets;
        public List<Particle> Particles;

		// ------- CONSTRUCTOR ------ //

        public GameData(in GameInputs inputs, in int highscore)
        { 
            NewGameData(inputs, highscore);
        }

        // --------- METHODS -------- //

        public void NewGameData(in GameInputs inputs, in int highscore)
        {
            Player     = new(inputs);
            Enemies    = new();
            Bullets    = new();
            Particles  = new();
            Highscore = highscore;
        }

        public void Update(in GameInputs inputs, ref List<GameEvent> events)
        {
            // Update all active entities.
            Player.Update(inputs, ref events);
            foreach (Enemy    e in Enemies)   e.Update(inputs, ref events);
            foreach (Bullet   b in Bullets)   b.Update(inputs, ref events);
            foreach (Particle p in Particles) p.Update(inputs, ref events);
        }
    }
}
