using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeGeneration
{
    public class MazeSprite
    {
        private Texture2D tileSet;
        public Rectangle tileProportions;

        public MazeSprite(Texture2D tileSet, Rectangle tileProportions)
        {
            this.tileSet = tileSet;
            this.tileProportions = tileProportions;
        }

        public void Draw(SpriteBatch spriteBatch, int tile, int x, int y)
        {
            Rectangle source = new Rectangle(tileProportions.X + tile * tileProportions.Width, tileProportions.Y, tileProportions.Width, tileProportions.Height);
            Rectangle destination = new Rectangle(x, y, tileProportions.Width, tileProportions.Height);
            spriteBatch.Draw(tileSet, destination, source, Color.White);
        }
    }
}
