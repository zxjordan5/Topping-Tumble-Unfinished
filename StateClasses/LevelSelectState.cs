// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using ToppingTumble.UI;

namespace ToppingTumble
{
    /// <summary>
    /// Handles the game loop for the level select screen.
    /// </summary>
    internal class LevelSelectState : BaseState
    {
        /// <summary>
        /// Stores the grouped data for levels.
        /// </summary>
        public LevelData[] Levels { get; private set; }

        private UIButton _backButton;

        private List<LevelButton> _levelButtons;

        public LevelSelectState()
        {
            // Initialize level data
            Levels = new LevelData[]
            {
                TileMap.LoadLevelData("Content/Levels/Level1.lvl"),
                TileMap.LoadLevelData("Content/Levels/Level2.lvl"),
                TileMap.LoadLevelData("Content/Levels/Level3.lvl"),
                TileMap.LoadLevelData("Content/Levels/SwitchDemo.lvl")
            };

            // Initialize stuff here. Content will already have been loaded once this is called
            _backButton = new UIButton(ContentLoader.TexFridge, ContentLoader.TexOven,
                new Vector2(8.0f, 8.0f), "Back");
            _backButton.ClickEvent += StartButton_ClickEvent;

            // Create level buttons
            Vector2 buttonsStartPosition = new Vector2(32.0f, 54.0f);
            _levelButtons = new List<LevelButton>();
            for (int i = 0; i < Levels.Length; i++)
            {
                LevelButton button = new LevelButton(Levels[i], i + 1,
                    new Vector2(buttonsStartPosition.X + i * 48.0f, buttonsStartPosition.Y));

                // When the button is clicked, it should start the given level
                int index = i;
                button.ClickEvent += delegate { StartLevel(index); };
                _levelButtons.Add(button);
            }
        }

        public override void Begin()
        {
            /* Called when this becomes the CurrentState in GameMain. If something needs
             * reset every time the state loads, do so here */
        }

        public override void Update(GameTime gameTime)
        {
            // Update here

            _backButton.Update(gameTime);

            foreach (LevelButton button in _levelButtons)
                button.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw here

            _backButton.Draw(spriteBatch);

            foreach (LevelButton button in _levelButtons)
                button.Draw(spriteBatch);
        }

        /// <summary>
        /// Starts the level with the given index.
        /// </summary>
        /// <param name="levelIndex">The level index.</param>
        public void StartLevel(int levelIndex)
        {
            GameMain.Instance.Gameplay.LoadLevel(levelIndex);
            GameMain.Instance.CurrentState = GameMain.Instance.Gameplay;
        }

        // Button Click Events
        /// <summary>
        /// When clicked, set game state to main menu.
        /// </summary>
        private void StartButton_ClickEvent(object sender, System.EventArgs e)
        {
            GameMain.Instance.CurrentState = GameMain.Instance.MainMenu;
        }
    }
}

