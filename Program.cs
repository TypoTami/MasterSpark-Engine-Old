using System;
using Raylib_cs;
using ImGuiNET;

namespace raylibTouhou
{
    class Program
    {
        static ImguiController ImguiController = new ImguiController();
        public static string CurrentScene = "MENU";
        static void Main(string[] args)
        {
            // Create our window
            Raylib.InitWindow(800, 600, "RaylibDanmaku");
            ImguiController.Load(800, 600);
            
            Raylib.SetTargetFPS(60);

            // Initalize the menu
            Menu.Init();
         
            while (!Raylib.WindowShouldClose())
            {
                ImguiController.Update(Raylib.GetFrameTime());
                SubmitUI();

                Raylib.BeginDrawing();

                Game.MainLoop();

                ImguiController.Draw();

                Raylib.EndDrawing();
            }

            ImguiController.Dispose();
            Raylib.CloseWindow();
        }
        unsafe static void SubmitUI()
        {
            {
                ImGui.Text($"FPS: {Raylib.GetFPS()}\t FrameTime: {Raylib.GetFrameTime()}");

                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");
                
                ImGui.SliderFloat("bulletX", ref Game.bulletX, 0f, 800f);
                ImGui.SliderFloat("bulletY", ref Game.bulletY, 0f, 600f);
                ImGui.SliderFloat("bulletAngle", ref Game.bulletAngle, -180f, 180f);
                ImGui.SliderFloat("bulletVelocity", ref Game.bulletVelocity, 1.5f, 5f);
                ImGui.SliderInt("bulletN", ref Game.bulletN, 1, 15);
                ImGui.SliderFloat("bulletSpread", ref Game.bulletSpread, 0f, 1f);
                ImGui.SliderFloat("bulletAngular", ref Game.bulletAngular, -0.01f, 0.01f);
            }
        }
    }
}
