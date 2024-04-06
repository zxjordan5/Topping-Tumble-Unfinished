// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ToppingTumble.UI;

namespace ToppingTumble
{
    /// <summary>
    /// Handles the game loop for the level victory screen.
    /// </summary>
    internal class VictoryState : BaseState
    {
        // Fields
        private UIButton _returnButton;
        private UIButton _nextButton;

        public VictoryState()
        {
            // Initialize stuff here. Content will already have been loaded once this is called

            _returnButton = new UIButton(ContentLoader.TexFridge,
                                         ContentLoader.TexOven,
                                         new Vector2(GameMain.RenderTargetWidth * 0.475f, GameMain.RenderTargetHeight * 0.5f),
                                         "Return to Menu");
            _returnButton.ClickEvent += ReturnClick;

            _nextButton = new UIButton(ContentLoader.TexFridge,
                                       ContentLoader.TexOven,
                                       new Vector2(GameMain.RenderTargetWidth * 0.475f, GameMain.RenderTargetHeight * 0.65f),
                                       "Next Level");
            _nextButton.ClickEvent += NextClick;
        }

        public override void Begin()
        {
            /* Called when this becomes the CurrentState in GameMain. If something needs
             * reset every time the state loads, do so here */
        }

        public override void Update(GameTime gameTime)
        {
            // Update here

            _returnButton.Update(gameTime);
            if (!GameMain.Instance.Gameplay.IsLastLevel)
                _nextButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw here

            _returnButton.Draw(spriteBatch);
            if (!GameMain.Instance.Gameplay.IsLastLevel)
                _nextButton.Draw(spriteBatch);

            // Victory text
            UIText.DrawString(spriteBatch, ContentLoader.FntPixelBold, "Victory!",
                              new Vector2(GameMain.RenderTargetWidth * 0.5f, GameMain.RenderTargetHeight * 0.4f));
        }

        /// <summary>
        /// Change the state to the level select.
        /// </summary>
        private void ReturnClick(object sender, System.EventArgs e)
        {
            GameMain.Instance.CurrentState = GameMain.Instance.LevelSelect;
        }

        /// <summary>
        /// Change the state to the next level.
        /// </summary>
        private void NextClick(object sender, System.EventArgs e)
        {
            GameMain.Instance.LevelSelect.StartLevel(GameMain.Instance.Gameplay.LevelIndex + 1);
            GameMain.Instance.CurrentState = GameMain.Instance.Gameplay;
        }
    }
}
