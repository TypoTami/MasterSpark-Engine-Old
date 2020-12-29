using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    class Player
    {
        private string Character;
        public Vector2 position;
        private Texture2D Atlas;
        private int frame = 4;

        public Player(string character)
        {
            Character = character;
            position.X = 400;
            position.Y = 300;

            Atlas = Raylib.LoadTexture("assets/reimu.png");
            Raylib.GenTextureMipmaps(ref Atlas);
        }

        public void Update()
        {
            float moveSpeed = 3.0f;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
            {
                moveSpeed = moveSpeed / 2;
            }  

            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                position.X += moveSpeed;
            } 
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                position.X -= moveSpeed;
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                position.Y += moveSpeed;
            } 
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_UP))
            {
                position.Y -= moveSpeed;
            }
        }

        public void Draw()
        {
            if (Game.frame % 5 == 0)
            {
                frame++;
            }
            if (frame > 8)
            {
                frame = 4;
            }

            Helpers.DrawFromAtlas(Atlas, position, 0.75f, 12, false, frame);     
        }
    }
}
