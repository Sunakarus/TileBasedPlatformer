using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileBasedSmoothPlatformer
{
    internal class Wall
    {
        public Vector2 position;
        public Rectangle hitbox;
        public static Wall Nothing = new Wall(new Vector2(-1000, -1000));

        public Wall(Vector2 position)
        {
            this.position = position;
            hitbox = new Rectangle((int)position.X, (int)position.Y, Controller.tileSize, Controller.tileSize);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentManager.tWall, position, Color.White);
        }
    }
}