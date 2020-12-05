using System;
using Raylib_cs;

namespace raylibTouhou
{
    class Program
    {
        public static string CurrentScene = "MENU";
        static void Main(string[] args)
        {
            // Create our window
            Raylib.InitWindow(800, 600, "RaylibTouhou");
            
            Raylib.SetTargetFPS(60);

            // Initalize the menu
            Menu.Init();
         
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                
                switch (CurrentScene)
                {
                    case "MENU":
                        Menu.MainLoop();
                        break;
                    case "GAME":
                        Game.MainLoop();
                        break;
                    case "EDITOR":
                        Editor.MainLoop();
                        break;
                }

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
