using System;
using System.Numerics;
using System.Collections.Generic;

using static MyMathLib.Geometry2D;

namespace Geostorm.Core
{
	public class Bullet : Entity
	{
		// ------- PROPERTIES ------- //

		public float   Speed	 { get; set; }
		public Vector2 Direction { get; set; }

		// ------- CONSTRUCTOR ------ //

		public Bullet(in Vector2 position, in float rotation, in float speed, in float size)
        {
			Position		= position;
			Rotation		= rotation;
			Speed			= speed;
			CollisionRadius = 5f * size;
			EntitySize		= size;
			Direction		= Vector2FromAngle(rotation, 1f).GetNormalized();
        }

        // --------- METHODS -------- //

		public bool IsOnScreen(in GameInputs inputs)
        {
			return Position.X >= 0f && Position.Y >= 0f && 
				   Position.X <= inputs.ScreenWidth     &&
				   Position.Y <= inputs.ScreenHeight;
        }

		// --------- HERITED -------- //

		public override void Update(in GameInputs inputs, ref List<GameEvent> events)
		{
			if (IsOnScreen(inputs)) Position += Direction * Speed;
			else events.Add(new BulletDestroyedEvent(this));
		}
    }
}
