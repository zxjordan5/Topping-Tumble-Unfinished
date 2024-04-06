using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble.UI
{
    /// <summary>
    /// Text alignments, horizontal and vertical
    /// </summary>
    public enum Align
    {
        Left,
        Right,
        Top,
        Bottom,
        Center
    }

    /// <summary>
    /// Static class which allows for text to be drawn with custom pixel font and effects
    /// </summary>
    static class UIText
    {

        // Methods

        /// <summary>
        /// Draw text with custom pixel font with default alignment and spacing
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="text">String to write</param>
        /// <param name="position">Origin of drawn text</param>
        public static void DrawString(SpriteBatch sb, UIFont font, String text, Vector2 position)
        {
            DrawString(sb, font, text, position, Align.Center, Align.Center);
        }

        /// <summary>
        /// Draw text with custom pixel font
        /// </summary>
        /// <param name="sb">SpriteBatch to draw with</param>
        /// <param name="text">String to write</param>
        /// <param name="position">Origin of drawn text</param>
        /// <param name="hAlign">Horizontal alignment of drawn text</param>
        /// <param name="vAlign">Vertical alingment of drawn text</param>
        /// <param name="spacing">Pixel count between each character</param>
        public static void DrawString(SpriteBatch sb, UIFont font, String text, Vector2 position, Align hAlign, Align vAlign)
        {
            int chars = text.Length;                                            // Number of individual characters in string to draw
            int fullWidth = font.Width * chars + font.Spacing * (chars - 1);    // Full pixel width of this string

            // Use given alignment to get horizontal position of origin to draw from
            switch (hAlign)
            {
                case Align.Left: break;
                case Align.Center: position.X -= fullWidth / 2; break;
                case Align.Right: position.X -= fullWidth; break;
                // Default to center-aligned
                default: goto case Align.Center;
            }
            // Use given alignment to get veritcal position of origin to draw from
            switch (vAlign)
            {
                case Align.Top: break;
                case Align.Center: position.Y -= font.Height / 2; break;
                case Align.Bottom: position.Y -= font.Height; break;
                // Default to center-aligned
                default: goto case Align.Center;
            }

            // Start at index 32 of UTF8 (32 = "Space")
            int charIndexOffset = 32;

            // Draw string characters
            for (int i = 0; i < chars; i++)
            {
                Rectangle charRect = new Rectangle((text[i] - charIndexOffset) * font.Width, 0, font.Width, font.Height);   // Get source rectangle position of current character
                Vector2 charPos = new Vector2(position.X + (font.Width + font.Spacing) * i, position.Y);                    // Get draw position of character from current iteration
                sb.Draw(font.Texture, charPos, charRect, Color.White);
            }
        }
    }
}
