using System;
using Raylib_cs;

namespace raylibTouhou
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create our window
            Raylib.InitWindow(800, 600, "RaylibTouhou");
            Raylib.SetTargetFPS(60);

            // Initalize the game
            Game.Init();

            while (!Raylib.WindowShouldClose())
            {
                Game.MainLoop();
            }

            Raylib.CloseWindow();
        }
    }
}
