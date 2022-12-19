using System;
using System.Numerics;
using System.Collections.Generic;

namespace Geostorm.Core
{
	public abstract class Enemy : Entity
	{
		public int ScorePoint { get; set; }
		public int XPGain	  { get; set; }

		// --------- HERITED -------- //

		public override void Update(in GameInputs inputs, ref List<GameEvent> events)
		{
			if (Timer <= SpawnTime && !HasSpawned)
			{
				Timer += 1f;
			}
			else
			{
				HasSpawned = true;
				Timer	   = 0f;
				DoUpdate(inputs);
			}
		}

		// --------- METHODS -------- //

		protected abstract void DoUpdate(in GameInputs inputs);

	}
}
