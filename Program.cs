using System;
using System.Numerics;
using Raylib_cs;

using Geostorm.Core;
using Geostorm.Renderer;

namespace Geostorm
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            Graphics   scene   = new(new Vector2(1280f, 720f), "GeoStorm", true);
            GameInputs inputs  = new();
            Game       game    = new(inputs, 0);
            Audio      audio   = new(0.1f);

            audio.Load();

            while (!Raylib.WindowShouldClose())
            {
                game.Update(ref inputs, ref scene, ref audio);
                game.Render(scene);
            }
            
            audio.Unload();
            scene.Unload();
        }
    }
}
