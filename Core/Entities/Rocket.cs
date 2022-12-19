using System;
using System.Numerics;
using System.Collections.Generic;

using static System.MathF;

using static MyMathLib.Arithmetic;
using static MyMathLib.Geometry2D;

namespace Geostorm.Core
{
    public class Rocket : Enemy
    {
		// ------- PROPERTIES ------- //

        public float Speed { get; set; }
        
        public Vector2 Direction;

		// ------- CONSTRUCTOR ------ //

        public Rocket(in Vector2 position, in float spawnTime, in float speed, in float size)
        {
            Position        = position;
            SpawnTime       = spawnTime;
            Speed           = speed;
            CollisionRadius = 20f * size;
            EntitySize      = size;
            ScorePoint      = 50;
            XPGain          = 2;

            Random random = new();
            switch (random.Next(0, 4))
            {
                case 0: Direction = new Vector2( 1f, 0f) * speed; Rotation = 3 * PI / 2f; break;
                case 1: Direction = new Vector2( 0f, 1f) * speed; Rotation = 0f;          break;
                case 2: Direction = new Vector2(-1f, 0f) * speed; Rotation = PI / 2;      break;
                case 3: Direction = new Vector2( 0f,-1f) * speed; Rotation = PI;          break;
                default:                                                                  break;
            }
            
        }

        // --------- HERITED -------- //

        protected override void DoUpdate(in GameInputs inputs)
        {
            if (Position.X - CollisionRadius <= 0f)
			{
				Position  += new Vector2(CollisionRadius, 0);
				Direction *= new Vector2(-1, 1);
                Rotation += PI;
			}
            if (Position.X + CollisionRadius >= inputs.ScreenWidth)
            {
                Position  -= new Vector2(CollisionRadius, 0);
				Direction *= new Vector2(-1, 1);
                Rotation += PI;
            }
			if (Position.Y - CollisionRadius <= 0f)
			{
				Position  += new Vector2(0, CollisionRadius);
				Direction *= new Vector2(1, -1);
                Rotation += PI;
			}
            if (Position.Y + CollisionRadius >= inputs.ScreenHeight)
            {
                Position  -= new Vector2(0, CollisionRadius);
				Direction *= new Vector2(1, -1);
                Rotation += PI;
            }

            Position += Direction;
        }
    }
}
