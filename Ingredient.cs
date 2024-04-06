using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ToppingTumble.TileClasses;

namespace ToppingTumble
{
    /// <summary>
    /// Class that represents the actual moving ingredients
    /// </summary>
    internal class Ingredient : GameObject
    {
        public const float DefaultMoveSpeed = 40.0f;
        public const float DefaultGravity = 280.0f;
        public const float DefaultHorizontalIncrement = 20.0f;

        // Fields

        // Constant speed ingredients move at
        Vector2 _movespeed;

        // Gravity vector to apply
        Vector2 _gravity;

        // Horizontal acceleration vector for speed boosts
        Vector2 _horizontalAccel;

        private List<Tile> _tilesCollidedLastFrame;

        // Animation Fields
        private Random _ran;
        private AnimationController _animation;
        private bool _isDying;
        private float _constantRotationValue;

        /// <summary>
        /// Constructor for GameObject that constructs the ingredient
        /// with aa texture and position
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        public Ingredient(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            _movespeed = new Vector2(DefaultMoveSpeed, 0);
            _gravity = new Vector2(0, DefaultGravity);
            _horizontalAccel = new Vector2(0, 0);

            // Animation Field Initialization
            _ran = new Random();
            _animation = new AnimationController(4, 4.5, _ran.Next(0, 5));
            _animation.Play();

            // An ingredient should not be dying on spawn
            _isDying = false;

            // Establish constant rotation value for death anim
            // Rotate at a rate of 1 rev/.5s
            // (2pi rad/30 frames)
            _constantRotationValue = 0;

            _tilesCollidedLastFrame = new List<Tile>();
        }

        /// <summary>
        /// Gets the current collision rectangle for the ingredient. Is smaller than 1 tile to help
        /// ingredients not get stuck in 1x1 tile spaces.
        /// </summary>
        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle(DrawingRect.X + 2, DrawingRect.Y + 4,
                    DrawingRect.Width - 4, DrawingRect.Height - 4);
            }
        }

        /// <summary>
        /// Gets and sets the IsDying boolean value
        /// </summary>
        public bool IsDying { get { return _isDying; } set { _isDying = value; } }


        /// <summary>
        /// Gets and sets the MoveSpeed Vector for the ingredient
        /// </summary>
        public Vector2 MoveSpeed { get { return _movespeed; }
            set
            {
                _movespeed.X = value.X;
                _movespeed.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets and sets the Horizontal Accleration of the ingredient
        /// </summary>
        public Vector2 HorizontalAcceleration { get { return _horizontalAccel; }
            set
            {
                _horizontalAccel.X = value.X;
                _horizontalAccel.Y = value.Y;
            }
        }


        /// <summary>
        /// Gets and sets the constant rotation value
        /// </summary>
        private float ConstantRotationSpeed
        {
            get
            {
                return _constantRotationValue;
            }
            set
            {
                _constantRotationValue = value;
            }
        }


        /// <summary>
        /// Checks the collision of the ingredients with 
        /// the tilemap and resolves them so that ingredients
        /// walk across the level
        /// </summary>
        /// <param name="map">The tilemap of the level</param>
        public void ResolveCollisions(TileMap tm)
        {
            // See which tiles the ingredient COULD be on

            // Make 2 indices that represent where the ingredient
            // is in the tile array for rows and columns
            int columnRangeMin = (int)this.X / this.Width;
            int columnRangeMax = (int)(this.X + this.Width) / this.Width;

            int rowRangeMin = (int)this.Y / this.Height;
            int rowRangeMax = (int)(this.Y + this.Height) / this.Height;

            Rectangle collisionRect = CollisionRect;

            // Loop through possible indices to see if its
            // colliding with anything on the map
            List<Tile> collidingTiles = new List<Tile>();

            for(int r = rowRangeMin; r <= rowRangeMax; r++)
            {
                for(int c = columnRangeMin; c <= columnRangeMax; c++)
                { 
                    if(tm.IsPositionInBounds(c, r))
                    {
                        Tile tile = tm.GetTile(c, r);
                        if (tile != null && tile.TileRect.Intersects(collisionRect))
                        {
                            // Add intersecting tile to the map
                            collidingTiles.Add(tile);
                        }
                    }
                }
            }

            // List to store overlapRectangles and combine them if needed
            List<Rectangle> overlapRectangles = new List<Rectangle>();

            // Loop through colliding tiles and decide what to do
            // with each one
            foreach (Tile tile in collidingTiles)
            {
                if (tile.IsWalkable) // If the tile is a floor, adjust position
                {
                    // Get the overlapping rectangle
                    Rectangle overlapRect = Rectangle.Intersect(collisionRect, tile.TileRect);
                    overlapRectangles.Add(overlapRect);

                    // Kill ingredients if they get too far stuck in a door tile
                    if (tile is SwitchDoorTile &&
                        overlapRect.Width >= collisionRect.Width - 2 &&
                        overlapRect.Height >= collisionRect.Height - 2)
                    {
                        GameMain.Instance.Gameplay.IngredientDeath(this);
                    }
                }
            }

            // Loop through and combine overlapping rectangles for
            // ease of collision
            for (int x = 0; x < overlapRectangles.Count; x++)
            {
                for (int j = x + 1; j < overlapRectangles.Count; j++)
                {
                    if (overlapRectangles[x].Y == overlapRectangles[j].Y)
                    {
                        // Combine the two rectangles into one
                        overlapRectangles[x] = new Rectangle
                            (overlapRectangles[x].X,
                            overlapRectangles[x].Y,
                            overlapRectangles[x].Width + overlapRectangles[j].Width,
                            overlapRectangles[x].Height);
                        overlapRectangles.RemoveAt(j);
                    }
                }
            }

            // Loop through again and resolve all collisions!
            for (int y = 0; y < overlapRectangles.Count; y++)
            {
                ResolveWallFloorCollision(overlapRectangles[y], collisionRect);
            }

            // Call OnCollision methods
            foreach (Tile tile in collidingTiles)
            {
                // Only call the onCollision if this tile was not collided last frame
                if (!_tilesCollidedLastFrame.Contains(tile))
                    tile.OnCollision(this);
            }

            _tilesCollidedLastFrame = collidingTiles;
        }

        /// <summary>
        /// Will resolve wall and floor collisions
        /// by adjusting the ingredient's position
        /// </summary>
        /// <param name="overlapRect">The rectangle overlapping with the player
        /// /wall tile being collided with</param>
        public void ResolveWallFloorCollision(Rectangle overlapRect, Rectangle collisionRect)
        {
            // Get ingredient's rectangle and store it
            Rectangle ingredientRect = collisionRect;
            
            

            if (overlapRect.Height <= overlapRect.Width)
            { 
                // Have to resolve a vertical collision
                if (overlapRect.Y > ingredientRect.Y)
                {
                    ingredientRect.Y -= overlapRect.Height;
                }
                else
                {
                    ingredientRect.Y += overlapRect.Height;
                }
                _movespeed.Y = 0;
            }
            else
            {
                // Have to resolve a horizontal collision
                if (overlapRect.X > ingredientRect.X)
                {
                    // Coming in from the left
                    // Shift to the left
                    ingredientRect.X -= overlapRect.Width + 2;
                }
                else
                {
                    // Coming in from the right
                    // Shift to the right
                    ingredientRect.X += overlapRect.Width + 2;
                }
                // Swap movement
                if ((ingredientRect.X + 2) % 16 == 0 || ((ingredientRect.X - 2) % 16 == 0))
                {
                    _movespeed.X *= -1;
                }
            }

            // Set position of ingredient
            // using copy/alter/replace method

            Vector2 posCopy = Position;
            posCopy.X = ingredientRect.X - 2;
            posCopy.Y = ingredientRect.Y - 4;
            Position = posCopy;
        }

        /// <summary>
        /// Applies gravity and horizontal
        /// movement to the ingredient
        /// </summary>
        private void ApplyMovement(GameTime gameTime)
        {
            // Add gravity to ingredient velocity
            _movespeed += _gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Decrease horizontal acceleration for constant slowdown IF
            // the ingredient has been sped up in some way
            if(Math.Abs(MoveSpeed.X) >  DefaultMoveSpeed)
            {
                if (MoveSpeed.X > 0)
                {
                    // Moving to the right, increment acceleration in the negative direction
                    _horizontalAccel.X = -1 * DefaultHorizontalIncrement;
                }
                else
                {
                    // Moving to the left, increment acceleration in the positive direction
                    _horizontalAccel.X = DefaultHorizontalIncrement;
                }
            }
            else
            {
                _horizontalAccel.X = 0;
            }
            

            _movespeed += _horizontalAccel * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Correction for errors with speed tiles
            if(Math.Abs(MoveSpeed.X) < DefaultMoveSpeed) 
            {
                float error = DefaultMoveSpeed - Math.Abs(MoveSpeed.X);

                Vector2 speedCopy = MoveSpeed;
                speedCopy.X += error;
                MoveSpeed = speedCopy;
            }

            // Add velocity to position
            Position += _movespeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Debug.WriteLine("Horizontal Acceleration: " + _horizontalAccel);
            // Debug.WriteLine("Move Speed: " + _movespeed);
        }

        public override void Update(GameTime gameTime)
        {
            ApplyMovement(gameTime);

            // Calls current GameplayState's level to check for collisions
            if (!IsDying)
            {
                // Don't want collisions to be resolved for the dying animation
                ResolveCollisions(GameMain.Instance.Gameplay.LevelMap);
            }
            else
            {
                // Adjust rotation value
                ConstantRotationSpeed += (float)(2 * Math.PI) / 20;

                // Adjust X placement for smoothness
                // Vector2 posCopy = Position;
                // posCopy.X = 2 * DefaultMoveSpeed;
                // Position = posCopy;

                if(ConstantRotationSpeed % (float)(2*Math.PI) == 0)
                {
                    ConstantRotationSpeed = 0;
                }
            }

            _animation.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the ingredient based on which frame and variant it is
        /// </summary>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the ingredient facing a different way depending on direction
            
            if(IsDying)
            {
                // Custom draw for dying ingredient
                spriteBatch.Draw(ContentLoader.TexIngredientSheet,
                    base.DrawingRect,
                    _animation.SourceRect,
                    Color.White,
                    ConstantRotationSpeed,
                    new Vector2(0, 0),
                    SpriteEffects.None,
                    0.0f);
            }
            else
            {
                if (MoveSpeed.X > 0)
                {
                    // Draw ingredient facing right
                    spriteBatch.Draw(ContentLoader.TexIngredientSheet,
                                 base.DrawingRect,
                                 _animation.SourceRect,
                                 Color.White);
                }
                else
                {
                    // Draw ingredient facing left
                    spriteBatch.Draw(ContentLoader.TexIngredientSheet,
                                 base.DrawingRect,
                                 _animation.SourceRect,
                                 Color.White,
                                 0.0f,
                                 new Vector2(0, 0),
                                 SpriteEffects.FlipHorizontally,
                                 0.0f);
                }
            }
            
        }
    }
}
