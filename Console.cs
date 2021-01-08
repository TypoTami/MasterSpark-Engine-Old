using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace MasterSpark
{
    static class Console
    {
        public static bool IsWindowClosed = false;
        private static List<(float Time, string Source, string LogType, string Content)> LogContents =
            new List<(float Time, string Source, string LogType, string Content)>();
        public static void RaylibLog(TraceLogType logType, string text, IntPtr args)
        {
            if (!IsWindowClosed)
            {
                string logMessage = RaylibInterop.GetLogMessage(RaylibInterop.ToUtf8(text), args);
                float time = (float)Raylib.GetTime();
                LogContents.Add(
                    (time, "Raylib", logType.ToString(), logMessage)
                );
            }
        }

        public static void Log(string logType, string logMessage)
        {
            if (!IsWindowClosed)
            {
                float time = (float)Raylib.GetTime();
                LogContents.Add(
                    (time, "Game", logType, logMessage)
                );
            }
        }

        private static bool AutoScroll = true;
        public static void SubmitUI()
        {
            Vector4 textColour;

            ImGui.Checkbox("AutoScroll", ref AutoScroll);
            ImGui.Separator();
            ImGui.BeginChild("scrolling", new Vector2(0, 0), false, ImGuiWindowFlags.HorizontalScrollbar);
                for (int i = 0; i < LogContents.Count; i++)
                {
                    switch (LogContents[i].LogType)
                    {
                        case "LOG_INFO":
                        {
                            textColour = Raylib.ColorNormalize(Color.BLUE);
                            break;
                        }
                        case "LOG_WARNING":
                        {
                            textColour = Raylib.ColorNormalize(Color.YELLOW);
                            break;
                        }
                        default:
                        {
                            textColour = new Vector4(1f, 1f, 1f, 1f);
                            break;
                        }
                    }

                    ImGui.Columns(4);
                    ImGui.SetColumnWidth(0, 100f);
                    ImGui.SetColumnWidth(1, 100f);
                    ImGui.SetColumnWidth(2, 100f);
                        ImGui.Text($"{LogContents[i].Time:0000.0000}");
                    ImGui.NextColumn();
                        ImGui.Text(LogContents[i].Source);
                    ImGui.NextColumn();
                        ImGui.TextColored(textColour, LogContents[i].LogType);
                    ImGui.NextColumn();
                        ImGui.TextColored(textColour, LogContents[i].Content);
                    ImGui.Columns(1);
                }

                if (AutoScroll && ImGui.GetScrollY() <= ImGui.GetScrollMaxY())
                {
                    ImGui.SetScrollHereY(1f);
                }
            ImGui.EndChild();
        }
    }
}