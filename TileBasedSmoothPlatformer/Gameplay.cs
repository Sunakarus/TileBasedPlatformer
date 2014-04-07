using Microsoft.Xna.Framework.Graphics;

namespace TileBasedSmoothPlatformer
{
    internal class Gameplay
    {
        private Controller controller;

        public Gameplay(Controller controller)
        {
            this.controller = controller;
        }

        public void Update()
        {
            controller.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            controller.Draw(spriteBatch);
        }
    }
}