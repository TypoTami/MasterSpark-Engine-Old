using System;
using System.Numerics;
using Raylib_cs;

namespace raylibTouhou
{
    class Player
    {
        private string Character;
        public Vector2 position;
        private Texture2D sprite;
        private Rectangle frameRect;
        private Rectangle destRect;
        private int tempA = 4;

        public Player(string character)
        {
            Character = character;
            position.X = 400;
            position.Y = 300;

            sprite = Raylib.LoadTexture("player/reimu.png");

            frameRect.x = 0.0f;
            frameRect.y = 0.0f; 
            frameRect.width = (float)sprite.width/12;
            frameRect.height = (float)sprite.height;

            
            destRect.x = 400.0f;
            destRect.y = 300.0f; 
            destRect.width = 108/2;
            destRect.height = 132/2;
        }

        public void Update()
        {
            float moveSpeed = 5.0f;

            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
            {
                moveSpeed = 2.5f;
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
            Vector2 temp = position;
            Vector2 origin = new Vector2(0, 0);
            float scale = 1.5f;

            scale = (float)Math.Sin(Raylib.GetTime()*2)+1.0f;

            destRect.width = (sprite.width / 12) * scale;
            destRect.height = (sprite.height) * scale;
            destRect.x = temp.X - destRect.width / 2;
            destRect.y = temp.Y - destRect.height / 2; 

            frameRect.x = tempA*(float)sprite.width/12;
            frameRect.width = (float)sprite.width/12;
            if (Game.frame % 5 == 0) {
                tempA++;
            }
            if (tempA == 9) {
                tempA = 4;
            }

            Raylib.DrawText($"{frameRect.x}, {frameRect.width}", 400, 100, 20, Color.BLACK);
            Raylib.DrawText($"{position.X}, {position.Y}", 400, 150, 20, Color.BLACK);
            Raylib.DrawText($"{sprite.width}, {sprite.height}, {scale}", 400, 200, 20, Color.BLACK);

            Raylib.DrawTexturePro(sprite, frameRect, destRect, origin, 0.0f, Color.WHITE);       
        }
    }
}
