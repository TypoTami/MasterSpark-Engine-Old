using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace MasterSpark
{
    class RenderController
    {
        public Camera2D Camera = new Camera2D();
        public RenderTexture2D FrameBuffer;
        private bool UseBuffer;
        public float RenderScale;
        private float PrevRenderScale = 0f;
        private Vector2 BaseSize;
        private Vector2 RenderSize;
        public Vector2 DestSize;

        public RenderController(Vector2 baseSize, Vector2 destSize, float renderScale, bool useBuffer = true)
        {
            this.BaseSize = baseSize;
            this.DestSize = destSize;
            this.RenderScale = renderScale;
            this.UseBuffer = useBuffer;

            this.RenderSize = Vector2.Multiply(this.BaseSize, RenderScale);

            if (UseBuffer) { this.FrameBuffer = Raylib.LoadRenderTexture((int)this.RenderSize.X, (int)this.RenderSize.Y); }
            
            Update();
        }

        ~RenderController()
        {
            if (UseBuffer) { Raylib.UnloadRenderTexture(this.FrameBuffer); }
        }

        // public void ChangeDest

        private void Update()
        {
            if (this.PrevRenderScale != this.RenderScale)
            {
                this.RenderSize = Vector2.Multiply(this.BaseSize, RenderScale);

                this.Camera.zoom = this.RenderSize.Y / this.BaseSize.Y;

                // Raylib.UnloadRenderTexture(this.FrameBuffer);
                // this.FrameBuffer = Raylib.LoadRenderTexture((int)this.RenderSize.X, (int)this.RenderSize.Y);
            }

            this.PrevRenderScale = this.RenderScale;

        }

        public void Begin()
        {
            Update();

            if (UseBuffer) { Raylib.BeginTextureMode(this.FrameBuffer); }
            
            Raylib.BeginMode2D(this.Camera);
        }

        public void End()
        {
            Raylib.EndMode2D();

            if (UseBuffer) { Raylib.EndTextureMode(); }
        }

        public void Draw()
        {
            float width;
            float height;

            if (this.DestSize.X > this.DestSize.Y)
            {
                width = this.DestSize.Y / (this.BaseSize.Y/this.BaseSize.X);
                height = this.DestSize.Y;
            }
            else
            {
                width = this.DestSize.X;
                height = this.DestSize.X / (this.BaseSize.X/this.BaseSize.Y);
            }

            if (UseBuffer) 
            {
                Raylib.DrawTexturePro(
                    this.FrameBuffer.texture,
                    new Rectangle(0f, 0f, this.FrameBuffer.texture.width, -this.FrameBuffer.texture.height),
                    new Rectangle(0f, 0f, width, height),
                    new Vector2(0f, 0f),
                    0f,
                    Color.WHITE
                );
            }
        }

        public void SubmitUI()
        {
            // ImGui.SliderInt("RenderHeight", ref Program.RenderHeight, 600, 1080*4);
            // Program.RenderWidth = Convert.ToInt32(Program.RenderHeight / 0.75f);
            // Program.camera.zoom = Program.RenderHeight/600f;
            
            ImGui.Text($"{this.BaseSize}");
            ImGui.Text($"{this.DestSize}");
            ImGui.Text($"{this.RenderSize}");

            // ImGui.SliderFloat("Angle", ref Program.camera.rotation, -360f, 360f);
            // ImGui.SliderFloat("X", ref Program.camera.target.X, -800f, 800f);
            // ImGui.SliderFloat("Y", ref Program.camera.target.Y, -800f, 800f);
        }
    }
}