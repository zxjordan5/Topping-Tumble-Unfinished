using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble.UI
{
    /// <summary>
    /// Custom font with a texture and width/height/spacing for characters
    /// </summary>
    internal class UIFont
    {
        // Properties
        public Texture2D Texture { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Spacing { get; private set; }

        // Constructor(s)
        /// <summary>
        /// Creates a new custom pixel font to be used with UIText
        /// </summary>
        /// <param name="texture">Texture sheet of the font</param>
        /// <param name="width">Width of an individual character in pixels</param>
        /// <param name="height">Height of an individual character in pixels</param>
        /// <param name="spacing">Pixel spacing between characters</param>
        public UIFont(Texture2D texture, int width, int height, int spacing)
        {
            Texture = texture;
            Width = width;
            Height = height;
            Spacing = spacing;
        }

    }
}
