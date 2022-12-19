using System;
using System.Numerics;
using System.Collections.Generic;

using static System.MathF;
using static MyMathLib.Geometry2D;

using static Raylib_cs.Raylib;
using static Raylib_cs.GamepadAxis;

namespace Geostorm.Core
{
	public class Player : Entity
	{
		// ------- PROPERTIES ------- //
		
		public int	   Score        { get; set; }
		public int     Level		{ get; set; }
		public int     XP			{ get; set; }
		public int     RequiredXP   { get; set; }

		public float   DashPower	{ get; set; }
		public float   Acceleration { get; set; }
		public float   Bounciness   { get; set; }

		public Vector2 Speed        { get; set; }

		public Weapon  Weapon		{ get; set; }

		private bool  IsDashing			   = false;
		private float PreviousAcceleration = 0f;

		// ------- CONSTRUCTOR ------ //

		public Player(in GameInputs inputs)
        {
			Position		= new Vector2(inputs.ScreenWidth / 2f, inputs.ScreenHeight / 2f);
			Rotation		= 0f;
			Velocity		= 10f;
			CollisionRadius = 15f;
			EntitySize		= 1f;

			Score = XP = 0;
			Level = 1;
			RequiredXP = ComputeNextLevelXP(Level);

			DashPower    = 30f;
			Acceleration = 1.04f;
			Bounciness   = 2f;

			Speed  = Vector2Zero();
			Weapon = new(Level, 20f, 1f, 5f);
        }

		// --------- HERITED -------- //

		public override void Update(in GameInputs inputs, ref List<GameEvent> events)
		{
			PlayerMovements(inputs, ref events);

			LevelingUpdate(ref events);

			Weapon.Update(this, inputs, ref events);
		}

		// --------- METHODS -------- //

		private static int ComputeNextLevelXP(in int level) { return level * 4 + 1; }

		public void LevelingUpdate(ref List<GameEvent> events)
        {
			if (XP >= RequiredXP)
            {
				Level++;
				XP = 0;

				// Attribute a bonus to the player when player has leveled up.
				if (Level % 10 == 0)								  Weapon.Level++;
				else if (Level % 7 == 0 && Weapon.FireRate >= 1f)	  Weapon.FireRate -= 0.25f;
				else if (Level % 5 == 0 && Weapon.BulletSpeed <= 200) Weapon.BulletSpeed += 15f;
				else if (Level % 3 == 0 && Acceleration <= 3f)		  Acceleration += 0.25f;
				else												  Score += Level * 100;

				RequiredXP = ComputeNextLevelXP(Level);

				events.Add(new PlayerLevelUp());
            }
        }

		public void PlayerMovements(in GameInputs inputs, ref List<GameEvent> events)
        {
			// Player dash.
			if (inputs.PlayerDashing && IsDashing == false)
            {
				IsDashing		     = true;
				PreviousAcceleration = Acceleration;
				Acceleration	     = DashPower;
				events.Add(new PlayerDashed());
            }
			else if (IsDashing == true && PreviousAcceleration <= Acceleration)
            {
				Acceleration -= DashPower / 4f;
            }
			else
            {
				if (IsDashing == true) Acceleration = PreviousAcceleration;

				IsDashing			 = false;
				PreviousAcceleration = 0f;
            }

			// Player acceleration and decceleration.
			if (inputs.PlayerMoving && Speed.Length() < Velocity)
				Speed += inputs.MoveAxis * Acceleration;

			if (Speed.Length() >= 0.1f) Speed *= 0.96f;
			else					    Speed = Vector2Zero();

			Position += Speed;

			// Player bouncing screen update.
			if (Position.X - CollisionRadius <= 0f)
			{
				Position += new Vector2(CollisionRadius, 0);
				Speed    *= new Vector2(-1, 1) * Bounciness;
				events.Add(new PlayerBounced());
				if (Speed.Length() > Velocity) Speed = Speed.GetModifiedLength(Velocity);
			}
			if (Position.X + CollisionRadius >= inputs.ScreenWidth)
            {
				Position -= new Vector2(CollisionRadius, 0);
				Speed    *= new Vector2(-1, 1) * Bounciness;
				events.Add(new PlayerBounced());
				if (Speed.Length() > Velocity) Speed = Speed.GetModifiedLength(Velocity);
            }
			if (Position.Y - CollisionRadius <= 0f)
			{
				Position += new Vector2(0, CollisionRadius);
				Speed    *= new Vector2(1, -1) * Bounciness;
				events.Add(new PlayerBounced());
				if (Speed.Length() > Velocity) Speed = Speed.GetModifiedLength(Velocity);
			}
			if (Position.Y + CollisionRadius >= inputs.ScreenHeight)
            {
				Position -= new Vector2(0, CollisionRadius);
				Speed    *= new Vector2(1, -1) * Bounciness;
				events.Add(new PlayerBounced());
				if (Speed.Length() > Velocity) Speed = Speed.GetModifiedLength(Velocity);
            }

			inputs.PlayerPosition = Position;

			// Player rotation.
			if (IsGamepadAvailable(0))
			{ 
				Vector2 Direction = new(GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_X),
										GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_Y));
				Rotation = Direction.GetAngle();
			}
			else
            {
				
				Rotation = Vector2FromPoints(inputs.ShootTarget, Position).GetAngle() + MathF.PI;
            }
		}
	}
}
