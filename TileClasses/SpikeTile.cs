// Don't Put me on the Spot, 3/29/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ToppingTumble.TileClasses
{
    /// <summary>
    /// Tiles which kill an ingredient when touched. Can be rotated to face any direction.
    /// </summary>
    internal class SpikeTile : Tile
    {
        /// <summary>
        /// The "normal direction" of the spikes (where they are pointing).
        /// </summary>
        public SpikeTileDirection Direction { get; private set; }

        public SpikeTile(SpikeTileDirection direction) : base(ContentLoader.TexSpikeTile, false, false)
        {
            Direction = direction;
        }

        public override void Draw(SpriteBatch sb, int x, int y)
        {
            float rotation = 0.0f;
            switch (Direction)
            {
                case SpikeTileDirection.Up:
                    rotation = 0.0f;
                    break;
                case SpikeTileDirection.Down:
                    rotation = MathF.PI;
                    break;
                case SpikeTileDirection.Left:
                    rotation = MathF.PI * 1.5f;
                    break;
                case SpikeTileDirection.Right:
                    rotation = MathF.PI * 0.5f;
                    break;
            }

            sb.Draw(_texture, new Rectangle(x + 8, y + 8, _texture.Width, _texture.Height), null, Color.White, rotation,
                new Vector2(8.0f, 8.0f), SpriteEffects.None, 0.0f);
        }

        public override void OnCollision(Ingredient ingredient)
        {
            base.OnCollision(ingredient);
            // Kill the colliding ingredient
            if (!ingredient.IsDying)
                GameMain.Instance.Gameplay.IngredientDeath(ingredient);
        }
    }

    internal enum SpikeTileDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
