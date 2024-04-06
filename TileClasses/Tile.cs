using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ToppingTumble
{
    /// <summary>
    /// Represents a generic tile in the game.
    /// </summary>
    internal class Tile
    {
        // Fields
        protected Texture2D _texture;
        protected bool _initializedMapPos;
        protected int _mapRowPos;
        protected int _mapColPos;
        private bool _isEditable;
        private bool _isWalkable;

        // Properties
        /// <summary>
        /// Gets and sets the texture of this tile
        /// </summary>
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }

        /// <summary>
        /// Returns the bounding/collision box for this tile. THIS MAY NOT NECESSARILY BE A FULL TILE!
        /// </summary>
        public virtual Rectangle TileRect
        {
            get
            {
                return new Rectangle(_mapColPos * 16, _mapRowPos * 16, 16, 16);
            }
        }

        /// <summary>
        /// True when the tile is editable by the player.
        /// </summary>
        public virtual bool IsEditable { get { return _isEditable; } }
        /// <summary>
        /// True when ingredients can collide with this tile.
        /// </summary>
        public virtual bool IsWalkable { get { return _isWalkable; } }

        // Constructor
        /// <summary>
        /// Makes a new tile for the game
        /// </summary>
        /// <param name="texture">Image for the tile</param>
        /// <param name="x">Its x position in the tilemap</param>
        /// <param name="y">Its y position in the tilemap</param>
        /// <param name="editable">Whether or not the player can edit the tile</param>
        public Tile(Texture2D image, bool editable, bool walkable)
        {
            // Set fields to corresponding parameters
            _texture = image;
            _isEditable = editable;
            _isWalkable = walkable;
        }

        /// <summary>
        /// Initializes the col, row map position for this tile. Should only be called ONCE and ONLY by TileMap.
        /// </summary>
        public void InitiailizeMapPos(int x, int y)
        {
            if (_initializedMapPos)
                throw new InvalidOperationException("Cannot initialize a tile's position twice!");
            _initializedMapPos = true;
            _mapRowPos = y;
            _mapColPos = x;
        }

        // Methods
        /// <summary>
        /// What to do when an ingredient collides with this tile,
        /// not really useful for Tile.cs, MEANT TO BE OVERRIDDEN
        /// </summary>
        /// <param name="ingredient">The ingredient that is colliding with the tile</param>
        public virtual void OnCollision(Ingredient ingredient) 
        {}

        /// <summary>
        /// Updates the tile, doesn't do anything in parent class
        /// (different tiles update differently)
        /// </summary>
        /// <param name="gameTime">object to tell how much time has passed</param>
        public virtual void Update(GameTime gameTime)
        {
            // Do nothing
        }

        /// <summary>
        /// Draws the tile to the screen
        /// </summary>
        /// <param name="sb">A SpriteBatch object to draw with</param>
        /// <param name="x">x position to draw it</param>
        /// <param name="y">y position to draw it</param>
        public virtual void Draw(SpriteBatch sb, int x, int y)
        {
            // Draw the texture to the screen
            sb.Draw(_texture,
                    new Rectangle(x, y, _texture.Width, _texture.Height),
                    Color.White);
        }
        

    }
}
