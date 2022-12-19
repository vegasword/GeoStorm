using System;
using System.Numerics;
using System.Collections.Generic;

using static MyMathLib.Geometry2D;

namespace Geostorm.Core
{
    public class Grunt : Enemy
    {
		// ------- PROPERTIES ------- //

        public float Speed { get; set; }
        
        public Vector2 Direction = new();

		// ------- CONSTRUCTOR ------ //

        public Grunt(in Vector2 position, in float spawnTime, in float speed, in float size)
        {
            Position        = position;
            SpawnTime       = spawnTime;
            Speed           = speed;
            CollisionRadius = 20f * size;
            EntitySize      = size;
            ScorePoint      = 20;
            XPGain          = 1;
        }

        // --------- HERITED -------- //

        protected override void DoUpdate(in GameInputs inputs)
        {
            Direction = Vector2FromPoints(Position, inputs.PlayerPosition).GetNormalized() * Speed;
            Position += Direction;
        }
    }
}
