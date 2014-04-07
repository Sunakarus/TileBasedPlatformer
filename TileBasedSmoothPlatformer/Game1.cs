#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TiledSharp;

#endregion Using Statements

namespace TileBasedSmoothPlatformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Gameplay gamePlay;
        private Controller controller;
        TmxMap map;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            controller = new Controller();
            ContentManager.tPlayer = Content.Load<Texture2D>("Sprites/hobgoblin");
            ContentManager.tWall = Content.Load<Texture2D>("Sprites/wall");
            ContentManager.tPixel = Content.Load<Texture2D>("Sprites/pixel");
            ContentManager.tahoma = Content.Load<SpriteFont>("Fonts/tahoma");
            map = new TmxMap("Content/Sprites/map.tmx");
            gamePlay = new Gameplay(controller);
            foreach (TmxLayer layer in map.Layers)
            {
                foreach (TmxLayerTile tile in layer.Tiles)
                {
                    if (tile.Gid == 1)
                    {
                        controller.wallList.Add(new Wall(new Vector2(tile.X * map.TileWidth, tile.Y * map.TileHeight)));
                    }
                }
            }
            controller.player = new Player(controller, new Vector2(map.ObjectGroups["playerLayer"].Objects["player"].X, map.ObjectGroups["playerLayer"].Objects["player"].Y));

            int thickness = 2;
            for (int i = 0; i < (int)Math.Floor(map.ObjectGroups["slopes"].Objects.Count / 2.0f); i++)
            {
                string name1 = "slope" + (i+1) + "a";
                string name2 = "slope" + (i+1) + "b";
                Vector2 slope1 = new Vector2(map.ObjectGroups["slopes"].Objects[name1].X, map.ObjectGroups["slopes"].Objects[name1].Y);
                Vector2 slope2 = new Vector2(map.ObjectGroups["slopes"].Objects[name2].X, map.ObjectGroups["slopes"].Objects[name2].Y);
                controller.slopeList.Add(new Slope(slope1, slope2, thickness));
            }

           /* Vector2 slope1 = new Vector2(map.ObjectGroups["slopes"].Objects["slope1a"].X, map.ObjectGroups["slopes"].Objects["slope1a"].Y);
            Vector2 slope2 = new Vector2(map.ObjectGroups["slopes"].Objects["slope1b"].X, map.ObjectGroups["slopes"].Objects["slope1b"].Y);
            controller.slopeList.Add(new Slope(slope1, slope2, 1));
            slope1 = new Vector2(map.ObjectGroups["slopes"].Objects["slope2a"].X
                , map.ObjectGroups["slopes"].Objects["slope2a"].Y);
            slope2 = new Vector2(map.ObjectGroups["slopes"].Objects["slope2b"].X, map.ObjectGroups["slopes"].Objects["slope2b"].Y);
            controller.slopeList.Add(new Slope(slope1, slope2, 1));*/
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            gamePlay.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            Window.Title = "FPS: " + (int)frameRate;
            spriteBatch.Begin();
            gamePlay.Draw(spriteBatch);
            
            spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}