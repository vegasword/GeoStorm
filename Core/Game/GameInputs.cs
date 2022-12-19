using System;
using System.Numerics;

using static Raylib_cs.KeyboardKey;
using static Raylib_cs.MouseButton;
using static Raylib_cs.GamepadAxis;
using static Raylib_cs.GamepadButton;
using static Raylib_cs.Raylib;

namespace Geostorm.Core
{
    public class GameInputs
    {
		// ------- PROPERTIES ------- //

        // Game inputs.
		public bool			ResetGame;
        public float		DeltaTime;
		public readonly int ScreenWidth  = GetScreenWidth();
		public readonly int ScreenHeight = GetScreenHeight();

        // Player inputs.
		public bool    PlayerDashing;
        public bool    PlayerMoving;
        public bool    Shoot;
		public int	   PlayerLevel;
        public Vector2 MoveAxis;
        public Vector2 ShootAxis;
        public Vector2 ShootTarget;
		public Vector2 PlayerPosition;

        // --------- METHODS -------- //

        public void Update()
        {
            // Update game inputs.
            DeltaTime = GetFrameTime();
			ResetGame = IsKeyPressed(KEY_ENTER) ||
					    IsGamepadButtonDown(0, GAMEPAD_BUTTON_MIDDLE_RIGHT);
				
			// Gamepad controls.
			if (IsGamepadAvailable(0))
            {
				PlayerMoving = GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_X) != 0f ||
							   GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_Y) != 0f;

				MoveAxis = new Vector2 (
					GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_X),
					GetGamepadAxisMovement(0, GAMEPAD_AXIS_LEFT_Y)
				);
				
				PlayerDashing = IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_TRIGGER_1) ||
								IsGamepadButtonDown(0, GAMEPAD_BUTTON_LEFT_TRIGGER_2);

				Shoot         = IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_TRIGGER_1) ||
								IsGamepadButtonDown(0, GAMEPAD_BUTTON_RIGHT_TRIGGER_1);

				ShootTarget += new Vector2 (
					GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_X),
					GetGamepadAxisMovement(0, GAMEPAD_AXIS_RIGHT_Y)
				);
			}
            // Keyboards controls.
			else
            {
				MoveAxis = new();

				PlayerMoving  = IsKeyDown(KEY_W) ||
								IsKeyDown(KEY_A) ||
								IsKeyDown(KEY_S) ||
								IsKeyDown(KEY_D);

				if (IsKeyDown(KEY_W)) MoveAxis.Y = -1.0f;
				if (IsKeyDown(KEY_A)) MoveAxis.X = -1.0f;
				if (IsKeyDown(KEY_S)) MoveAxis.Y =  1.0f;
				if (IsKeyDown(KEY_D)) MoveAxis.X =  1.0f;

				PlayerDashing = IsKeyPressed(KEY_SPACE);

				// Mouse controls.
				Shoot       = IsMouseButtonPressed(MOUSE_LEFT_BUTTON) ||
							  IsMouseButtonDown   (MOUSE_LEFT_BUTTON);

				ShootTarget = GetMousePosition();
            }
        }
    }
}
