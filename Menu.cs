using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    static class Menu
    {
        private static string[] MainMenu = new string[] {"Game", "Editor", "Options", "Lorem", "Ipsum"};
        private static int CurrentSelection = 0;
        private static Vector2 origin = new Vector2(100, 150);
        private static bool IsSelectionComfirmed;

        public static void Init()
        {
            // Initalize the game
            Game.Init();
        }

        public static void MainLoop()
        {
            if (IsSelectionComfirmed)
            {
                switch (MainMenu[CurrentSelection])
                {
                    case "Game":
                        Program.CurrentScene = "GAME";
                        break;
                    case "Editor":
                        Program.CurrentScene = "EDITOR";
                        break;
                }
            }

            Update();
            Draw();
            //Game.MainLoop();
        }

        private static void Update()
        {
            IsSelectionComfirmed = false;

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_DOWN))
            {
                CurrentSelection++;
            } 
            else if (Raylib.IsKeyPressed(KeyboardKey.KEY_UP))
            {
                CurrentSelection--;
            }

            if (CurrentSelection > MainMenu.Length - 1)
            {
                CurrentSelection = MainMenu.Length - 1;
            }
            else if (CurrentSelection < 0)
            {
                CurrentSelection = 0;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                IsSelectionComfirmed = true;
            }
        }

        private static void Draw()
        {
            Raylib.ClearBackground(Color.WHITE);

            Color textColour;

            for (int i = 0; MainMenu.Length > i; i++)
            {
                if (i == CurrentSelection)
                {
                    textColour = Color.RED;
                }
                else
                {
                    textColour = Color.BLACK;
                }
                Raylib.DrawText(
                    MainMenu[i],
                    Convert.ToInt32(origin.X),
                    Convert.ToInt32(origin.Y + (i * 20)),
                    20,
                    textColour
                );
            }

            Raylib.DrawText(MainMenu[CurrentSelection], 10, 10, 10, Color.RED);
        }
    }
}