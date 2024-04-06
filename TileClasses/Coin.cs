// Don't Put me on the Spot, 3/27/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble.TileClasses
{
    /// <summary>
    /// A coin that can be collected by ingredients and increases the current score.
    /// </summary>
    internal class Coin : Tile
    {
        public override Rectangle TileRect
        {
            get
            {
                // Make the coin collision smaller
                return new Rectangle(_mapColPos * 16 + 4, _mapRowPos * 16 + 4, 8, 8);
            }
        }

        /// <summary>
        /// Returns true while collected.
        /// </summary>
        public bool IsCollected { get; private set; }

        public Coin() : base(ContentLoader.TexCoin, false, false)
        {
            GameMain.Instance.Gameplay.OnResetGame += OnResetGame;
        }

        public override void OnCollision(Ingredient ingredient)
        {
            base.OnCollision(ingredient);
            if (IsCollected)
                return;
            IsCollected = true;
            GameMain.Instance.Gameplay.CoinsCollected++;
        }

        public override void Draw(SpriteBatch sb, int x, int y)
        {
            Color color = Color.White;
            if (IsCollected)
                color.A = 63;
            sb.Draw(_texture, new Rectangle(x, y, _texture.Width, _texture.Height), color);
        }

        private void OnResetGame()
        {
            if (!IsCollected)
                return;
            IsCollected = false;
            GameMain.Instance.Gameplay.CoinsCollected--;
        }
    }
}
