using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace MasterSpark
{
    class Entity
    {
        public string Name;
        public Vector2 Position;
        private Texture2D Sprite;
        private float SpriteScale = 0.1f;
        private float SpriteAngle = 0.0f;
        private bool SkipDraw = false;
        // private Vector3 SpriteColour = new Vector3(321f, 0.6f, 1f);
        private Color SpriteColour = Raylib.ColorFromHSV(new Vector3(321f, 0.6f, 1f));
        public Entity(string name, Vector2 position, Texture2D sprite)
        {
            this.Name = name;
            this.Position = position;
            this.Sprite = sprite;
        }

        public void Draw()
        {
            if (!SkipDraw)
            {
                this.SpriteColour = Raylib.ColorFromHSV(new Vector3((float)Raylib.GetTime()*100f, 0.6f, 1f));
                Helpers.DrawSprite(this.Sprite, this.Position, this.SpriteScale, this.SpriteAngle, this.SpriteColour);
            }
        }

        public void Move(float x, float y)
        {
            this.Position.X += x;
            this.Position.Y += y;
        }

        public void SetPosition(float x, float y)
        {
            this.Position.X = x;
            this.Position.Y = y;
        }

        public void SubmitUI()
        {
            // ImGui.BeginChild($"Entity: {Name}", new Vector2(0, 200), true);
            // ImGui.EndChild();

            float windowPadX = ImGui.GetWindowWidth() - ImGui.GetWindowContentRegionMax().X;

            ImGui.Columns(2);
            ImGui.SetColumnWidth(0, ImGui.GetWindowContentRegionWidth() * 3f/4f);
            ImGui.Separator();
                ImGui.Text($"{this.Name}\n{this.Position}");
            ImGui.NextColumn();
                float nodeWidth = ImGui.GetColumnWidth() - windowPadX;
                ImGui.Image(
                    new IntPtr(Sprite.id),
                    new Vector2(nodeWidth, Sprite.height * (nodeWidth / Sprite.height)),
                    new Vector2(0, -1),
                    new Vector2(1, 0),
                    Raylib.ColorNormalize(SpriteColour)
                );
            ImGui.Columns(1);
            ImGui.Separator();

            ImGui.PushItemWidth(nodeWidth);
            ImGui.SliderFloat("X", ref this.Position.X, 0.0f, 800.0f);
            ImGui.SliderFloat("Y", ref this.Position.Y, 0.0f, 600.0f);
            ImGui.SliderFloat("Scale", ref this.SpriteScale, 0.0f, 2.0f);
            ImGui.SliderFloat("Angle", ref this.SpriteAngle, -180.0f, 180.0f);

            // ImGui.SliderFloat("H", ref this.SpriteColour.X, 0f, 360f);
            // ImGui.SliderFloat("S", ref this.SpriteColour.Y, 0f, 1f);
            // ImGui.SliderFloat("V", ref this.SpriteColour.Z, 0f, 1f);

            ImGui.Checkbox("SkipDraw", ref SkipDraw);
        }
    }
}