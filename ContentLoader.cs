// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ToppingTumble.UI;

namespace ToppingTumble
{
    /// <summary>
    /// Stores and allows access to any assets in the game.
    /// </summary>
    internal static class ContentLoader
    {
        // Textures (name prefixed with Tex)
        public static Texture2D TexCursor { get; private set; }
        public static Texture2D TexTestingTerry { get; private set; }
        public static Texture2D TexIngredientSheet { get; private set; }
        public static Texture2D TexMainMenuBackground { get; private set; }
        public static Texture2D TexScrollBackground { get; private set; }
        public static Texture2D TexFontPixelBold { get; private set; }
        public static Texture2D TexFontPixelSmall { get; private set; }
        public static Texture2D TexButtonGreen { get; private set; }
        public static Texture2D TexButtonGreenHover { get; private set; }
        public static Texture2D TexButtonPink { get; private set; }
        public static Texture2D TexButtonPinkHover { get; private set; }
        public static Texture2D TexButtonYellow { get; private set; }
        public static Texture2D TexButtonYellowHover { get; private set; }
        public static Texture2D TexLevelSelectButton { get; private set; }
        public static Texture2D TexLevelSelectButtonHover { get; private set; }
        public static Texture2D TexLevelSelectCoin { get; private set; }
        public static Texture2D TexPlaceTile { get; private set; }
        public static Texture2D TexPlaceTileset { get; private set; }
        public static Texture2D TexStaticTileset { get; private set; }
        public static Texture2D TexTileHighlight { get; private set; }
        public static Texture2D TexFridge { get; private set; }
        public static Texture2D TexOven { get; private set; }
        public static Texture2D TexSpring { get; private set; }
        public static Texture2D TexSpringStill { get; private set; }
        public static Texture2D TexCoin { get; private set; }
        public static Texture2D TexSpikeTile { get; private set; }
        public static Texture2D TexSwitchBlock { get; private set; }
        public static Texture2D TexSwitch { get; private set; }

        // Fonts (name prefixed with Fnt)
        public static UIFont FntPixelBold { get; private set; }
        public static UIFont FntPixelSmall { get; private set; }

        // Sounds (name prefixed with Snd)

        /// <summary>
        /// Loads and stores all game content in static variables.
        /// </summary>
        public static void LoadAllContent(ContentManager content)
        {
            // Load textures
            TexCursor = content.Load<Texture2D>("cursor");

            TexTestingTerry = content.Load<Texture2D>("testingTerry");
            TexIngredientSheet = content.Load<Texture2D>("ingredientSheet");

            TexMainMenuBackground = content.Load<Texture2D>("mainMenuBackground");
            TexScrollBackground = content.Load<Texture2D>("scrollBackground");

            TexFontPixelBold = content.Load<Texture2D>("fontPixelBold");
            TexFontPixelSmall = content.Load<Texture2D>("fontPixelSmall");

            TexButtonGreen = content.Load<Texture2D>("Buttons/buttonGreen");
            TexButtonGreenHover = content.Load<Texture2D>("Buttons/buttonGreenHover");
            TexButtonPink = content.Load<Texture2D>("Buttons/buttonPink");
            TexButtonPinkHover = content.Load<Texture2D>("Buttons/buttonPinkHover");
            TexButtonYellow = content.Load<Texture2D>("Buttons/buttonYellow");
            TexButtonYellowHover = content.Load<Texture2D>("Buttons/buttonYellowHover");
            TexLevelSelectButton = content.Load<Texture2D>("Buttons/levelSelectButton");
            TexLevelSelectButtonHover = content.Load<Texture2D>("Buttons/levelSelectButtonHover");
            TexLevelSelectCoin = content.Load<Texture2D>("Buttons/levelSelectCoin");

            TexPlaceTileset = content.Load<Texture2D>("placeTileset");
            TexStaticTileset = content.Load<Texture2D>("staticTileset");
            TexPlaceTile = content.Load<Texture2D>("placeTile");
            TexTileHighlight = content.Load<Texture2D>("tileHighlight");
            TexFridge = content.Load<Texture2D>("fridge");
            TexOven = content.Load<Texture2D>("oven");
            TexSpring = content.Load<Texture2D>("spring");
            TexSpringStill = content.Load<Texture2D>("springStill");
            TexCoin = content.Load<Texture2D>("coin");
            TexSpikeTile = content.Load<Texture2D>("knifeTile");
            TexSwitchBlock = content.Load<Texture2D>("Tiles/switchBlock");
            TexSwitch = content.Load<Texture2D>("Tiles/switch");

            // Load fonts
            FntPixelBold = new UIFont(TexFontPixelBold, 9, 16, -1);
            FntPixelSmall = new UIFont(TexFontPixelSmall, 6, 7, -1);

            // Load sounds
        }
    }
}
