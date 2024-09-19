using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MazeGeneration
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Random random = new Random();
        private int seed;

        private Maze maze;
        private Texture2D mazeTiles;
        private MazeSprite mazeSprite;

        private static int spriteSize = 16;
        private static int imageScale = 8;

        private int columnCount = (1920 / spriteSize) / imageScale;
        private int rowCount = (1080 / spriteSize) / imageScale;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            seed = random.Next(50_000);
            random = new Random(seed);
            System.Diagnostics.Debug.WriteLine("Seed: " + seed);

            maze = new Maze(columnCount, rowCount, random);

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = columnCount * spriteSize * imageScale;
            graphics.PreferredBackBufferHeight = rowCount * spriteSize * imageScale;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mazeTiles = Content.Load<Texture2D>("mazetiles");
            mazeSprite = new MazeSprite(mazeTiles, new Rectangle(0, 0, spriteSize, spriteSize), imageScale);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);
            maze.Draw(mazeSprite, spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
