using System;
using Raylib_cs;

namespace raylibTouhou
{
    static class Editor
    {
        public static void Init()
        {
        }

        public static void MainLoop()
        {
            Draw();
        }

        private static void Draw()
        {
            Raylib.ClearBackground(Color.WHITE);
            
            Raylib.DrawText("This is the editor!", 190, 200, 20, Color.BLACK);
        }
    }
}