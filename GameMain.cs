// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ToppingTumble
{
    /// <summary>
    /// Handles overarching game loop logic.
    /// </summary>
    internal class GameMain : Game
    {
        /// <summary>
        /// If states or GameObjects need to access data from elsewhere, they can do so by starting
        /// at GameMain. It is assumed there will only ever be one GameMain.
        /// </summary>
        public static GameMain Instance { get; private set; }

        /// <summary>
        /// The screen width rendered to. Not the actual window size.
        /// </summary>
        public const int RenderTargetWidth = 320;
        /// <summary>
        /// The screen height rendered to. Not the actual window size.
        /// </summary>
        public const int RenderTargetHeight = 180;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget; // The texture rendered to and upscaled to fit the screen
        private Rectangle _renderTargetRect; // Used to position the render texture in the window

        private GameObject scrollBackground; // Scrolling background rendered in every game scene

        private BaseState _currentState;

        /// <summary>
        /// Gets or sets the current state of the game. The current state handles game logic/rendering.
        /// </summary>
        public BaseState CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                _currentState.Begin();
            }
        }

        /// <summary>
        /// Variables and logic for the main menu screen.
        /// </summary>
        public MainMenuState MainMenu { get; private set; }
        /// <summary>
        /// Variables and logic for the level select screen.
        /// </summary>
        public LevelSelectState LevelSelect { get; private set; }
        /// <summary>
        /// Variables and logic for the main gameplay/editing scene.
        /// </summary>
        public GameplayState Gameplay { get; private set; }
        /// <summary>
        /// Variables and logic for the fail state screen.
        /// </summary>
        public GameOverState GameOver { get; private set; }
        /// <summary>
        /// Variables and logic for the level victory screen.
        /// </summary>
        public VictoryState Victory { get; private set; }

        public GameMain()
        {
            Instance = this;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = RenderTargetWidth * 4; // Default screen scale multiplier
            _graphics.PreferredBackBufferHeight = RenderTargetHeight * 4; // Default screen scale multiplier
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes any important values. State creation is not handled here since
        /// it might require that content be loaded first.
        /// </summary>
        protected override void Initialize()
        {
            _renderTarget = new RenderTarget2D(GraphicsDevice, RenderTargetWidth, RenderTargetHeight);
            UpdateRenderTargetRect();

            Window.Title = "Topping Tumble"; // Change window title so it has a space
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(ClientSizeChanged);

            // Hide user's mouse to allow for custom mouse sprite
            Instance.IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// Loads content on the ContentLoader class and creates/initializes states.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLoader.LoadAllContent(Content);

            scrollBackground = new GameObject(ContentLoader.TexScrollBackground);

            MainMenu = new MainMenuState();
            LevelSelect = new LevelSelectState();
            Gameplay = new GameplayState();
            GameOver = new GameOverState();
            Victory = new VictoryState();

            CurrentState = MainMenu; // Set initial state
        }

        /// <summary>
        /// Updates the current game state.
        /// </summary>
        /// <param name="gameTime">The current elapsed game time.</param>
        protected override void Update(GameTime gameTime)
        {
            CurrentState.Update(gameTime); // Update the current state

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Begin rendering
            GraphicsDevice.SetRenderTarget(_renderTarget);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, samplerState: SamplerState.PointClamp);

            DrawScrollBackground(gameTime); // Render scrolling background texture

            CurrentState.Draw(_spriteBatch); // Render the current state

            DrawCustomCursor(gameTime);     // Render custom cursor texture

            // Finish rendering
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            // Display the rendered texture in the window
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            _spriteBatch.Draw(_renderTarget, _renderTargetRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Called when the window is resized and changes the back buffer/render target to fit that.
        /// </summary>
        private void ClientSizeChanged(object sender, EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphics.ApplyChanges();
            UpdateRenderTargetRect();
        }

        /// <summary>
        /// Calculates the scaled and centered screen rect for the render target.
        /// </summary>
        private void UpdateRenderTargetRect()
        {
            float scale;
            float screenAspectRatio = (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight;
            float rtAspectRatio = (float)_renderTarget.Width / _renderTarget.Height;
            if (screenAspectRatio > rtAspectRatio)
                scale = (float)_graphics.PreferredBackBufferHeight / _renderTarget.Height;
            else
                scale = (float)_graphics.PreferredBackBufferWidth / _renderTarget.Width;

            _renderTargetRect = new Rectangle(0, 0, (int)(_renderTarget.Width * scale), (int)(_renderTarget.Height * scale));
            _renderTargetRect.Location = new Point((_graphics.PreferredBackBufferWidth - _renderTargetRect.Width) / 2,
                (_graphics.PreferredBackBufferHeight - _renderTargetRect.Height) / 2);
        }

        /// <summary>
        /// Returns the mouse position scaled to fit the current upscaled screen size.
        /// </summary>
        /// <returns>The accurate mouse position.</returns>
        public Vector2 GetMousePosition()
        {
            Vector2 position = Mouse.GetState().Position.ToVector2();
            Vector2 rtLocation = _renderTargetRect.Location.ToVector2();
            Vector2 rtSize = _renderTargetRect.Size.ToVector2();
            Vector2 rtActualSize = new Vector2(_renderTarget.Width, _renderTarget.Height);
            return (position - rtLocation) / rtSize * rtActualSize;
        }

        /// <summary>
        /// Moves/loops and draws the scrolling background texture
        /// </summary>
        private void DrawScrollBackground(GameTime gameTime)
        {
            float scrollPos = scrollBackground.X + 32.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (scrollPos >= 0) scrollPos = -32;
            scrollBackground.Position = new Vector2(scrollPos, scrollPos);
            scrollBackground.Draw(_spriteBatch);
        }

        /// <summary>
        /// Draws a custom cursor texture at the mouse's screen position
        /// </summary>
        public void DrawCustomCursor(GameTime gameTime)
        {
            Vector2 position = GetMousePosition();
            // Round mouse position to a pixel-aligned position
            int x = (int)position.X;
            int y = (int)position.Y;
            _spriteBatch.Draw(ContentLoader.TexCursor, new Vector2(x, y), Color.White);
        }
    }
}