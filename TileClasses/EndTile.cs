using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Represents the finish of the level (oven)

namespace ToppingTumble
{
    internal class EndTile : Tile
    {
        // Constructor
        /// <summary>
        /// Creates a new finish/end level tile (oven)
        /// </summary>
        /// <param name="x">x value in the tilemap</param>
        /// <param name="y">y value in the tilemap</param>
        public EndTile() :
            base(ContentLoader.TexOven, false, false)
        { }
        // REPLACE PLACEHOLDER WITH OVEN TEXTURE

        // Methods
        /// <summary>
        /// Removes an ingredient since it reached the end point,
        /// ends the level once all ingredients are gone
        /// </summary>
        public override void OnCollision(Ingredient ingredient)
        {
            // Call the ingredient "passed" method in GameplayState
            GameMain.Instance.Gameplay.IngredientPassed(ingredient);
        }
    }
}
