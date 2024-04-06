// Don't Put me on the Spot, 3/27/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble
{
    /// <summary>
    /// A spring that launches ingredients when stepped on.
    /// </summary>
    internal class Spring : Tile
    {
        private AnimationController _animation;

        public override Rectangle TileRect
        {
            get
            {
                // Make the jump pad collision smaller
                return new Rectangle(_mapColPos * 16 + 6, _mapRowPos * 16 + 8, 4, 8);
            }
        }

        /// <summary>
        /// Constructor for spring object with set animation frames and fps
        /// </summary>
        public Spring() : base(ContentLoader.TexSpring, true, false)
        {
            _animation = new AnimationController(4, 16);
        }

        /// <summary>
        /// Updates the animation frame of the spring
        /// </summary>
        /// <param name="gameTime">Object to tell how much time has passed</param>
        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }

        /// <summary>
        /// Draws the spring to the screen
        /// </summary>
        /// <param name="sb">A SpriteBatch object to draw with</param>
        /// <param name="x">x position to draw it</param>
        /// <param name="y">y position to draw it</param>
        public override void Draw(SpriteBatch sb, int x, int y)
        {
            sb.Draw(_texture, base.TileRect,
                _animation.SourceRect, Color.White);
        }

        public override void OnCollision(Ingredient ingredient)
        {
            // Launch ingredient
            base.OnCollision(ingredient);
            Vector2 moveSpeed = ingredient.MoveSpeed;
            moveSpeed.Y = -160.0f;
            ingredient.MoveSpeed = moveSpeed;

            // Play animation
            _animation.PlayOnce();
        }
    }
}
