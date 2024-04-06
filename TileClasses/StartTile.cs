using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Represents the starting tile (fridge)

namespace ToppingTumble
{
    internal class StartTile : Tile
    {
        // Constructor
        /// <summary>
        /// Creates a new start tile (fridge) that ingredients spawn from
        /// </summary>
        /// <param name="x">x value on the tilemap</param>
        /// <param name="y">y value on the tilemap</param>
        public StartTile() :
            base(ContentLoader.TexFridge, false, false)
        { }
        // REPLACE PLACEHOLDER WITH FRIDGE TEXTURE
    }
}
