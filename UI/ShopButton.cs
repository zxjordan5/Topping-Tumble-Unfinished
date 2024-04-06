// Don't Put me on the Spot, 3/22/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble.UI
{
    /// <summary>
    /// A button that displays information about a shop item.
    /// </summary>
    internal class ShopButton : UIButton
    {
        public ShopButton(ShopItem item, Vector2 position)
            : base(item.Texture, item.HoverTexture, position, item.Cost + "$") { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, DrawingRect, Color.White);
            UIText.DrawString(spriteBatch, ContentLoader.FntPixelSmall, Text, Position, Align.Left, Align.Top);
        }
    }
}
