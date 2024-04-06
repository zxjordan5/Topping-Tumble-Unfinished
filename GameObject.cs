// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToppingTumble
{
    /// <summary>
    /// Represents an object on the screen with a graphic and position.
    /// </summary>
    internal class GameObject
    {
        private Texture2D _texture;
        /// <summary>
        /// The texture or texture sheet associated with this object.
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                if (_texture == null)
                    return;
                // Update the rectangle to reflect the new width and height
                _drawingRect.Width = _texture.Width;
                _drawingRect.Height = _texture.Height;
            }
        }

        private Vector2 _position;
        /// <summary>
        /// Gets or sets the GameObject's world space position.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _drawingRect.X = (int)_position.X;
                _drawingRect.Y = (int)_position.Y;
            }
        }
        /// <summary>
        /// Gets or sets the GameObject's world space position based on the center of the bounding box.
        /// </summary>
        public Vector2 PositionCenter
        {
            get { return new Vector2(Position.X + Width * 0.5f, Position.Y + Height * 0.5f); }
            set { Position = new Vector2(value.X - Width * 0.5f, value.Y - Height * 0.5f); }
        }

        /// <summary>
        /// Gets or sets the X coordinate of the position.
        /// </summary>
        public float X
        {
            get { return _position.X; }
            set
            {
                _position.X = value;
                _drawingRect.X = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the position.
        /// </summary>
        public float Y
        {
            get { return _position.Y; }
            set
            {
                _position.Y = value;
                _drawingRect.Y = (int)value;
            }
        }

        private Rectangle _drawingRect;
        /// <summary>
        /// Only used to store the calculated drawing rectangle rather than re-making it every frame.
        /// </summary>
        protected Rectangle DrawingRect { get { return _drawingRect; } } // Protected in case needed for an overridden draw method

        /// <summary>
        /// Width of the game object.
        /// </summary>
        public int Width { get { return _drawingRect.Width; } }
        /// <summary>
        /// Height of the game object.
        /// </summary>
        public int Height { get { return _drawingRect.Height; } }

        public GameObject(Texture2D texture)
        {
            Texture = texture;
        }

        public GameObject(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        /// <summary>
        /// Updates object logic.
        /// </summary>
        /// <param name="gameTime">The current elapsed game time.</param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Draws the object to screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to draw to.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, _drawingRect, Color.White);
        }
    }
}
