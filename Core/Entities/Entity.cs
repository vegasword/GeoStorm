using System;
using System.Collections.Generic;
using System.Numerics;

namespace Geostorm.Core
{
	public abstract class Entity
	{
		public Vector2 Position	       { get; set; }
		public float   Rotation		   { get; set; }
		public float   Velocity        { get; set; }
		public float   CollisionRadius { get; set; }
		public float   EntitySize      { get; set; }
		
		public float Timer = 0f;
		public float SpawnTime  { get; set; }
		public bool  HasSpawned { get; set; }

		public abstract void Update(in GameInputs inputs, ref List<GameEvent> events);
	}
}
