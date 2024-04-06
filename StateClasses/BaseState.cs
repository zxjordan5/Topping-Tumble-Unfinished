// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble
{
    /// <summary>
    /// A class to represent a current state of the game and the logic/rendering loop around that.
    /// </summary>
    internal abstract class BaseState
    {
        /// <summary>
        /// Called by GameMain when this state becomes the CurrentState.
        /// </summary>
        public virtual void Begin() { }

        /// <summary>
        /// Updates game loop logic.
        /// </summary>
        /// <param name="gameTime">The current elapsed game time.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the current state to screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw to.</param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
