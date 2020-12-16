using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace raylibTouhou
{
    class Program
    {
        const int GameWidth = 800;
        const int GameHeight = 600; 
        static int WindowWidth = 1280;
        static int WindowHeight = 720;
        static ImguiController ImguiController = new ImguiController();
        static RenderTexture2D FrameBuffer;
        static IntPtr FrameBufferPointer;

        public static string CurrentScene = "MENU";
        static void Main(string[] args)
        {
            // Create our window
            Raylib.SetConfigFlags(ConfigFlag.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(WindowWidth, WindowHeight, "RaylibDanmaku");
            ImguiController.Load(WindowWidth, WindowHeight);
            
            Raylib.SetTargetFPS(60);

            // Initalize the menu
            Menu.Init();

            FrameBuffer = Raylib.LoadRenderTexture(GameWidth, GameHeight);
            FrameBufferPointer = new IntPtr(6);
            
            while (!Raylib.WindowShouldClose())
            {
                WindowWidth = Raylib.GetScreenWidth();
                WindowHeight = Raylib.GetScreenHeight();

                ImguiController.Resize(WindowWidth, WindowHeight);
                ImguiController.Update(Raylib.GetFrameTime());


                Raylib.BeginTextureMode(FrameBuffer);

                    Game.MainLoop();

                Raylib.EndTextureMode();

                Raylib.GenTextureMipmaps(ref FrameBuffer.texture);

                SubmitUI();

                Raylib.BeginDrawing();
                    
                    Raylib.ClearBackground(Color.PURPLE);
                    Raylib.DrawTexture(FrameBuffer.texture, 0, 0, Color.WHITE);
                    ImguiController.Draw();

                Raylib.EndDrawing();
            }

            Raylib.UnloadRenderTexture(FrameBuffer);
            ImguiController.Dispose();
            Raylib.CloseWindow();
        }
        unsafe static void SubmitUI()
        {
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());
            {
                ImGui.Begin("Bullets!");

                ImGui.SliderFloat("bulletX", ref Game.bulletX, 0f, 800f);
                ImGui.SliderFloat("bulletY", ref Game.bulletY, 0f, 600f);
                ImGui.SliderFloat("bulletAngle", ref Game.bulletAngle, -180f, 180f);
                ImGui.SliderFloat("bulletVelocity", ref Game.bulletVelocity, 1.5f, 5f);
                ImGui.SliderInt("bulletN", ref Game.bulletN, 1, 15);
                ImGui.SliderFloat("bulletSpread", ref Game.bulletSpread, 0f, 1f);
                ImGui.SliderFloat("bulletAngular", ref Game.bulletAngular, -0.01f, 0.01f);

                ImGui.End();
            }

            {
                ImGui.Begin("FrameBuffer");

                float width;
                float height;

                // float contentWidth = ImGui.GetWindowWidth();
                // float contentHeight = ImGui.GetWindowHeight();

                Vector2 contentMin = ImGui.GetWindowContentRegionMin();
                Vector2 contentMax = ImGui.GetWindowContentRegionMax();
                float contentWidth = contentMax.X - contentMin.X;
                float contentHeight = contentMax.Y - contentMin.Y;

                if (contentHeight > contentWidth * 0.75f) {
                    width = contentWidth;
                    height = width * 3/4;
                } 
                else {
                    height = contentHeight;
                    width = height * 4/3;
                }

                // int ptr = (int)FrameBufferPointer;

                // ImGui.SliderInt("PTR", ref ptr, 1, 15);

                // FrameBufferPointer = new IntPtr(ptr);

                ImGui.Image(
                    FrameBufferPointer,
                    new Vector2(width, height),
                    new Vector2(0, 1),
                    new Vector2(1, 0)
                );
                ImGui.End();
            }

            {
                ImGui.Begin("Performance");

                ImGui.Text($"FPS: {Raylib.GetFPS()}\t FrameTime: {Raylib.GetFrameTime()}");

                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");

                Game.SubmitUI();

                ImGui.End();
            }
        }
    }
}
