using System;
using System.Collections.Generic;
using System.Numerics;

using MyMathLib;
using static MyMathLib.Geometry2D;

namespace Geostorm.Core
{
	public class Particle : Entity
	{
        // ------- PROPERTIES ------- //

		public int     Lifespan   { get; set; }
		public int     Power	  { get; set; }
		public float   Speed	  { get; set; }
		public float   Bounciness { get; set; }
		public Vector2 Direction  { get; set; }

		// ------- CONSTRUCTOR ------ //

		public Particle(in Vector2 spawnPos, in float spawnAngle, in int power, in float speed)
        {
			Position		= spawnPos;
			Rotation		= 0f;
			CollisionRadius = 2f;
			Bounciness		= 1.75f;
			Direction       = Vector2FromAngle(spawnAngle, 1f);
			Speed			= speed;
			Power			= power;
			Lifespan		= 30;
        }

		public override void Update(in GameInputs inputs, ref List<GameEvent> events)
        {
			Lifespan -= 1;

			if (Lifespan > 0) Position += Direction;
			else events.Add(new ParticleDestroyedEvent(this));

			// Screen border bounce.
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

			// Speed decceleration.
			if (Speed >= 0.1f) Speed *= 0.96f;
			else		       Speed = 0f;

			Position += Direction * Speed;
        }
	}
}
