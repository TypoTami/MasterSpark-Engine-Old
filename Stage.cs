using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace raylibTouhou
{
    class Stage
    {
        private string Name;
        private List<Entity> Entities = new List<Entity>();
        private Texture2D[] Sprites = { Raylib.LoadTexture("assets/dagger.png"), Raylib.LoadTexture("assets/logo.png") };

        public Stage(string name)
        {
            this.Name = name;
            
            for (int i = 0; i < 5; i++)
            {
                Entities.Add(
                    new Entity(
                        $"Entity.{i}",
                        new Vector2(100 + (75 * i), 300),
                        Sprites[0]
                    )
                );
            }

            for (int i = 0; i < Sprites.Length; i++)
            {
                Raylib.GenTextureMipmaps(ref Sprites[i]);
            }
        }

        public void Draw()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Draw();
            }
        }

        public void SubmitUI()
        {
            if (ImGui.TreeNode($"Stage: {Name}"))
            {
                for (int i = 0; i < Entities.Count; i++)
                {
                    Entities[i].SubmitUI();
                }
                // ImGui.TreePop();
            }
            ImGui.TreePop();
        }
    }
}