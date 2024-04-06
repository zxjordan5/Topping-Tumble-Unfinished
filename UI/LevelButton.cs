// Don't Put me on the Spot, 3/22/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble.UI
{
    /// <summary>
    /// A button that displays information about a level for the level select.
    /// </summary>
    internal class LevelButton : UIButton
    {
        private int _number;
        public LevelButton(LevelData level, int number, Vector2 position)
            : base(ContentLoader.TexLevelSelectButton, ContentLoader.TexLevelSelectButtonHover, position, "")
        {
            _number = number;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            UIText.DrawString(spriteBatch, ContentLoader.FntPixelBold, _number + "", new Vector2(PositionCenter.X, Y));
        }
    }
}
