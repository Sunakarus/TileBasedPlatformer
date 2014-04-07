using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TileBasedSmoothPlatformer
{
    internal class Controller
    {
        public Player player;
        public List<Wall> wallList = new List<Wall>();
        public List<Slope> slopeList = new List<Slope>();
        public static readonly int tileSize = 32;

        public Controller()
        {
          /*  slopeList.Add(new Slope(new Vector2(-500, 400), new Vector2(400, 300), 1));
            slopeList.Add(new Slope(new Vector2(400, 300), new Vector2(500, 300), 5));
            slopeList.Add(new Slope(new Vector2(500, 350), new Vector2(900, 500), 10));
             player = new Player(this, new Vector2(100, 0));*/
            //slopeList.Add(new Slope(new Vector2(500, 350), new Vector2(600, 100), 32));

           /* wallList.Add(new Wall(new Vector2(400, 300)));
            wallList.Add(new Wall(new Vector2(432, 300)));
            wallList.Add(new Wall(new Vector2(464, 300)));*/

           

            /*  for (int i = 0; i < 15; i++)
              {
                  wallList.Add(new Wall(new Vector2(i * tileSize, tileSize * 10)));
              }
              int numOfSteps = 15;
              for (int ix = 0; ix < numOfSteps; ix++)
              {
                  wallList.Add(new Wall(new Vector2(ix * tileSize, tileSize * (numOfSteps-ix +20) * numOfSteps/80)));
              }
              for (int i = 0; i < 15; i++)
              {
                  wallList.Add(new Wall(new Vector2(i * tileSize + tileSize * 5, tileSize * 6)));
              }
              for (int i = 0; i < 15; i++)
              {
                  wallList.Add(new Wall(new Vector2(i * tileSize + tileSize * 10, tileSize * 3)));
              }
              for (int i = 0; i < 2; i++)
              {
                  wallList.Add(new Wall(new Vector2(i * tileSize + tileSize * 4, tileSize * 3)));
              }
              for (int i = 0; i < 2; i++)
              {
                  wallList.Add(new Wall(new Vector2(i * tileSize, tileSize * 9)));
              }
              for (int ix = 0; ix < 5; ix++)
              {
                  for (int iy = 0; iy < 2; iy++)
                  {
                      wallList.Add(new Wall(new Vector2(ix * tileSize + tileSize * ix, iy * tileSize + tileSize * 9f)));
                  }
              }*/
        }

        public bool IsObstacle(Vector2 pos, out Wall collision, out Slope slopeCollision)
        {
            foreach (Wall w in wallList)
            {
                if (w.hitbox.Contains((int)pos.X, (int)pos.Y))
                {
                    slopeCollision = null;
                    collision = w;
                    return true;
                }
            }
            foreach (Slope s in slopeList)
            {
                if (s.ContainsPoint(pos))
                {
                    collision = null;
                    slopeCollision = s;
                    return true;
                }
            }
            slopeCollision = null;
            collision = null;
            return false;
        }

        public bool IsObstacle(Vector2 pos)
        {
            foreach (Wall w in wallList)
            {
                if (w.hitbox.Contains((int)pos.X, (int)pos.Y))
                {
                    return true;
                }
            }
            foreach (Slope s in slopeList)
            {
                if (s.ContainsPoint(pos))
                {
                    return true;
                }
            }
            return false;
        }

        public void Update()
        {
            player.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
            foreach (Wall wall in wallList)
            {
                wall.Draw(spriteBatch);
            }
            foreach (Slope slope in slopeList)
            {
                slope.Draw(spriteBatch);
            }

            spriteBatch.DrawString(ContentManager.tahoma, player.position.ToString() + "\ngrounded: " + player.grounded + "\njump: " + player.jump, Vector2.Zero, Color.Black);
        }
    }
}