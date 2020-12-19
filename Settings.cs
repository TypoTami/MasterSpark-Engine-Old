using System;
using ImGuiNET;

namespace raylibTouhou
{
    public static class Settings
    {
        public static bool TimeWarp = false;

        public static void SubmitUI()
        {
            ImGui.Checkbox("TimeWarp", ref TimeWarp);
        }
    }
}