using System;
using System.Numerics;
using Raylib_cs;
using ImGuiNET;

// ImguiController from https://github.com/ChrisDill/Raylib-cs-Examples

namespace raylibTouhou
{
    /// <summary>
    /// ImGui controller for Raylib-cs.
    /// </summary>
    public class ImguiController : IDisposable
    {
        IntPtr context;
        Texture2D fontTexture;
        Vector2 size;
        Vector2 scaleFactor = Vector2.One;

        public ImguiController()
        {
            context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            ImGuiIOPtr io = ImGui.GetIO();
            // io.Fonts.AddFontDefault();
            // io.Fonts.AddFontFromFileTTF("assets/Px437_IBM_VGA_8x16.ttf", 16.0f);
            io.Fonts.AddFontFromFileTTF("assets/Roboto-Regular.ttf", 18.0f);
            io.ConfigFlags = ImGuiConfigFlags.DockingEnable;
            io.ConfigDockingWithShift = true;
        }

        public void Dispose()
        {
            ImGui.DestroyContext(context);
            Raylib.UnloadTexture(fontTexture);
        }

        /// <summary>
        /// Creates a texture and loads the font data from ImGui.
        /// </summary>
        public void Load(int width, int height)
        {
            size = new Vector2(width, height);
            LoadFontTexture();
            SetupInput();
            SetupStyle();
            ImGui.NewFrame();
        }

        public void Resize(int width, int height)
        {
            size = new Vector2(width, height);
        }

        unsafe void LoadFontTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();

            // Load as RGBA 32-bit (75% of the memory is wasted, but default font is so small) because it is more likely to be compatible with user's existing shaders.
            // If your ImTextureId represent a higher-level concept than just a GL texture id, consider calling GetTexDataAsAlpha8() instead to save on GPU memory.
            io.Fonts.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height);

            // Upload texture to graphics system
            IntPtr data = new IntPtr(pixels);
            Image image = Raylib.LoadImagePro(data, width, height, (int)PixelFormat.UNCOMPRESSED_R8G8B8A8);
            fontTexture = Raylib.LoadTextureFromImage(image);
            Raylib.UnloadImage(image);

            // Store our identifier
            io.Fonts.SetTexID(new IntPtr(fontTexture.id));

            // Clears font data on the CPU side
            io.Fonts.ClearTexData();
        }

        void SetupStyleCorpGray()
        {
            var style = ImGui.GetStyle();
            var colors = style.Colors;
            
            /// 0 = FLAT APPEARENCE
            /// 1 = MORE "3D" LOOK
            int is3D = 0;

            // ImGuiCol.Text
                
            colors[(int)ImGuiCol.Text]                   = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled]           = new Vector4(0.40f, 0.40f, 0.40f, 1.00f);
            colors[(int)ImGuiCol.ChildBg]                = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.WindowBg]               = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.PopupBg]                = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.Border]                 = new Vector4(0.12f, 0.12f, 0.12f, 0.71f);
            colors[(int)ImGuiCol.BorderShadow]           = new Vector4(1.00f, 1.00f, 1.00f, 0.06f);
            colors[(int)ImGuiCol.FrameBg]                = new Vector4(0.42f, 0.42f, 0.42f, 0.54f);
            colors[(int)ImGuiCol.FrameBgHovered]         = new Vector4(0.42f, 0.42f, 0.42f, 0.40f);
            colors[(int)ImGuiCol.FrameBgActive]          = new Vector4(0.56f, 0.56f, 0.56f, 0.67f);
            colors[(int)ImGuiCol.TitleBg]                = new Vector4(0.19f, 0.19f, 0.19f, 1.00f);
            colors[(int)ImGuiCol.TitleBgActive]          = new Vector4(0.22f, 0.22f, 0.22f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed]       = new Vector4(0.17f, 0.17f, 0.17f, 0.90f);
            colors[(int)ImGuiCol.MenuBarBg]              = new Vector4(0.335f, 0.335f, 0.335f, 1.000f);
            colors[(int)ImGuiCol.ScrollbarBg]            = new Vector4(0.24f, 0.24f, 0.24f, 0.53f);
            colors[(int)ImGuiCol.ScrollbarGrab]          = new Vector4(0.41f, 0.41f, 0.41f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered]   = new Vector4(0.52f, 0.52f, 0.52f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive]    = new Vector4(0.76f, 0.76f, 0.76f, 1.00f);
            colors[(int)ImGuiCol.CheckMark]              = new Vector4(0.65f, 0.65f, 0.65f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab]             = new Vector4(0.52f, 0.52f, 0.52f, 1.00f);
            colors[(int)ImGuiCol.SliderGrabActive]       = new Vector4(0.64f, 0.64f, 0.64f, 1.00f);
            colors[(int)ImGuiCol.Button]                 = new Vector4(0.54f, 0.54f, 0.54f, 0.35f);
            colors[(int)ImGuiCol.ButtonHovered]          = new Vector4(0.52f, 0.52f, 0.52f, 0.59f);
            colors[(int)ImGuiCol.ButtonActive]           = new Vector4(0.76f, 0.76f, 0.76f, 1.00f);
            colors[(int)ImGuiCol.Header]                 = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
            colors[(int)ImGuiCol.HeaderHovered]          = new Vector4(0.47f, 0.47f, 0.47f, 1.00f);
            colors[(int)ImGuiCol.HeaderActive]           = new Vector4(0.76f, 0.76f, 0.76f, 0.77f);
            colors[(int)ImGuiCol.Separator]              = new Vector4(0.000f, 0.000f, 0.000f, 0.137f);
            colors[(int)ImGuiCol.SeparatorHovered]       = new Vector4(0.700f, 0.671f, 0.600f, 0.290f);
            colors[(int)ImGuiCol.SeparatorActive]        = new Vector4(0.702f, 0.671f, 0.600f, 0.674f);
            colors[(int)ImGuiCol.ResizeGrip]             = new Vector4(0.26f, 0.59f, 0.98f, 0.25f);
            colors[(int)ImGuiCol.ResizeGripHovered]      = new Vector4(0.26f, 0.59f, 0.98f, 0.67f);
            colors[(int)ImGuiCol.ResizeGripActive]       = new Vector4(0.26f, 0.59f, 0.98f, 0.95f);
            colors[(int)ImGuiCol.PlotLines]              = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered]       = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram]          = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered]   = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg]         = new Vector4(0.73f, 0.73f, 0.73f, 0.35f);
            colors[(int)ImGuiCol.ModalWindowDimBg]       = new Vector4(0.80f, 0.80f, 0.80f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget]         = new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight]           = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight]  = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg]      = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);

            style.PopupRounding = 3;

            style.WindowPadding = new Vector2(4, 4);
            style.FramePadding  = new Vector2(6, 4);
            style.ItemSpacing   = new Vector2(6, 2);

            style.ScrollbarSize = 18;

            style.WindowBorderSize = 1;
            style.ChildBorderSize  = 1;
            style.PopupBorderSize  = 1;
            style.FrameBorderSize  = is3D; 

            style.WindowRounding    = 3;
            style.ChildRounding     = 3;
            style.FrameRounding     = 3;
            style.ScrollbarRounding = 2;
            style.GrabRounding      = 3;

            
            style.TabBorderSize = is3D; 
            style.TabRounding   = 3;

            colors[(int)ImGuiCol.DockingEmptyBg]     = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
            colors[(int)ImGuiCol.Tab]                = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.TabHovered]         = new Vector4(0.40f, 0.40f, 0.40f, 1.00f);
            colors[(int)ImGuiCol.TabActive]          = new Vector4(0.33f, 0.33f, 0.33f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused]       = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocusedActive] = new Vector4(0.33f, 0.33f, 0.33f, 1.00f);
            colors[(int)ImGuiCol.DockingPreview]     = new Vector4(0.85f, 0.85f, 0.85f, 0.28f);

            if (((uint)ImGui.GetIO().ConfigFlags & (uint)ImGuiConfigFlags.ViewportsEnable) >= 1)
            {
                style.WindowRounding = 0.0f;
                style.Colors[(int)ImGuiCol.WindowBg].W = 1.0f;
            }
        }
        
        void SetupStyle()
        {
            ImGui.GetStyle().FrameRounding = 4.0f;
            ImGui.GetStyle().GrabRounding = 4.0f;
            
            var colors = ImGui.GetStyle().Colors;

            colors[(int)ImGuiCol.Text]= new Vector4(0.95f, 0.96f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled]= new Vector4(0.36f, 0.42f, 0.47f, 1.00f);
            colors[(int)ImGuiCol.WindowBg]= new Vector4(0.11f, 0.15f, 0.17f, 0.94f);
            colors[(int)ImGuiCol.ChildBg]= new Vector4(0.15f, 0.18f, 0.22f, 0.4f);
            colors[(int)ImGuiCol.PopupBg]= new Vector4(0.08f, 0.08f, 0.08f, 0.94f);
            colors[(int)ImGuiCol.Border]= new Vector4(0.08f, 0.10f, 0.12f, 1.00f);
            colors[(int)ImGuiCol.BorderShadow]= new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int)ImGuiCol.FrameBg]= new Vector4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.FrameBgHovered]= new Vector4(0.12f, 0.20f, 0.28f, 1.00f);
            colors[(int)ImGuiCol.FrameBgActive]= new Vector4(0.09f, 0.12f, 0.14f, 1.00f);
            colors[(int)ImGuiCol.TitleBg]= new Vector4(0.09f, 0.12f, 0.14f, 0.65f);
            colors[(int)ImGuiCol.TitleBgActive]= new Vector4(0.08f, 0.10f, 0.12f, 1.00f);
            colors[(int)ImGuiCol.TitleBgCollapsed]= new Vector4(0.00f, 0.00f, 0.00f, 0.51f);
            colors[(int)ImGuiCol.MenuBarBg]= new Vector4(0.15f, 0.18f, 0.22f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarBg]= new Vector4(0.02f, 0.02f, 0.02f, 0.39f);
            colors[(int)ImGuiCol.ScrollbarGrab]= new Vector4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered]= new Vector4(0.18f, 0.22f, 0.25f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive]= new Vector4(0.09f, 0.21f, 0.31f, 1.00f);
            colors[(int)ImGuiCol.CheckMark]= new Vector4(0.28f, 0.56f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab]= new Vector4(0.28f, 0.56f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.SliderGrabActive]= new Vector4(0.37f, 0.61f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.Button]= new Vector4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.ButtonHovered]= new Vector4(0.28f, 0.56f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.ButtonActive]= new Vector4(0.06f, 0.53f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.Header]= new Vector4(0.20f, 0.25f, 0.29f, 0.55f);
            colors[(int)ImGuiCol.HeaderHovered]= new Vector4(0.26f, 0.59f, 0.98f, 0.80f);
            colors[(int)ImGuiCol.HeaderActive]= new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.Separator]= new Vector4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.SeparatorHovered]= new Vector4(0.10f, 0.40f, 0.75f, 0.78f);
            colors[(int)ImGuiCol.SeparatorActive]= new Vector4(0.10f, 0.40f, 0.75f, 1.00f);
            colors[(int)ImGuiCol.ResizeGrip]= new Vector4(0.26f, 0.59f, 0.98f, 0.25f);
            colors[(int)ImGuiCol.ResizeGripHovered]= new Vector4(0.26f, 0.59f, 0.98f, 0.67f);
            colors[(int)ImGuiCol.ResizeGripActive]= new Vector4(0.26f, 0.59f, 0.98f, 0.95f);
            colors[(int)ImGuiCol.Tab]= new Vector4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.TabHovered]= new Vector4(0.26f, 0.59f, 0.98f, 0.80f);
            colors[(int)ImGuiCol.TabActive]= new Vector4(0.20f, 0.25f, 0.29f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocused]= new Vector4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.TabUnfocusedActive]= new Vector4(0.11f, 0.15f, 0.17f, 1.00f);
            colors[(int)ImGuiCol.PlotLines]= new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered]= new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram]= new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered]= new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg]= new Vector4(0.26f, 0.59f, 0.98f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget]= new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight]= new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight]= new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg]= new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.ModalWindowDimBg]= new Vector4(0.80f, 0.80f, 0.80f, 0.35f);
        }
        void SetupInput()
        {
            // Setup back-end capabilities flags
            ImGuiIOPtr io = ImGui.GetIO();
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
            io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;

            // Keyboard mapping. ImGui will use those indices to peek into the io.KeysDown[] array.
            io.KeyMap[(int)ImGuiKey.Tab] = (int)KeyboardKey.KEY_TAB;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)KeyboardKey.KEY_LEFT;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)KeyboardKey.KEY_RIGHT;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)KeyboardKey.KEY_UP;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)KeyboardKey.KEY_DOWN;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)KeyboardKey.KEY_PAGE_UP;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)KeyboardKey.KEY_PAGE_DOWN;
            io.KeyMap[(int)ImGuiKey.Home] = (int)KeyboardKey.KEY_HOME;
            io.KeyMap[(int)ImGuiKey.End] = (int)KeyboardKey.KEY_END;
            io.KeyMap[(int)ImGuiKey.Insert] = (int)KeyboardKey.KEY_INSERT;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)KeyboardKey.KEY_DELETE;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)KeyboardKey.KEY_BACKSPACE;
            io.KeyMap[(int)ImGuiKey.Space] = (int)KeyboardKey.KEY_SPACE;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)KeyboardKey.KEY_ENTER;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)KeyboardKey.KEY_ESCAPE;
            io.KeyMap[(int)ImGuiKey.A] = (int)KeyboardKey.KEY_A;
            io.KeyMap[(int)ImGuiKey.C] = (int)KeyboardKey.KEY_C;
            io.KeyMap[(int)ImGuiKey.V] = (int)KeyboardKey.KEY_V;
            io.KeyMap[(int)ImGuiKey.X] = (int)KeyboardKey.KEY_X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)KeyboardKey.KEY_Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)KeyboardKey.KEY_Z;
        }

        public void Update(float dt)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            SetPerFrameData(dt);
            UpdateInput();

            ImGui.NewFrame();
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        void SetPerFrameData(float dt)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = size / scaleFactor;
            io.DisplayFramebufferScale = Vector2.One;
            io.DeltaTime = dt;
        }

        void UpdateInput()
        {
            UpdateMousePosAndButtons();
            UpdateMouseCursor();
            UpdateGamepads();

            int keyPressed = Raylib.GetKeyPressed();
            if (keyPressed > 0)
            {
                ImGuiIOPtr io = ImGui.GetIO();
                io.AddInputCharacter((uint)keyPressed);
            }
        }

        void UpdateMousePosAndButtons()
        {
            // Update mouse buttons
            ImGuiIOPtr io = ImGui.GetIO();
            for (int i = 0; i < io.MouseDown.Count; i++)
            {
                io.MouseDown[i] = Raylib.IsMouseButtonDown((MouseButton)i);
            }

            // Modifiers are not reliable across systems
            io.KeyCtrl = io.KeysDown[(int)KeyboardKey.KEY_LEFT_CONTROL] || io.KeysDown[(int)KeyboardKey.KEY_RIGHT_CONTROL];
            io.KeyShift = io.KeysDown[(int)KeyboardKey.KEY_LEFT_SHIFT] || io.KeysDown[(int)KeyboardKey.KEY_RIGHT_SHIFT];
            io.KeyAlt = io.KeysDown[(int)KeyboardKey.KEY_LEFT_ALT] || io.KeysDown[(int)KeyboardKey.KEY_RIGHT_ALT];
            io.KeySuper = io.KeysDown[(int)KeyboardKey.KEY_LEFT_SUPER] || io.KeysDown[(int)KeyboardKey.KEY_RIGHT_SUPER];

            // Mouse scroll
            io.MouseWheel += (float)Raylib.GetMouseWheelMove();

            // Key states
            for (int i = (int)KeyboardKey.KEY_SPACE; i < (int)KeyboardKey.KEY_KB_MENU + 1; i++)
            {
                io.KeysDown[i] = Raylib.IsKeyDown((KeyboardKey)i);
            }

            // Update mouse position
            Vector2 mousePositionBackup = io.MousePos;
            io.MousePos = new Vector2(-float.MaxValue, -float.MaxValue);
            const bool focused = true;

            if (focused)
            {
                if (io.WantSetMousePos)
                {
                    Raylib.SetMousePosition((int)mousePositionBackup.X, (int)mousePositionBackup.Y);
                }
                else
                {
                    io.MousePos = Raylib.GetMousePosition();
                }
            }
        }

        void UpdateMouseCursor()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            if ((io.ConfigFlags & ImGuiConfigFlags.NoMouseCursorChange) == 0 || Raylib.IsCursorHidden())
            {
                return;
            }

            ImGuiMouseCursor imgui_cursor = ImGui.GetMouseCursor();
            if (imgui_cursor == ImGuiMouseCursor.None || io.MouseDrawCursor)
            {
                Raylib.HideCursor();
            }
            else
            {
                Raylib.ShowCursor();
            }
        }

        void UpdateGamepads()
        {
            ImGuiIOPtr io = ImGui.GetIO();
        }

        /// <summary>
        /// Gets the geometry as set up by ImGui and sends it to the graphics device
        /// </summary>
        public void Draw()
        {
            ImGui.Render();
            unsafe { RenderCommandLists(ImGui.GetDrawData()); }
        }

        // Returns a Color struct from hexadecimal value
        Color GetColor(uint hexValue)
        {
            Color color;

            color.r = (byte)(hexValue & 0xFF);
            color.g = (byte)((hexValue >> 8) & 0xFF);
            color.b = (byte)((hexValue >> 16) & 0xFF);
            color.a = (byte)((hexValue >> 24) & 0xFF);

            return color;
        }

        void DrawTriangleVertex(ImDrawVertPtr idxVert)
        {
            Color c = GetColor(idxVert.col);
            Rlgl.rlColor4ub(c.r, c.g, c.b, c.a);
            Rlgl.rlTexCoord2f(idxVert.uv.X, idxVert.uv.Y);
            Rlgl.rlVertex2f(idxVert.pos.X, idxVert.pos.Y);
        }

        // Draw the imgui triangle data
        void DrawTriangles(uint count, ImVector<ushort> idxBuffer, ImPtrVector<ImDrawVertPtr> idxVert, int idxOffset, int vtxOffset, IntPtr textureId)
        {
            uint texId = (uint)textureId;
            ushort index;
            ImDrawVertPtr vertex;

            if (Rlgl.rlCheckBufferLimit((int)count * 3))
            {
                Rlgl.rlglDraw();
            }

            Rlgl.rlPushMatrix();
            Rlgl.rlBegin(Rlgl.RL_TRIANGLES);
            Rlgl.rlEnableTexture(texId);

            for (int i = 0; i <= (count - 3); i += 3)
            {
                index = idxBuffer[idxOffset + i];
                vertex = idxVert[vtxOffset + index];
                DrawTriangleVertex(vertex);

                index = idxBuffer[idxOffset + i + 2];
                vertex = idxVert[vtxOffset + index];
                DrawTriangleVertex(vertex);

                index = idxBuffer[idxOffset + i + 1];
                vertex = idxVert[vtxOffset + index];
                DrawTriangleVertex(vertex);
            }

            Rlgl.rlDisableTexture();
            Rlgl.rlEnd();
            Rlgl.rlPopMatrix();
        }

        unsafe void RenderCommandLists(ImDrawDataPtr drawData)
        {
            // Scale coordinates for retina displays (screen coordinates != framebuffer coordinates)
            int fbWidth = (int)(drawData.DisplaySize.X * drawData.FramebufferScale.X);
            int fbHeight = (int)(drawData.DisplaySize.Y * drawData.FramebufferScale.Y);

            // Avoid rendering if display is minimized or if the command list is empty
            if (fbWidth <= 0 || fbHeight <= 0 || drawData.CmdListsCount == 0)
            {
                return;
            }

            drawData.ScaleClipRects(ImGui.GetIO().DisplayFramebufferScale);
            Rlgl.rlDisableBackfaceCulling();

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[n];

                // Vertex buffer and index buffer generated by Dear ImGui
                ImPtrVector<ImDrawVertPtr> vtxBuffer = cmdList.VtxBuffer;
                ImVector<ushort> idxBuffer = cmdList.IdxBuffer;

                for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
                {
                    ImDrawCmdPtr pcmd = cmdList.CmdBuffer[cmdi];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        // pcmd.UserCallback(cmdList, pcmd);
                    }
                    else
                    {
                        Vector2 pos = drawData.DisplayPos;
                        int rectX = (int)((pcmd.ClipRect.X - pos.X) * drawData.FramebufferScale.X);
                        int rectY = (int)((pcmd.ClipRect.Y - pos.Y) * drawData.FramebufferScale.Y);
                        int rectW = (int)((pcmd.ClipRect.Z - rectX) * drawData.FramebufferScale.X);
                        int rectH = (int)((pcmd.ClipRect.W - rectY) * drawData.FramebufferScale.Y);

                        if (rectX < fbWidth && rectY < fbHeight && rectW >= 0.0f && rectH >= 0.0f)
                        {
                            Raylib.BeginScissorMode(rectX, rectY, rectW, rectH);
                            DrawTriangles(pcmd.ElemCount, idxBuffer, vtxBuffer, (int)pcmd.IdxOffset, (int)pcmd.VtxOffset, pcmd.TextureId);
                        }
                    }
                }
            }

            Raylib.EndScissorMode();
            Rlgl.rlEnableBackfaceCulling();
        }
    }
}
