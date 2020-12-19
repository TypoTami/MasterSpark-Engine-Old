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
        static int TargetFPS = 60;
        static float TargetFrameTime = 1f/TargetFPS;
        public static float TimeScalar;
        static ImguiController ImguiController = new ImguiController();
        static RenderTexture2D FrameBuffer;
        static Queue<float> Frametimes = new Queue<float>();
        static IntPtr FrameBufferPointer;

        public static string CurrentScene = "MENU";
        static void Main(string[] args)
        {
            // Create our window
            Raylib.SetConfigFlags(ConfigFlag.FLAG_WINDOW_RESIZABLE);
            Raylib.InitWindow(WindowWidth, WindowHeight, "MasterSpark");
            ImguiController.Load(WindowWidth, WindowHeight);
            Raylib.SetTargetFPS(TargetFPS);

            // Initalize the menu
            Menu.Init();

            FrameBuffer = Raylib.LoadRenderTexture(GameWidth, GameHeight);
            FrameBufferPointer = new IntPtr(8);
            // Raylib.GenTextureMipmaps(ref FrameBuffer.texture);

            Texture2D logo = Raylib.LoadTexture("assets/logo.png");
            Raylib.GenTextureMipmaps(ref logo);
            
            while (!Raylib.WindowShouldClose())
            {
                Raylib.SetTargetFPS(TargetFPS);
                if (Settings.TimeWarp) { /* Raylib.SetTargetFPS(TargetFPS); */ TimeScalar = Raylib.GetFrameTime() / TargetFrameTime; }
                else { TimeScalar = 1f; }

                WindowWidth = Raylib.GetScreenWidth();
                WindowHeight = Raylib.GetScreenHeight();

                ImguiController.Resize(WindowWidth, WindowHeight);
                ImguiController.Update(Raylib.GetFrameTime());


                Raylib.BeginTextureMode(FrameBuffer);

                    Game.MainLoop();

                Raylib.EndTextureMode();

                SubmitUI();

                Raylib.BeginDrawing();
                    
                    Raylib.ClearBackground(Color.WHITE);
                    // Raylib.DrawTexture(FrameBuffer.texture, 0, 0, Color.WHITE);
                    Raylib.DrawTextureEx(logo, new Vector2(0, Raylib.GetScreenHeight() - logo.height*0.25f), 0f, 0.25f, Color.WHITE);
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

                int ptr = (int)FrameBufferPointer;

                ImGui.SliderInt("PTR", ref ptr, 1, 15);

                FrameBufferPointer = new IntPtr(ptr);

                ImGui.Image(
                    FrameBufferPointer,
                    new Vector2(width, height),
                    new Vector2(0, 1),
                    new Vector2(1, 0)
                );
                ImGui.End();
            }

            {
                Frametimes.Enqueue(Raylib.GetFrameTime());
                if (Frametimes.Count > 100) { Frametimes.Dequeue(); }
                float[] frameArray = Frametimes.ToArray();

                ImGui.Begin("Performance");

                ImGui.Text($"FPS: {Raylib.GetFPS()}\t FrameTime: {TargetFrameTime}/{Raylib.GetFrameTime()}");

                ImGui.PlotLines("", ref frameArray[0], frameArray.Length);
                ImGui.PlotLines("", ref frameArray[0], frameArray.Length, 0, null, 0f, 0.1f, new Vector2(0, 80));

                ImGui.Text($"TimeScalar: {TimeScalar}");
                ImGui.Text($"{Raylib.GetTime()}");

                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");

                ImGui.SliderInt("TargetFPS", ref TargetFPS, 15, 1000);

                Game.SubmitUI();

                ImGui.End();
            }

            {
                ImGui.Begin("Settings");
                    Settings.SubmitUI();
                ImGui.End();
            }

            {
                ImGui.ShowDemoWindow();
            }

            {
                ImGui.BeginMainMenuBar();
                    bool Test = ImGui.MenuItem("Test");
                    bool About = ImGui.MenuItem("About");
                ImGui.EndMainMenuBar();
            }

            {
                ImGui.Begin("StageTree");
                    Game.CurrentStage.SubmitUI();
                ImGui.End();
            }
        }
    }
}
