using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace MasterSpark
{
    class Program
    {
        public static RenderController MainCamera;
        static int WindowWidth = 1280;
        static int WindowHeight = 720;
        static int TargetFPS = 60;
        static ImguiController ImguiController = new ImguiController();
        static Texture2D logo;
        static bool ImGuiVisible = false;
        static void Main(string[] args)
        {
            Init();
            
            while (!Raylib.WindowShouldClose())
            {
                Update();

                Draw();
            }

            Console.IsWindowClosed = true;
            ImguiController.Dispose();
            Raylib.CloseWindow();
        }

        public static void Init()
        {
            Raylib.SetTraceLogCallback(Console.RaylibLog);

            // Create our window
            Raylib.SetConfigFlags(
                ConfigFlag.FLAG_WINDOW_RESIZABLE |
                ConfigFlag.FLAG_MSAA_4X_HINT |
                ConfigFlag.FLAG_VSYNC_HINT
            );
            // Raylib.SetConfigFlags(ConfigFlag.FLAG_MSAA_4X_HINT);
            Raylib.InitWindow(WindowWidth, WindowHeight, "MasterSpark");
            ImguiController.Load(WindowWidth, WindowHeight);
            Raylib.SetTargetFPS(TargetFPS);

            logo = Raylib.LoadTexture("assets/logo.png");
            Raylib.GenTextureMipmaps(ref logo);

            MainCamera = new RenderController(
                new Vector2(1280, 720),
                new Vector2(1280, 720),
                4f
            );

            Game.Init();
        }

        public static void Update()
        {
            WindowWidth = Raylib.GetScreenWidth();
            WindowHeight = Raylib.GetScreenHeight();

            ImguiController.Resize(WindowWidth, WindowHeight);
            ImguiController.Update(Raylib.GetFrameTime());

            Game.Update();
            MainCamera.DestSize = new Vector2(
                Raylib.GetScreenWidth(), Raylib.GetScreenHeight()
            );

            if (ImGuiVisible)
            {
                GUI.SubmitUI();
            }

            if (Raylib.IsKeyPressed(KeyboardKey.KEY_F10)) { ImGuiVisible = !ImGuiVisible;}

        }

        public static void Draw()
        {
            MainCamera.Begin();

                Raylib.ClearBackground(Color.SKYBLUE);

                Game.Draw();

            MainCamera.End();

            Raylib.BeginDrawing();
                
                Raylib.ClearBackground(Color.WHITE);

                MainCamera.Draw();

                if(ImGuiVisible) { Raylib.DrawTextureEx(logo, new Vector2(0, Raylib.GetScreenHeight() - logo.height*0.25f), 0f, 0.25f, Color.WHITE); }

                ImguiController.Draw();

            Raylib.EndDrawing();
        }
    }
}
