using System;
using System.Numerics;
using System.Collections.Generic;
using ImGuiNET;
using Raylib_cs;

namespace MasterSpark
{
    class GUI
    {
        static Queue<float> Frametimes = new Queue<float>();
        static Queue<float> GCHist = new Queue<float>();
        static int LastGC = 0;
        public static IntPtr FrameBufferPointer = new IntPtr(8);
        static bool FlipFrameBuffer = true;
        public unsafe static void SubmitUI()
        {
            ImGui.DockSpaceOverViewport(ImGui.GetMainViewport());

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

                ImGui.SliderInt("PTR", ref ptr, 1, 15); ImGui.SameLine();
                ImGui.Checkbox("Flip", ref FlipFrameBuffer);

                FrameBufferPointer = new IntPtr(ptr);

                if (FlipFrameBuffer)
                {
                    ImGui.Image(
                        FrameBufferPointer,
                        new Vector2(width, height),
                        new Vector2(0, 1),
                        new Vector2(1, 0)
                    );
                }
                else
                {
                    ImGui.Image(
                        FrameBufferPointer,
                        new Vector2(width, height),
                        new Vector2(0, -1),
                        new Vector2(1, 0)
                    );
                }

                
                ImGui.End();
            }

            {
                var memInfo =  GC.GetGCMemoryInfo();
                if (memInfo.Index > LastGC)
                {
                    LastGC = (int)memInfo.Index;
                    
                    GCHist.Enqueue(memInfo.Generation + 1);
                }
                else
                {
                    GCHist.Enqueue(0f);
                }

                Frametimes.Enqueue(Raylib.GetFrameTime());
                if (Frametimes.Count > 100) { Frametimes.Dequeue(); GCHist.Dequeue(); }
                float[] frameArray = Frametimes.ToArray();
                float[] GCHistArray = GCHist.ToArray();

                ImGui.Begin("Performance");


                ImGui.PlotLines("", ref frameArray[0], frameArray.Length, 0, "", 0.016665f, 0.01667f, new Vector2(0, 60));
                ImGui.PlotHistogram("", ref GCHistArray[0], GCHistArray.Length, 0,"", 0f, 3f, new Vector2(0, 20));

                ImGui.Text($"{System.GC.GetTotalMemory(false) / 1000000f:00.00}MB");

                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");

                Game.SubmitUI();

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

            {
                ImGui.Begin("EntityInspector");
                    if (Game.CurrentStage.EntitySelection > -1)
                    {
                        Game.CurrentStage.Entities[Game.CurrentStage.EntitySelection].SubmitUI();
                    }
                ImGui.End();
            }

            {
                ImGui.Begin("CameraController");
                    Program.MainCamera.SubmitUI();
                    Game.GameCamera.SubmitUI();
                ImGui.End();
            }

            {
                ImGui.Begin("Console");
                    Console.SubmitUI();
                ImGui.End();
            }
        }
    }
}