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
        static Queue<Bullet> bullets = new Queue<Bullet>();
        public static Vector2 PlayAreaOrigin = new Vector2(40, 40);
        public static Vector2 PlayAreaSize = new Vector2(440, 520);
        private static Random random = new Random();
        public static void Init()
        {

            // Create the player
            player = new Player("Reimu");
        }
        public static void MainLoop()
        {

            if (frame % 30 == 0)
            {
                bullets.Enqueue(
                    new Bullet(
                        new Vector2(random.Next(200, 300), random.Next(200, 300)),
                        new Vector2((random.Next(-10, 10)/5.0f), (random.Next(-10, 10)/2.5f))
                    )
                );
            }

            player.Update();

            Draw();
            frame++;
        }

        private static void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.GRAY);

            Raylib.DrawRectangleV(PlayAreaOrigin, PlayAreaSize, Color.LIGHTGRAY);

            for (int i = 0; bullets.Count > i; i++)
            {
                Bullet current = bullets.Dequeue();
                if (current.UpdateDraw())
                {
                    bullets.Enqueue(current);
                }
            }

            player.Draw();

            Raylib.DrawText($"Active bullets: \t{bullets.Count}\n {PlayAreaOrigin}, {PlayAreaSize}", 10, 10, 20, Color.BLACK);

            Raylib.EndDrawing();
        }
    }
}
