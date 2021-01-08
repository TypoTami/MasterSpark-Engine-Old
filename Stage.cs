using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using NLua;
using ImGuiNET;

namespace MasterSpark
{
    class Stage
    {
        private string Name;
        public List<Entity> Entities = new List<Entity>();
        private Texture2D[] Sprites = { Raylib.LoadTexture("assets/Square.png"), Raylib.LoadTexture("assets/logo.png") };
        private Lua testScript = new Lua();
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
            
            testScript.LoadCLRPackage();
            testScript.DoFile("scripts/test.lua");
            testScript["Entities"] = Entities;
            // testScript["ScriptHelpers"] = new ScriptHelpers();
        }
        
        public void Update()
        {
            // testScript.GetFunction("Update").Call((float)Raylib.GetTime());
        }

        public void Draw()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Draw();
            }
        }

        public int EntitySelection = 2;
        public void SubmitUI()
        {
            if (ImGui.TreeNode($"Stage: {Name}"))
            {
                for (int i = 0; i < Entities.Count; i++)
                {
                    if (ImGui.Selectable($"Entity: {Entities[i].Name}", EntitySelection == i))
                    {
                        EntitySelection = i;
                    }
                }
                ImGui.TreePop();
            }
        }
    }
}