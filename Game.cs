using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using ImGuiNET;

namespace MasterSpark
{
    static class Game
    {
        public static RenderController GameCamera;
        public static int frame = 0;
        static Player player;
        static Queue<LinearBullet> ActiveBullets = new Queue<LinearBullet>();
        public static Vector2 PlayAreaOrigin = new Vector2(20, 20);
        public static Vector2 PlayAreaSize = new Vector2(600, 700);
        public static Random random = new Random();
        public static Texture2D BulletTexture; // Make a proper texture initialisating thing at some point!
        public static Stage CurrentStage;

        public static float bulletX = 240f;
        public static float bulletY = 50f;
        public static float bulletRadian = 0f;
        public static float bulletVelocity = 1.2f;
        public static int bulletN = 5;
        public static float bulletSpread = 0.5f;
        public static float bulletAngular = 0f;

        public static void Init()
        {
            CurrentStage = new Stage("TestStage");
            // Create the player
            player = new Player("Reimu");

            BulletTexture = Raylib.LoadTexture("assets/dagger.png");
            Raylib.GenTextureMipmaps(ref BulletTexture);

            GameCamera = new RenderController(
                PlayAreaSize,
                new Vector2(600, 700),
                1f,
                true
            );
        }
        public static void Update()
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
                            new Vector2(bulletX, bulletY), 
                            (float)(Math.PI/12 * (i - (bulletN/2))),
                            Raylib.ColorFromHSV(new Vector3((float)Raylib.GetTime()*20f, 0.6f, 1f)),
                            bulletVelocity
                        )
                    );
                }
                // ActiveBullets.Enqueue(
                //     new LinearBullet(
                //         new Vector2(bulletX, bulletY), 
                //         bulletRadian,
                //         Raylib.ColorFromHSV(new Vector3((float)Raylib.GetTime()*20f, 0.6f, 1f)),
                //         bulletVelocity
                //     )
                // );
                // ActiveBullets.Enqueue(
                //     new LinearBullet(
                //         new Vector2(bulletX, bulletY), 
                //         bulletRadian,
                //         Raylib.ColorFromHSV(new Vector3((float)Raylib.GetTime()*20f, 0.6f, 1f)),
                //         bulletVelocity,
                //         (float)(1f/800f * Math.PI)
                //     )
                // );
            }

            player.Update();
            
            frame++;
        }
        public static void Draw()
        {
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
