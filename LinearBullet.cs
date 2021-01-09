using System;
using System.Numerics;
using Raylib_cs;

namespace MasterSpark
{
    class LinearBullet
    {
        private Vector2 Position;
        private Vector2 Velocity;
        private float AngularVelocity;
        private float Angle;
        private Color Colour;
        private float HitboxRadius;

        public LinearBullet(Vector2 position, float angle, Color color, float velocity = 0.0f, float angularVelocity = 0.0f, float hitboxRadius = 5f)
        {
            this.Position = position;
            this.Velocity = new Vector2(
                (float)Math.Cos(angle + (Math.PI/2)) * velocity,
                (float)Math.Sin(angle + (Math.PI/2)) * velocity
            );
            this.AngularVelocity = angularVelocity;

            this.Angle = angle;
            this.HitboxRadius = hitboxRadius;

            int hex = Game.random.Next(0x000000f, 0x00f00ff);
            this.Colour = color;
        }

        public bool UpdateDraw()
        {
            Update();
            Draw();

            if ((Helpers.InBetweenFloat(Game.PlayAreaOrigin.X - 10, (Game.PlayAreaSize.X + Game.PlayAreaOrigin.X) + 10, Position.X)) &&
                (Helpers.InBetweenFloat(Game.PlayAreaOrigin.Y - 10, (Game.PlayAreaSize.Y + Game.PlayAreaOrigin.Y) + 10, Position.Y)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Update()
        {
            Position = Vector2.Add(Position, Velocity);
            if (AngularVelocity != 0f)
            {
                float AngularVelocityCorrected = AngularVelocity;
                Angle += AngularVelocityCorrected;
                Velocity.X = (float)(Velocity.X * Math.Cos(AngularVelocityCorrected) - Velocity.Y * Math.Sin(AngularVelocityCorrected));
                Velocity.Y = (float)(Velocity.X * Math.Sin(AngularVelocityCorrected) + Velocity.Y * Math.Cos(AngularVelocityCorrected));
            }
        }

        private void Draw()
        {
            Vector2 TexOrigin = new Vector2(
                Position.X - (Game.BulletTexture.width / 2f) * 0.25f,
                Position.Y - (Game.BulletTexture.height / 2f) * 0.25f
            );
            Helpers.DrawSprite(Game.BulletTexture, Position, 0.3f, Angle, Colour);

            if (Settings.DEBUGShowHitbox) { Raylib.DrawCircle((int)Position.X, (int)Position.Y, HitboxRadius, new Color(0, 255, 0, 80)); }

        }
    }
}
