using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace raylibTouhou
{
    class Entity
    {
        public string Name;
        public Vector2 Position;
        private Texture2D Sprite;
        private float SpriteScale = 1.0f;
        private float SpriteAngle = 0.0f;
        private bool SkipDraw = false;
        private Vector3 SpriteColour = new Vector3(321f, 0.6f, 1f);
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
                this.SpriteColour.X = (float)Raylib.GetTime()*100f;
                Helpers.DrawSprite(this.Sprite, this.Position, this.SpriteScale, this.SpriteAngle, Raylib.ColorFromHSV(this.SpriteColour));
            }
        }

        public void SubmitUI()
        {
            if (ImGui.TreeNode($"Entity: {Name}"))
            {
                ImGui.BeginChild($"Entity: {Name}", new Vector2(0, 200), true);
                    float nodeWidth = ImGui.GetColumnWidth() * 0.75f;

                    ImGui.PushItemWidth(nodeWidth);
                    ImGui.SliderFloat("X", ref this.Position.X, 0.0f, 800.0f);
                    ImGui.SliderFloat("Y", ref this.Position.Y, 0.0f, 600.0f);
                    ImGui.SliderFloat("Scale", ref this.SpriteScale, 0.0f, 2.0f);
                    ImGui.SliderFloat("Angle", ref this.SpriteAngle, -180.0f, 180.0f);

                    ImGui.SliderFloat("H", ref this.SpriteColour.X, 0f, 360f);
                    ImGui.SliderFloat("S", ref this.SpriteColour.Y, 0f, 1f);
                    ImGui.SliderFloat("V", ref this.SpriteColour.Z, 0f, 1f);


                    ImGui.Image(
                        new IntPtr(Sprite.id),
                        new Vector2(nodeWidth, Sprite.height * (nodeWidth / Sprite.height))
                    );

                    ImGui.Checkbox("SkipDraw", ref SkipDraw);
                ImGui.EndChild();

                ImGui.TreePop();
            }
        }
    }
}