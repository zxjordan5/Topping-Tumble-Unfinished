using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToppingTumble.TileClasses;
// Represents a wall that the user put down

namespace ToppingTumble
{
    internal class PlaceableWall : Tile, ITileable
    {
        public int TextureIndex { get; set; }

        // Constructor
        /// <summary>
        /// Makes a new wall that the user has placed into the level
        /// </summary>
        /// <param name="x">x value in the tilemap</param>
        /// <param name="y">y value in the tilemap</param>
        public PlaceableWall() :
            base(ContentLoader.TexPlaceTileset, true, true)
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

        /* TODO:
         * - Change color when hovered over?
         */
    }
}
