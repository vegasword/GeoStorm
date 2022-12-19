using System;
using System.Numerics;
using System.Collections.Generic;

using Raylib_cs;
using static Raylib_cs.Raylib;

using Geostorm.Core;

namespace Geostorm.Renderer
{
	public class Audio : IGameEventListener
	{
		// ------- PROPERTIES ------- //

		private List<Sound> Sfx;
		private Music		Music;

		// ------- CONSTRUCTOR ------ //

		public Audio(in float volume)
        {
			Raylib.InitAudioDevice();
			SetMasterVolume(volume);
        }

		// --------- METHODS -------- //

		public void Load()
        {
			Sfx = new();
			Sfx.Add(LoadSound("Assets/Audio/bounce.ogg"));
			Sfx.Add(LoadSound("Assets/Audio/dash.ogg"));
			Sfx.Add(LoadSound("Assets/Audio/explosion.ogg"));
			Sfx.Add(LoadSound("Assets/Audio/levelup.ogg"));
			Sfx.Add(LoadSound("Assets/Audio/shot.ogg"));

			// Daniel Deluxe - Firewall
			Music = LoadMusicStream("Assets/Audio/music.ogg");
			Music.looping = true;
			PlayMusicStream(Music);
		}

		public void Unload()
		{
			foreach(Sound sound in Sfx) UnloadSound(sound);
			Sfx.Clear();

			UnloadMusicStream(Music);
			Raylib.CloseAudioDevice();
		}

		public void Update()
        {
			UpdateMusicStream(Music);
        }

		// --------- HERITED -------- //

		public void HandleEvents(in List<GameEvent> events)
        {
			foreach(GameEvent e in events)
            {
				switch(e)
                {
					case PlayerBounced:	   PlaySoundMulti(Sfx[0]); break;
					case PlayerDashed:	   PlaySoundMulti(Sfx[1]); break;
					case EnemyKilledEvent: PlaySoundMulti(Sfx[2]); break;
					case PlayerLevelUp:    PlaySoundMulti(Sfx[3]); break;
					case PlayerShootEvent: PlaySoundMulti(Sfx[4]); break;
					default: break;
                }
            }
        }

	}
}
