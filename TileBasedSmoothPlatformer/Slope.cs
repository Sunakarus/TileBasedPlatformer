using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TileBasedSmoothPlatformer
{
    internal class Slope
    {
        private List<Line> lineList = new List<Line>();
        public static Slope Nothing = new Slope(Vector2.Zero, Vector2.Zero, 1);
        public Line topLine;

        public Slope(Vector2 point1, Vector2 point2, int thickness)
        {
            if (thickness <= 0)
            {
                thickness = 1;
            }
            for (int i = 0; i < thickness; i++)
            {
                lineList.Add(new Line(new Vector2(point1.X, point1.Y + i), new Vector2(point2.X, point2.Y + i)));
            }
            topLine = lineList[0];
        }

        public bool ContainsPoint(Vector2 point)
        {
            foreach (Line l in lineList)
            {
               
                if (l.ContainsPoint(point))
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                topLine.DrawLine(spriteBatch, lineList.Count);
        }
    }
}