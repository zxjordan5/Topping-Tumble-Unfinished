using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToppingTumble.TileClasses
{
    /// <summary>
    /// Interface to define tiles which source their textures from a tileset
    /// </summary>
    internal interface ITileable
    {
        /// <summary>
        /// Index from tileset to get texture at
        /// </summary>
        int TextureIndex { get; set; }

        // Methods
        /// <summary>
        /// Draws the tile to the screen
        /// </summary>
        /// <param name="sb">A SpriteBatch object to draw with</param>
        /// <param name="x">x position to draw it</param>
        /// <param name="y">y position to draw it</param>
        void Draw(SpriteBatch sb, int x, int y);
    }
}
