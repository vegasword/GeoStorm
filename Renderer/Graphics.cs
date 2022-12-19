using System;
using System.Numerics;
using System.Collections.Generic;

using static System.MathF;
using static MyMathLib.Arithmetic;
using static MyMathLib.Geometry2D;

using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlags;
using static Raylib_cs.TraceLogLevel;
using static Raylib_cs.Color;
using static Raylib_cs.CameraProjection;
using static Raylib_cs.MaterialMapIndex;
using static Raylib_cs.CameraMode;
using static Raylib_cs.ShaderUniformDataType;

using Geostorm.Core;

namespace Geostorm.Renderer
{
	public class Graphics : IGameEventListener
	{
		// ------- PROPERTIES ------- //

		private readonly List<Vector2> PlayerVertices   = new();
		private readonly List<Vector2> BulletVertices   = new();
		private readonly List<Vector2> GruntVertices    = new();
		private readonly List<Vector2> WandererVertices = new();
		private readonly List<Vector2> RocketVertices   = new();

		private Vector2 ScenePosition;

		private int StarsCount;
		private readonly Dictionary<Vector2, float> Stars = new();

		private Model     UFOModel;
		private float     UFORotation;
		public  Camera3D  Camera;

		private RenderTexture2D GameRenderTexture;

		// ------- CONSTRUCTOR ------ //

		public unsafe Graphics(Vector2 windowSize, string windowName, bool fullScreen, int monitor = 0)
        {
			// Initialize Raylib window.
			SetTraceLogCallback(&Logging.LogConsole);
			SetTraceLogLevel(TraceLogLevel.LOG_FATAL);
			SetConfigFlags(FLAG_VSYNC_HINT | FLAG_MSAA_4X_HINT);

			if (fullScreen)
			{
				InitWindow(GetMonitorWidth(monitor), GetMonitorHeight(monitor), windowName);
				ToggleFullscreen();
			}
			else
            {
				InitWindow((int)windowSize.X, (int)windowSize.Y, windowName);
            }

			SetTargetFPS(60);

			// Initialize graphics data.
			Load();
        }

		// --------- METHODS -------- //

		public void Load()
        {
			// Vertices setup.
			float preScale = 20f;
			PlayerVertices.Add(new Vector2(-0.3f, 0.0f)  * preScale);
			PlayerVertices.Add(new Vector2( 0.0f,-0.55f) * preScale);
			PlayerVertices.Add(new Vector2( 0.8f,-0.3f)  * preScale);
			PlayerVertices.Add(new Vector2(-0.2f,-0.8f)  * preScale);
			PlayerVertices.Add(new Vector2(-0.8f, 0.0f)  * preScale);
			PlayerVertices.Add(new Vector2(-0.2f, 0.8f)  * preScale);
			PlayerVertices.Add(new Vector2( 0.8f, 0.3f)  * preScale);
			PlayerVertices.Add(new Vector2( 0.0f, 0.55f) * preScale);
			PlayerVertices.Add(new Vector2(-0.3f, 0.0f)  * preScale);

			preScale = 15.0f;
			BulletVertices.Add(new Vector2(-0.1f, 0.2f) * preScale);
            BulletVertices.Add(new Vector2( 0.8f, 0.0f) * preScale);
            BulletVertices.Add(new Vector2(-0.1f,-0.2f) * preScale);
            BulletVertices.Add(new Vector2(-0.3f, 0.0f) * preScale);
			BulletVertices.Add(new Vector2(-0.1f, 0.2f) * preScale);

			preScale = 18f;
			GruntVertices.Add(new Vector2(-1.0f,  0.0f) * preScale);
			GruntVertices.Add(new Vector2( 0.0f, -1.0f) * preScale);
			GruntVertices.Add(new Vector2( 1.0f,  0.0f) * preScale);
			GruntVertices.Add(new Vector2( 0.0f,  1.0f) * preScale);
			GruntVertices.Add(new Vector2(-1.0f,  0.0f) * preScale);
            
			WandererVertices.Add(new Vector2( 0.0f, 1.0f) * preScale);
            WandererVertices.Add(new Vector2( 0.2f, 0.2f) * preScale);
            WandererVertices.Add(new Vector2( 1.0f, 0.0f) * preScale);
            WandererVertices.Add(new Vector2( 0.2f,-0.2f) * preScale);
            WandererVertices.Add(new Vector2( 0.0f,-1.0f) * preScale);
            WandererVertices.Add(new Vector2(-0.2f,-0.2f) * preScale);
            WandererVertices.Add(new Vector2(-1.0f, 0.0f) * preScale);
            WandererVertices.Add(new Vector2(-0.2f, 0.2f) * preScale);
            WandererVertices.Add(new Vector2( 0.0f, 1.0f) * preScale);
            WandererVertices.Add(new Vector2( 0.0f, 1.0f) * preScale);

			RocketVertices.Add(new Vector2( 0f, 1.5f) * preScale);
			RocketVertices.Add(new Vector2(-1f, 0f)   * preScale);
			RocketVertices.Add(new Vector2( 1f, 0f)   * preScale);
			RocketVertices.Add(new Vector2( 0f, 1.5f) * preScale);

			// Scene setup.
			ScenePosition = new(0f);

			// Stars setup.
			Random random = new();
			StarsCount = random.Next(200, 300);
			for (int i = 0; i < StarsCount; i++)
			{ 
				Vector2 StarPosition = new((float)random.Next(-200, GetScreenWidth() + 200),
										   (float)random.Next(-200, GetScreenHeight() + 200));
				float   StarRadius = random.Next(1, 4);
				Stars.Add(StarPosition, StarRadius);
			}

			// UFO model loading.
			UFOModel    = LoadModel("Assets/UFO/Low_poly_UFO.obj");
			UFORotation = 0f;

			// Camera 3D setup.
			Camera			  = new();
			Camera.position   = new(0f, 10f, 10f);
			Camera.target     = new(0f, 0f, -1f);
			Camera.up		  = new(0f, 1f, 0f);
			Camera.fovy		  = 100f;
			Camera.projection = CAMERA_PERSPECTIVE;
			SetCameraMode(Camera, CAMERA_FIRST_PERSON);

			// Shaders setup;
			GameRenderTexture = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());
		}

		public void Unload()
		{
			// Unload UFO.
			UnloadModel(UFOModel);

			// Unload render textures.
			UnloadRenderTexture(GameRenderTexture);

			// Do the rest of the unload procedure.
			CloseWindow();
		}

		public static void DrawLinesStripEx(in List<Vector2> vertices, in float thickness, in Color color)
        {
			for (int i = 1; i < vertices.Count; i++) DrawLineEx(vertices[i - 1], vertices[i], thickness, color);
        }

		public void DrawEntity<T> (in T entity, in float thickness, in Color color) where T : Entity
        {
			// Local copy in order to manipulate vertices easily according to the entity type.
			List<Vector2> entityVertices    = new();
			List<Vector2> entityVerticesRef = new();
			
			if      (entity is Player)   entityVertices = entityVerticesRef = new(PlayerVertices);
			else if (entity is Bullet)   entityVertices = entityVerticesRef = new(BulletVertices);
			else if (entity is Grunt)    entityVertices = entityVerticesRef = new(GruntVertices);
			else if (entity is Wanderer) entityVertices = entityVerticesRef = new(WandererVertices);
			else if (entity is Rocket)   entityVertices = entityVerticesRef = new(RocketVertices);
				
			// Set vertices transform according to rotation.
			for (int i = 0; i < entityVertices.Count; i++)
			{
				if (!entity.HasSpawned && entity is Enemy)
					entityVertices[i] *= Remap(entity.Timer, 0, entity.SpawnTime, 0, 1);

				entityVertices[i] *= entity.EntitySize;
				entityVertices[i]  = entityVerticesRef[i] + entity.Position;
				entityVertices[i]  = entityVerticesRef[i].GetRotatedAsPoint(entity.Rotation, entity.Position);
			}

			// Draw vertices.
			DrawLinesStripEx(entityVertices, thickness, color);
        }

		public static void DrawParticle(in Particle particle, Color color)
        {
			DrawLineV(particle.Position + particle.Direction * particle.Lifespan * particle.Power, particle.Position, color);
        }

		public static void DrawCrosshair(in GameData data, in float size, in Color color)
        {
			List<Vector2> cross = new();
			float radius        = -Remap(data.Player.Position.GetDistanceFromPoint(GetMousePosition()), 0, GetScreenWidth(), size * 1.25f, size * 5f);
			float crossSize     =  Remap(radius,  -size * 1.25f, -size * 5f, 1f, 3f);

			cross.Add(new Vector2(GetMousePosition().X - size * crossSize, GetMousePosition().Y).GetRotatedAsPoint(data.Player.Rotation, GetMousePosition()));
			cross.Add(new Vector2(GetMousePosition().X + size * crossSize, GetMousePosition().Y).GetRotatedAsPoint(data.Player.Rotation, GetMousePosition()));
			cross.Add(new Vector2(GetMousePosition().X , GetMousePosition().Y).GetRotatedAsPoint(data.Player.Rotation, GetMousePosition()));
			cross.Add(new Vector2(GetMousePosition().X , GetMousePosition().Y - size * crossSize).GetRotatedAsPoint(data.Player.Rotation, GetMousePosition()));
			cross.Add(new Vector2(GetMousePosition().X , GetMousePosition().Y + size * crossSize).GetRotatedAsPoint(data.Player.Rotation, GetMousePosition()));

			DrawLinesStripEx(cross, crossSize, color);
			DrawCircleLines(GetMouseX(), GetMouseY(), radius, color);
        }

		public static void DrawInGameUI(in GameData data)
        {
			// Draw player current level.
			string LevelText = "LEVEL " + data.Player.Level.ToString();
			float LevelFontSize = 30f, LevelFontSpacing = 1f;
			Vector2 LevelPosition = new Vector2(GetScreenWidth() / 2, GetScreenHeight() - 60f) -
									MeasureTextEx(GetFontDefault(), LevelText, LevelFontSize, LevelFontSpacing) / 2f;

			DrawTextEx(GetFontDefault(), LevelText, LevelPosition, LevelFontSize, LevelFontSpacing, WHITE);

			// Draw player XP bar.
			int xp = data.Player.XP, rxp = data.Player.RequiredXP;
			float barPadding = GetScreenWidth() / 12f, barRoundness = 25f;

			Rectangle bar	    = new(barPadding, GetScreenHeight() - barPadding / 5f, GetScreenWidth() - barPadding * 2f, barPadding / 24f);
			Rectangle barFilled = new(barPadding, GetScreenHeight() - barPadding / 5f, Remap(xp, 0, rxp, 0, GetScreenWidth() - barPadding * 2f), barPadding / 24f);

			DrawRectangleRoundedLines(bar, barRoundness,  3, 2f, WHITE);
			DrawRectangleRounded(barFilled, barRoundness, 3, GREEN);
        }

		public static void DrawGameOverSreen(in GameData data)
        {
			// Create titles.
			string  YouDiedText     = "YOU DIED";
			float   YouDiedFontSize = 300f, YouDiedFontSpacing = 30f;
			Vector2 YouDiedPosition = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f) -
									  MeasureTextEx(GetFontDefault(), YouDiedText, YouDiedFontSize, YouDiedFontSpacing) / 2f;

			string  HightScoreText     = "HIGHSCORE: " + data.Highscore;
			float   HightScoreFontSize = 50f, HightScoreFontSpacing = 10f;
			Vector2 HightScorePosition = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f + YouDiedPosition.Y / 2f) -
								         MeasureTextEx(GetFontDefault(), HightScoreText, HightScoreFontSize, HightScoreFontSpacing) / 2f;

			string  ScoreText     = "SCORE: " + data.Player.Score;
			float   ScoreFontSize = 100f, ScoreFontSpacing = 15f;
			Vector2 ScorePosition = new Vector2(GetScreenWidth() / 2f, GetScreenHeight() / 2f + YouDiedPosition.Y) -
								    MeasureTextEx(GetFontDefault(), ScoreText, ScoreFontSize, ScoreFontSpacing) / 2f;

			string  ReturnText     = "PRESS SPACE TO RESTART";
			float   ReturnFontSize = 50f, ReturnFontSpacing = 5f;
			Vector2 ReturnPosition = new Vector2(GetScreenWidth() / 2f, ScorePosition.Y + MeasureTextEx(GetFontDefault(), ReturnText, ReturnFontSize, ReturnFontSpacing).Y * 3f) -
								     MeasureTextEx(GetFontDefault(), ReturnText, ReturnFontSize, ReturnFontSpacing) / 2f;

			// Display titles.
			DrawTextEx(GetFontDefault(), YouDiedText,    YouDiedPosition,    YouDiedFontSize,    YouDiedFontSpacing,    RED);
			DrawTextEx(GetFontDefault(), HightScoreText, HightScorePosition, HightScoreFontSize, HightScoreFontSpacing, WHITE);
			DrawTextEx(GetFontDefault(), ScoreText,      ScorePosition,      ScoreFontSize,      ScoreFontSpacing,      WHITE);
			DrawTextEx(GetFontDefault(), ReturnText,     ReturnPosition,     ReturnFontSize,     ReturnFontSpacing,     WHITE);
        }

		public void Update(in GameData data, in List<GameEvent> events)
        {
			ScenePosition = new (Remap(data.Player.Position.X, 0f, (float)GetScreenWidth(), 1f, -1f),
						         Remap(data.Player.Position.Y, 0f, (float)GetScreenWidth(), 0f, 1f));

			HandleEvents(events);
        }

		public void Draw(in GameData data)
        {
			// ------- RENDER TEXTURES ------- //

			// Game rendering texture.
			BeginTextureMode(GameRenderTexture);
			ClearBackground(new Color(0, 0, 0, 30));

			if (!data.IsGameOver)
			{ 
				// Draw entities.
				DrawEntity(data.Player, 2f, GREEN);
				foreach(Bullet bullet in data.Bullets)		 DrawEntity(bullet, 1f, WHITE);
				foreach(Enemy  enemy  in data.Enemies)		 DrawEntity(enemy, 3f,  WHITE);
				foreach(Particle particle in data.Particles) DrawParticle(particle, WHITE);

				// Draw UI;
				if (!IsGamepadAvailable(0)) DrawCrosshair(data, 10f, GREEN);
				DrawInGameUI(data);
			}
			else
            {
				DrawGameOverSreen(data);
            }

			EndTextureMode();

			// ------- SCENE DRAWING ------- //

			BeginDrawing();
			HideCursor();

			// Draw stary background.
			foreach (KeyValuePair<Vector2, float> Star in Stars)
				DrawCircleV(Star.Key + ScenePosition * 200f, Star.Value, WHITE);

			// 3D scene draw.
			ClearBackground(new Color(0, 0, 0, 0));

			BeginMode3D(Camera);

			UFORotation += PI / 40f;

			DrawModelEx(UFOModel, new Vector3(ScenePosition.X * 4f, 0f, 0f),
						new(0f, 1f, 0f), UFORotation, new(0.15f), WHITE);

			EndMode3D();

			// Game render texture draw.
			DrawTextureRec(GameRenderTexture.texture, new Rectangle(0f, 0f, GetScreenWidth(), -GetScreenHeight()), new(0), WHITE);

			EndDrawing();
		}

		public void HandleEvents(in List<GameEvent> events)
        {
			foreach(GameEvent e in events) if (e is PlayerLevelUp && Camera.fovy >= 40f) Camera.fovy -= 0.5f;
        }
	}
}
