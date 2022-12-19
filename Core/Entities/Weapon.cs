using System;
using System.Numerics;
using System.Collections.Generic;

using static System.MathF;

using static MyMathLib.Geometry2D;

namespace Geostorm.Core
{
	public class Weapon
	{
		// ------- PROPERTIES ------- //

		private float Timer = 0f;

		public int	  Level		  { get; set; }
		public float  BulletSpeed { get; set; }
		public float  BulletSize  { get; set; }
		public float  FireRate    { get; set; }

		// ------- CONSTRUCTOR ------ //

		public Weapon(in int level, in float bulletSpeed, in float bulletSize, in float fireRate)
        {
			Level		= level;
			BulletSpeed = bulletSpeed;
			BulletSize  = bulletSize;
			FireRate    = fireRate; 
        }

		public void Update(in Player player, in GameInputs inputs, ref List<GameEvent> events)
		{
			if (Timer <= FireRate) Timer += 1f;

			if (inputs.Shoot && Timer >= FireRate)
			{ 
				Vector2 leftBulletPosition  = player.Position + Vector2FromAngle(player.Rotation, 10f).GetNormal() + Vector2FromAngle(player.Rotation, 20f);
				Vector2 rightBulletPosition = player.Position - Vector2FromAngle(player.Rotation, 10f).GetNormal() + Vector2FromAngle(player.Rotation, 20f);

				for (int i = 0; i < Level; i++)
				{ 
					events.Add(new PlayerShootEvent(new Bullet(leftBulletPosition,  player.Rotation + i * PI / 30f, BulletSpeed, BulletSize)));
					events.Add(new PlayerShootEvent(new Bullet(rightBulletPosition, player.Rotation - i * PI / 30f, BulletSpeed, BulletSize)));
				}

				Timer = 0f;
			}
		}
    }
}
