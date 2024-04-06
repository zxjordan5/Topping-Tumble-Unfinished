using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using ToppingTumble.UI;

namespace ToppingTumble
{
    /// <summary>
    /// Interactable UI Button that performs an action when clicked
    /// </summary>
    internal class UIButton : GameObject
    {
        // Fields
        private Texture2D _baseImage;
        private Texture2D _hoverImage;

        public string Text { get; set; }

        private bool _wasHovered; // Used to ensure buttons don't get pressed the moment a state gets loaded
        public bool IsHovered { get; private set; }
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        // Properties
        public event EventHandler ClickEvent;

        // Constructor
        /// <summary>
        /// Creates a new clickable UI Button
        /// </summary>
        /// <param name="texture">Default texture of the button</param>
        /// <param name="hoverTexture">Texture of the button while hovered by the mouse</param>
        /// <param name="position">Screen position of the button</param>
        /// <param name="text">String of text for the button to display</param>
        public UIButton(Texture2D texture, Texture2D hoverTexture, Vector2 position, string text) : base(texture, position)
        {
            _baseImage = Texture;
            _hoverImage = hoverTexture;
            Text = text;
        }

        // Methods
        /// <summary>
        /// Updates this button's logic
        /// </summary>
        /// <param name="gameTime">The current elapsed game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Update mouse state of previous frame and current frame
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            // Get current mouse position (scaled to rendered screen size)
            Vector2 mousePosition = GameMain.Instance.GetMousePosition();

            // Check if this mouse position is intersecting with this button and update IsHovered
            IsHovered = false;
            var mouseRectangle = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, 1, 1);

            if (mouseRectangle.Intersects(DrawingRect))
            {
                IsHovered = true;

                // Invoke click event when this button is clicked while hovered
                if (_currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released
                    && _wasHovered)
                {
                    ClickEvent?.Invoke(this, new EventArgs());
                }
            }

            _wasHovered = IsHovered;

            // Set default button image
            Texture = _baseImage;
            // Update image to hoverImage if hovered by mouse
            if (IsHovered) Texture = _hoverImage;
        }

        /// <summary>
        /// Draws this button with text and appropriate image texture
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw to.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw this button
            base.Draw(spriteBatch);

            // Draw this button's text
            UIText.DrawString(spriteBatch, ContentLoader.FntPixelBold, Text, PositionCenter);
        }
    }
}
