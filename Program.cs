using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace raylibTouhou
{
    class Program
    {
        public static int RenderHeight = 1080; 
        public static int RenderWidth = Convert.ToInt32(RenderHeight / 0.75f);
        public static Camera2D camera = new Camera2D();
        static int WindowWidth = 1280;
        static int WindowHeight = 720;
        static int TargetFPS = 60;
        static ImguiController ImguiController = new ImguiController();
        static RenderTexture2D FrameBuffer;
        static bool ImGuiVisible = true;
        public static string CurrentScene = "MENU";
        static void Main(string[] args)
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

            //
            // camera.target = new Vector2(100, 200);
            camera.zoom = RenderHeight/600;

            // Initalize the menu
            Menu.Init();

            int previousHeight = RenderHeight;

            FrameBuffer = Raylib.LoadRenderTexture(RenderWidth, RenderHeight);

            Texture2D logo = Raylib.LoadTexture("assets/logo.png");
            Raylib.GenTextureMipmaps(ref logo);
            
            while (!Raylib.WindowShouldClose())
            {
                // Raylib.SetTargetFPS(TargetFPS);
                // if (Settings.TimeWarp) { /* Raylib.SetTargetFPS(TargetFPS); */ TimeScalar = Raylib.GetFrameTime() / TargetFrameTime; }
                // else { TimeScalar = 1f; }

                WindowWidth = Raylib.GetScreenWidth();
                WindowHeight = Raylib.GetScreenHeight();

                ImguiController.Resize(WindowWidth, WindowHeight);
                ImguiController.Update(Raylib.GetFrameTime());

                Raylib.BeginTextureMode(FrameBuffer);
                
                    Raylib.BeginMode2D(camera);

                        Game.MainLoop();

                    Raylib.EndMode2D();

                Raylib.EndTextureMode();

                if (ImGuiVisible) { GUI.SubmitUI(); }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_F10)) { ImGuiVisible = !ImGuiVisible;}

                Raylib.BeginDrawing();
                    
                    Raylib.ClearBackground(Color.WHITE);
                    // Raylib.DrawTexture(FrameBuffer.texture, 0, 0, Color.WHITE);
                    // Raylib.DrawTextureEx(FrameBuffer.texture, new Vector2(0, 0), 0f, Raylib.GetScreenHeight()/(float)RenderHeight, Color.WHITE);

                    // DrawTexturePro(Texture2D texture, Rectangle sourceRec, Rectangle destRec, Vector2 origin, float rotation, Color tint);
                    // (Rectangle){ (GetScreenWidth() - ((float)gameScreenWidth*scale))*0.5, (GetScreenHeight() - ((float)gameScreenHeight*scale))*0.5,

                    Raylib.DrawTexturePro(
                        FrameBuffer.texture,
                        new Rectangle(0f, 0f, FrameBuffer.texture.width, -FrameBuffer.texture.height),
                        new Rectangle(0f, 0f, Raylib.GetScreenHeight()/0.75f, Raylib.GetScreenHeight()),
                        new Vector2(
                            -(Raylib.GetScreenWidth()/2f - (Raylib.GetScreenHeight()/0.75f)/2f),
                            0f
                        ),
                        0f,
                        Color.WHITE
                    );

                    if(ImGuiVisible) { Raylib.DrawTextureEx(logo, new Vector2(0, Raylib.GetScreenHeight() - logo.height*0.25f), 0f, 0.25f, Color.WHITE); }

                    ImguiController.Draw();

                Raylib.EndDrawing();

                if (previousHeight != RenderHeight)
                {
                    Raylib.UnloadRenderTexture(FrameBuffer);
                    FrameBuffer = Raylib.LoadRenderTexture(RenderWidth, RenderHeight);
                    previousHeight = RenderHeight;
                }
            }

            Console.IsWindowClosed = true;
            Raylib.UnloadRenderTexture(FrameBuffer);
            ImguiController.Dispose();
            Raylib.CloseWindow();
        }
    }
}
