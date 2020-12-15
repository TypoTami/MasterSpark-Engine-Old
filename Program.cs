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
            Raylib.InitWindow(800, 600, "RaylibDanmaku");
            
            Raylib.SetTargetFPS(60);

            // Initalize the menu
            Menu.Init();
         
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                
                //  This is super jank, fix it.
                // switch (CurrentScene)
                // {
                //     case "MENU":
                //         Menu.MainLoop();
                //         break;
                //     case "GAME":
                //         Game.MainLoop();
                //         break;
                //     case "EDITOR":
                //         Editor.MainLoop();
                //         break;
                // }

                Game.MainLoop();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();
        }
    }
}
