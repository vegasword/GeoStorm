using System;
using System.Numerics;
using System.Collections.Generic;

using static System.MathF;

using static MyMathLib.Arithmetic;
using static MyMathLib.Geometry2D;

namespace Geostorm.Core
{
    public class Wanderer : Enemy
    {
		// ------- PROPERTIES ------- //

        public float   Bounciness { get; set; }
        public float   Speed      { get; set; }
        
        public Vector2 Direction;

		// ------- CONSTRUCTOR ------ //

        public Wanderer(in Vector2 position, in float spawnTime, in float speed, in float size)
        {
            Position        = position;
            SpawnTime       = spawnTime;
            Speed           = speed;
            Bounciness      = 1f;
            CollisionRadius = 20f * size;
            EntitySize      = size;
            ScorePoint      = 50;
            XPGain          = 2;
            Direction       = new Vector2(1f, 0f);
        }

        // --------- HERITED -------- //

        protected override void DoUpdate(in GameInputs inputs)
        {
            Random random = new();

            Rotation += PI / 15f;

            Direction = Direction.GetRotated(random.Next() % PI / 10 - PI / 20);

            if (Position.X - CollisionRadius <= 0f)
			{
				Position  += new Vector2(CollisionRadius, 0);
				Direction *= new Vector2(-1, 1) * Bounciness;
			}
            if (Position.X + CollisionRadius >= inputs.ScreenWidth)
            {
                Position  -= new Vector2(CollisionRadius, 0);
				Direction *= new Vector2(-1, 1) * Bounciness;
            }
			if (Position.Y - CollisionRadius <= 0f)
			{
				Position  += new Vector2(0, CollisionRadius);
				Direction *= new Vector2(1, -1) * Bounciness;
			}
            if (Position.Y + CollisionRadius >= inputs.ScreenHeight)
            {
                Position  -= new Vector2(0, CollisionRadius);
				Direction *= new Vector2(1, -1) * Bounciness;
            }

            Position += Direction * Speed;
        }
    }
}
