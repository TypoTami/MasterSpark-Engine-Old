using System;
using Raylib_cs;

namespace raylibTouhou
{
    static class Game
    {
        public static int frame = 0;
        static Player player;
        public static void Init()
        {
            // Create the player
            player = new Player("Reimu");
        }
        public static void MainLoop()
        {
            player.Update();
            Draw();
            frame++;
        }

        private static void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.GRAY);

            player.Draw();

            Raylib.EndDrawing();
        }
    }
}
