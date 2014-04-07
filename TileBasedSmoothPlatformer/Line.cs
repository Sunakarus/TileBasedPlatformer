using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TileBasedSmoothPlatformer
{
    public class Line
    {
        private Vector2 point1, point2;
        private float tolerance = 0.001f;
        public double magnitude;
        public double angle;

        double drawAngle;
        Vector2 drawPoint1, drawPoint2;
        public Line(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1.X < point2.X ? point1 : point2;
            this.point2 = this.point1 == point1 ? point2 : point1;
           // this.point1 = point1;
         //   this.point2 = point2;
            float difX = Math.Abs(point1.X - point2.X);
            float difY = Math.Abs(point1.Y - point2.Y);
            magnitude = Math.Sqrt(Math.Pow(difX, 2) + Math.Pow(difY, 2));
            angle = Math.Atan2(difY, difX);

            drawAngle = angle;
            drawPoint1 = point1;
            drawPoint2 = point2;
            if (point2.X < point1.X)
            {
                drawPoint1 = point2;
                drawPoint2 = point1;
            }
            if (drawPoint1.Y > drawPoint2.Y)
            {
                drawAngle = Math.PI * 2 - angle;
            }
        }

        public bool ContainsPoint(Vector2 point)
        {
            if (!IsWithinBoundaries(point))
            {
                return false;
            }
            if (point1.X == point2.X)
            {
                return (point.Y >= point1.Y && point.Y <= point2.Y && Math.Abs(point.X - point1.X) <= tolerance);
            }
            if (point1.Y == point2.Y)
            {
                return (point.X >= point1.X && point.Y <= point2.X && Math.Abs(point.Y - point1.Y) <= tolerance);
            }
            double slope = (point2.Y - point1.Y) / (point2.X - point1.X);
            double Yintersect = point1.Y - slope * point1.X;
            return Math.Abs(point.Y - (slope * point.X + Yintersect)) <= tolerance;
        }

        public bool IsWithinBoundaries(Vector2 point)
        {
            float down, left, up, right;
            down = point1.Y > point2.Y ? point1.Y : point2.Y;
            up = point1.Y < point2.Y ? point1.Y : point2.Y;
            right = point1.X > point2.X ? point1.X : point2.X;
            left = point1.X < point2.X ? point1.X : point2.X;

            if (down - up <= tolerance)
            {
                return (point.X >= left && point.X <= right);
            }
            if (right - left <= tolerance)
            {
                return (point.Y >= up && point.Y <= down);
            }
            return (point.X > left && point.X < right && point.Y > up && point.Y < down);
        }

        public Vector2 GetPointAtPosX(float x)
        {
            float slope = (point2.Y - point1.Y) / (point2.X - point1.X);
            float Yintersect = point1.Y - slope * point1.X;
            return new Vector2(x, slope * x + Yintersect);
        }

        public Vector2 GetPointAtPosY(float y)
        {
            float slope = (point2.Y - point1.Y) / (point2.X - point1.X);
            float Yintersect = point1.Y - slope * point1.X;
            return new Vector2((y - Yintersect)/slope,y);
        }
    
        public void DrawLine(SpriteBatch spriteBatch, int thickness)
        {
            spriteBatch.Draw(ContentManager.tPixel, drawPoint1, null, Color.White, (float)drawAngle, Vector2.Zero, new Vector2((float)magnitude,thickness), SpriteEffects.None, 1);
        }
    }

}