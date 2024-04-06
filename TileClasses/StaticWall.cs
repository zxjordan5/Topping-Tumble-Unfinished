using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToppingTumble.TileClasses;
// Represents an immovable wall

namespace ToppingTumble
{
    internal class StaticWall : Tile, ITileable
    {
        public int TextureIndex { get; set; }

        // Constructor
        /// <summary>
        /// Creates a new immovable wall tile
        /// </summary>
        /// <param name="x">x position in the tilemap</param>
        /// <param name="y">y position in the tilemap</param>
        public StaticWall() :
            base(ContentLoader.TexStaticTileset, false, true)
        { }

        /// <summary>
        /// Draws the tile to the screen
        /// </summary>
        /// <param name="sb">A SpriteBatch object to draw with</param>
        /// <param name="x">x position to draw it</param>
        /// <param name="y">y position to draw it</param>
        public override void Draw(SpriteBatch sb, int x, int y)
        {
            // Draw the texture to the screen
            sb.Draw(_texture,
                    new Vector2(x, y),
                    new Rectangle(TextureIndex * 16, 0, 16, 16),
                    Color.White);
        }
    }
}
