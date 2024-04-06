using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// Don't put me on the spot - 4/6/24
namespace ToppingTumble.TileClasses
{
    /// <summary>
    /// Tile that will speed up the ingredient
    /// </summary>
    internal class SpeedTile : Tile
    {
        /// <summary>
        /// Default tile launch speed
        /// </summary>
        public const float DefaultLaunchSpeed = 75.0f;

        /// <summary>
        /// Constructor for a speed tile, not walkable, not
        /// </summary>
        public SpeedTile() : base(ContentLoader.TexTestingTerry, true, false) { }

        /// <summary>
        /// Collision method for a speed tile, will give a horizontal acceleration
        /// increase to the ingredient
        /// </summary>
        /// <param name="ingredient">The ingredient currently colliding with the tile</param>
        public override void OnCollision(Ingredient ingredient)
        {
            base.OnCollision(ingredient);

            // Give a horizontal speed boost to the ingredient
            Vector2 accelCopy = ingredient.MoveSpeed;
            if(accelCopy.X > 0)
            {
                accelCopy.X += DefaultLaunchSpeed;
            }
            else
            {
                accelCopy.X -= DefaultLaunchSpeed;
            }
            ingredient.MoveSpeed = accelCopy;

            // Debug.WriteLine($"New horizontal acceleration vector: " + ingredient.HorizontalAcceleration.X);

            //System.Diagnostics.Debug.WriteLine("SPEED UP");
        }
    }
}
