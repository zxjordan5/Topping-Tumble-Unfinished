// Don't Put me on the Spot, 4/3/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble.TileClasses
{
    /// <summary>
    /// A tile which can be toggled by a switch.
    /// </summary>
    internal class SwitchDoorTile : Tile
    {
        public override bool IsWalkable
        {
            get
            {
                if (!IsEnabled)
                    return false;
                return base.IsWalkable;
            }
        }

        public SwitchColor TargetColor { get; private set; }

        /// <summary>
        /// Gets or sets whether the door can be collided with.
        /// </summary>
        public bool IsEnabled { get; set; }
        private bool _startedEnabled;

        /// <summary>
        /// Overload constructor that will also take into consideration if the tile
        /// is currently activated upon level loading
        /// </summary>
        /// <param name="targetColor">The target color that corresponds with the tile</param>
        /// <param name="isEnabled">Whether or not the tile will start enabled or not</param>
        public SwitchDoorTile(SwitchColor targetColor, bool isEnabled) : base(ContentLoader.TexSwitchBlock, false, true)
        {
            TargetColor = targetColor;

            IsEnabled = isEnabled;
            _startedEnabled = isEnabled;

            GameMain.Instance.Gameplay.OnResetGame += OnResetGame;
        }

        public override void Draw(SpriteBatch sb, int x, int y)
        {
            Color color = Color.White;
            if (!IsEnabled)
                color.A = 63;
            sb.Draw(_texture, new Rectangle(x, y, 16, _texture.Height), new Rectangle(16 * (int)TargetColor, 0, 16, _texture.Height), color);
        }

        private void OnResetGame()
        {
            IsEnabled = _startedEnabled;
        }
    }
}
