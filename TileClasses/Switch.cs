// Don't Put me on the Spot, 4/3/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToppingTumble.TileClasses
{
    /// <summary>
    /// A switch which can toggle door tiles.
    /// </summary>
    internal class Switch : Tile
    {
        private AnimationController _animation;
        public SwitchColor TargetColor { get; private set; }

        public Switch(SwitchColor targetColor) : base(ContentLoader.TexSwitch, false, false)
        {
            TargetColor = targetColor;
            switch (targetColor)
            {
                case SwitchColor.Yellow:
                    _animation = new AnimationController(4, 16, 0);
                    break;
                case SwitchColor.Green:
                    _animation = new AnimationController(4, 16, 1);
                    break;
                case SwitchColor.Purple:
                    _animation = new AnimationController(4, 16, 2);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb, int x, int y)
        {
            sb.Draw(_texture, base.TileRect,
                _animation.SourceRect, Color.White);
        }

        public override void OnCollision(Ingredient ingredient)
        {
            base.OnCollision(ingredient);
            ToggleLinkedDoors();

            // Play animation
            _animation.PlayOnce();
        }

        /// <summary>
        /// Updates doors of the associated color and toggles their current state.
        /// </summary>
        public void ToggleLinkedDoors()
        {
            TileMap tileMap = GameMain.Instance.Gameplay.LevelMap;
            List<SwitchDoorTile> doorTiles = null;
            switch (TargetColor)
            {
                case SwitchColor.Yellow:
                    doorTiles = tileMap.YellowDoors;
                    break;

                case SwitchColor.Green:
                    doorTiles = tileMap.GreenDoors;
                    break;

                case SwitchColor.Purple:
                    doorTiles = tileMap.PurpleDoors;
                    break;
            }

            foreach (SwitchDoorTile doorTile in doorTiles)
                doorTile.IsEnabled = !doorTile.IsEnabled;
        }
    }

    internal enum SwitchColor
    {
        Yellow,
        Green,
        Purple
    }
}
