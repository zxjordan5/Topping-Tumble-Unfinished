// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace ToppingTumble
{
    /// <summary>
    /// Handles the game loop for the main menu screen.
    /// </summary>
    internal class MainMenuState : BaseState
    {
        private UIButton _startButton;
        private UIButton _optionsButton;
        private UIButton _quitButton;

        public MainMenuState()
        {
            // Initialize stuff here. Content will already have been loaded once this is called

            // UI Button(s) for main menu state
            _startButton = new UIButton(ContentLoader.TexButtonPink, ContentLoader.TexButtonPinkHover,
                Vector2.Zero, "START");
            _startButton.PositionCenter = new Vector2(216, 88);
            _startButton.ClickEvent += StartButton_ClickEvent;

            _optionsButton = new UIButton(ContentLoader.TexButtonYellow, ContentLoader.TexButtonYellowHover,
                Vector2.Zero, "OPTIONS");
            _optionsButton.PositionCenter = new Vector2(216, 112);

            _quitButton = new UIButton(ContentLoader.TexButtonGreen, ContentLoader.TexButtonGreenHover,
                Vector2.Zero, "QUIT");
            _quitButton.PositionCenter = new Vector2(216, 136);
            _quitButton.ClickEvent += QuitButton_ClickEvent;
        }

        public override void Begin()
        {
            /* Called when this becomes the CurrentState in GameMain. If something needs
             * reset every time the state loads, do so here */
        }

        public override void Update(GameTime gameTime)
        {
            // Update here

            _startButton.Update(gameTime);
            _optionsButton.Update(gameTime);
            _quitButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw here
            spriteBatch.Draw(ContentLoader.TexMainMenuBackground, new Vector2(0, 0), Color.White);
            _startButton.Draw(spriteBatch);
            _optionsButton.Draw(spriteBatch);
            _quitButton.Draw(spriteBatch);
        }

        // Button Click Events
        /// <summary>
        /// When clicked, set game state to gameplay.
        /// </summary>
        private void StartButton_ClickEvent(object sender, System.EventArgs e)
        {
            GameMain.Instance.CurrentState = GameMain.Instance.LevelSelect;
        }
        /// <summary>
        /// When clicked, exit the game
        /// </summary>
        private void QuitButton_ClickEvent(object sender, System.EventArgs e)
        {
            GameMain.Instance.Exit();
        }
    }
}
