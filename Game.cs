using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace MasterSpark
{
    static class Game
    {
        public static int frame = 0;
        static Player player;
        static Queue<LinearBullet> ActiveBullets = new Queue<LinearBullet>();
        public static Vector2 PlayAreaOrigin = new Vector2(40, 40);
        public static Vector2 PlayAreaSize = new Vector2(440, 520);
        public static Random random = new Random();
        public static Texture2D BulletTexture; // Make a proper texture initialisating thing at some point!
        public static Stage CurrentStage;

        public static float bulletX = 240f;
        public static float bulletY = 50f;
        public static float bulletAngle = 0f;
        public static float bulletVelocity = 1.2f;
        public static int bulletN = 4;
        public static float bulletSpread = 0.5f;
        public static float bulletAngular = 0f;

        public static void Init()
        {
            CurrentStage = new Stage("TestStage");
            // Create the player
            player = new Player("Reimu");

            BulletTexture = Raylib.LoadTexture("assets/dagger.png");
            Raylib.GenTextureMipmaps(ref BulletTexture);
        }
        public static void MainLoop()
        {
            if (frame % 30 == 0)
            {
                for (int i = 0; i < bulletN; i++)
                {
                    // ActiveBullets.Enqueue(
                    //     new LinearBullet(
                    //         new Vector2((float)Math.Sin(Raylib.GetTime() * 10) * 10  + 240f, 50f),
                    //         new Vector2((random.Next(-10, 10)/7.0f), (random.Next(8, 10)/3.5f)),
                    //         random.Next(-100, 100)/10000f
                    //     )
                    // );
                    ActiveBullets.Enqueue(
                        new LinearBullet(
                            new Vector2(/* (float)Math.Sin(Raylib.GetTime() * 10) * 1  + */ bulletX, bulletY), 
                            (float)(Math.PI/180) * bulletAngle,
                            Raylib.ColorFromHSV(new Vector3((float)Raylib.GetTime()*20f, 0.6f, 1f)),
                            bulletVelocity,
                            (float)(Math.PI/180) * i/(float)bulletN * bulletSpread - (bulletSpread/200f) + bulletAngular
                        )
                    );
                }
            }

            player.Update();
            
            Draw();
            frame++;
        }
        private static void Draw()
        {
            // Raylib.ClearBackground(Color.PURPLE);
            Raylib.DrawRectangle(0, 0, 800, 600, Color.PURPLE);

            Raylib.DrawRectangleV(PlayAreaOrigin, PlayAreaSize, Color.LIGHTGRAY);

            CurrentStage.Draw();

            for (int i = 0; ActiveBullets.Count > i; i++)
            {
                LinearBullet current = ActiveBullets.Dequeue();
                if (current.UpdateDraw())
                {
                    ActiveBullets.Enqueue(current);
                }
            }

            player.Draw();
        }
    
        public static void SubmitUI()
        {
            ImGui.Text($"ActiveBullets: {ActiveBullets.Count}");
        }
    }
}
