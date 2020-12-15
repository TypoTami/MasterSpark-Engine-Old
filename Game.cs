using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;

namespace raylibTouhou
{
    static class Game
    {
        public static int frame = 0;
        static Player player;
        static Queue<LinearBullet> ActiveBullets = new Queue<LinearBullet>();
        public static Vector2 PlayAreaOrigin = new Vector2(40, 40);
        public static Vector2 PlayAreaSize = new Vector2(440, 520);
        public static Random random = new Random();
        static Path testPath;
        public static void Init()
        {
            // Create the player
            player = new Player("Reimu");

            Vector2[] testPoints = new Vector2[] {
                new Vector2(90, 30),
                new Vector2(130, 100),
                new Vector2(180, 70),
                new Vector2(230, 120),
                new Vector2(305, 280),
                new Vector2(295, 320),
                new Vector2(260, 360),
                new Vector2(160, 420)
            };
            string[] testSegments = new string[] {
                "BEZIER",
                "BEZIER",
                "LINE"                
            };
            testPath = new Path(testPoints, testSegments, new Vector2(100, 100), 0.0f);
        }
        public static void MainLoop()
        {
            // if (frame % 5 == 0)
            // {
            //     for (int i = 0; i < 10; i++)
            //     {
            //         ActiveBullets.Enqueue(
            //             new LinearBullet(
            //                 new Vector2((float)Math.Sin(Raylib.GetTime() * 10) * 10  + 240f, 50f),
            //                 new Vector2((random.Next(-10, 10)/7.0f), (random.Next(8, 10)/3.5f))
            //             )
            //         );
            //     }
            // }

            player.Update();
            
            testPath.Origin = new Vector2(
                (float)Math.Sin(Raylib.GetTime() * 1f) * 75f + 100,
                (float)Math.Cos(Raylib.GetTime() * 1f) * 50f + 100
            );
            testPath.Rotation = ((float)Math.Sin(Raylib.GetTime() * 1f) * 0.8f) + 0f;
            testPath.Scale = ((float)Math.Cos(Raylib.GetTime() * 0.85f) * 0.1f) + 0.9f;
            testPath.Update();

            Draw();
            frame++;
        }
        private static void Draw()
        {
            Raylib.ClearBackground(Color.BLACK);

            // Raylib.DrawRectangleV(PlayAreaOrigin, PlayAreaSize, Color.LIGHTGRAY);

            for (int i = 0; ActiveBullets.Count > i; i++)
            {
                LinearBullet current = ActiveBullets.Dequeue();
                if (current.UpdateDraw())
                {
                    ActiveBullets.Enqueue(current);
                }
            }
            testPath.Draw();

            player.Draw();

            Raylib.DrawText($"Active bullets: \t{ActiveBullets.Count}\n {PlayAreaOrigin}, {PlayAreaSize}", 10, 40, 20, Color.RED);
            
            Raylib.DrawText($"FPS: {Raylib.GetFPS()}\t FrameTime: {Raylib.GetFrameTime()}", 10, 10, 20, Color.GREEN);
        }
    }
}
