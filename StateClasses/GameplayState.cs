// Don't Put me on the Spot, 3/4/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using ToppingTumble.UI;

namespace ToppingTumble
{
    /// <summary>
    /// Handles the game loop for the main gameplay/editing scene.
    /// </summary>
    internal class GameplayState : BaseState
    {
        /// <summary>
        /// The index of the level from the level select that is currently loaded.
        /// </summary>
        public int LevelIndex { get; private set; }
        /// <summary>
        /// True when the currently-loaded level is the last one.
        /// </summary>
        public bool IsLastLevel { get { return LevelIndex >= GameMain.Instance.LevelSelect.Levels.Length - 1; } }
        /// <summary>
        /// Gets the currently-loaded tilemap for the level.
        /// </summary>
        public TileMap LevelMap { get; private set; }

        /// <summary>
        /// The current remaining currency in the level.
        /// </summary>
        public int CurrentCurrency { get; private set; }
        /// <summary>
        /// The current number of coins collected.
        /// </summary>
        public int CoinsCollected { get; set; }

        /// <summary>
        /// Use to access shop data.
        /// </summary>
        public ShopData ShopItemsData { get; private set; }
        /// <summary>
        /// Gest the index of the shop item to be placed. -1 when no item is selected.
        /// </summary>
        public int SelectedShopIndex { get; private set; }
        /// <summary>
        /// True when the player has selected an item from the shop and is placing it.
        /// </summary>
        public bool PlacingShopItem { get; private set; }
        /// <summary>
        /// True while allowed to place or remove tiles.
        /// </summary>
        public bool IsAllowedToEdit { get { return !IngredientsSpawning; } }

        /// <summary>
        /// Delegate for resetting the game
        /// </summary>
        public delegate void ResetGameDelegate();

        /// <summary>
        /// Event that resets the game
        /// </summary>
        public event ResetGameDelegate OnResetGame;

        private List<ShopButton> _shopButtons;

        private bool _tileHighlightValid;
        private TilePhantom _tilePhantom;
        private GameObject _tileHighlight;

        // Spawning fields
        private List<Ingredient> _ingredients;
        private bool _startSpawning;
        private int _numIngredients;
        private double _spawnTimer;
        private UIButton _spawnButton;

        /// <summary>
        /// True while ingredients are present or spawning.
        /// </summary>
        public bool IngredientsSpawning { get { return _startSpawning; } }

        // Reset fields
        private UIButton _resetButton;

        // Win condition fields
        private int _ingredientsPassed;

        public GameplayState()
        {
            // Initialize stuff here. Content will already have been loaded once this is called

            DeselectShopItem();

            _tilePhantom = new TilePhantom();
            _tileHighlight = new GameObject(ContentLoader.TexTileHighlight);

            // Initialize the spawning button
            _spawnButton = new UIButton(ContentLoader.TexFridge,
                                        ContentLoader.TexOven,
                                        new Vector2(GameMain.RenderTargetWidth * 0.4f - 8.0f, 4.0f),
                                        "Spawn");
            _spawnButton.ClickEvent += SpawnIngredients;

            // Initialize the reset button
            _resetButton = new UIButton(ContentLoader.TexFridge,
                                        ContentLoader.TexOven,
                                        new Vector2(GameMain.RenderTargetWidth * 0.6f - 8.0f, 4.0f),
                                        "Reset");
            _resetButton.ClickEvent += ResetIngredients;
        }

        public override void Begin()
        {
            /* Called when this becomes the CurrentState in GameMain. If something needs
             * reset every time the state loads, do so here */

            ShopItemsData = new ShopData(LevelMap.EnabledItems);

            DeselectShopItem();

            // Create shop buttons
            Vector2 buttonsStartPosition = new Vector2(8.0f, GameMain.RenderTargetHeight - 18.0f);
            _shopButtons = new List<ShopButton>();
            for (int i = 0; i < ShopItemsData.Items.Length; i++)
            {
                ShopButton button = new ShopButton(ShopItemsData.Items[i],
                    new Vector2(buttonsStartPosition.X + i * 24.0f, buttonsStartPosition.Y));

                // When the button is clicked, it should select the given item
                int index = i;
                button.ClickEvent += delegate { SelectShopItem(index); };
                _shopButtons.Add(button);
            }

            // Initialize ingredient list
            _ingredients = new List<Ingredient>();

            // Spawning field initialization
            _startSpawning = false;
            _numIngredients = 0;
            _spawnTimer = 1;

            // Win condition initialization
            _ingredientsPassed = 0;
        }

        /// <summary>
        /// Loads the level with the given level select index.
        /// </summary>
        /// <param name="file">The index of the level.</param>
        public void LoadLevel(int index)
        {
            OnResetGame = null;
            LevelIndex = index;
            LevelMap = new TileMap(GameMain.Instance.LevelSelect.Levels[index]);
            CurrentCurrency = LevelMap.Currency;
            CoinsCollected = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 mouseGridPos = GetMouseGridPosition();
            int gridPosX = (int)mouseGridPos.X;
            int gridPosY = (int)mouseGridPos.Y;
            bool gridPosValid = MapPosInPlaceableBounds(gridPosX, gridPosY) && IsAllowedToEdit;

            // Check if the user left-clicked on a tile
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && gridPosValid)
                LeftClickedTile(gridPosX, gridPosY);

            // Check if the user right-clicked on a tile
            if (Mouse.GetState().RightButton == ButtonState.Pressed && gridPosValid)
                RightClickedTile(gridPosX, gridPosY);

            // Update the shop
            foreach (ShopButton button in _shopButtons)
                button.Update(gameTime);

            // Update the tile highlight position
            _tilePhantom.Position = mouseGridPos * 16.0f;
            _tileHighlight.Position = mouseGridPos * 16.0f;

            LevelMap.UpdateTiles(gameTime);
            // Update ingredients
            for (int i = 0; i < _ingredients.Count; i++)
                _ingredients[i].Update(gameTime);

            _tilePhantom.Update(gameTime);
            _tileHighlight.Update(gameTime);

            _spawnButton.Update(gameTime);
            _resetButton.Update(gameTime);

            // Ingredient Spawning
            if (_startSpawning == true && _numIngredients < LevelMap.NumIngredients)
            {
                // Update the spawn timer
                _spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // Spawn an ingredient every second
                if (_spawnTimer > 1)
                {
                    // Add an ingredient to the list (spawn it)
                    _ingredients.Add(new Ingredient(ContentLoader.TexTestingTerry,
                                     LevelMap.SpawnLocation));

                    // Increment number of ingredients and reset the spawn timer
                    _numIngredients++;
                    _spawnTimer = 0;
                }
            }

            // If the player presses a pause input, just go back to the level select for now
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                // For now, go to the level select state
                GameMain.Instance.CurrentState = GameMain.Instance.LevelSelect;
            }
        }
            
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw here

            LevelMap.DrawTiles(spriteBatch);

            // Draw ingredients
            for (int i = 0; i < _ingredients.Count; i++)
                _ingredients[i].Draw(spriteBatch);

            // Only draw the phantom/highlight while placement is allowed and the highlight is over top the map
            if (IsAllowedToEdit &&
                MapPosInPlaceableBounds((int)_tileHighlight.X / 16, (int)_tileHighlight.Y / 16))
            {
                if (PlacingShopItem)
                    _tilePhantom.Draw(spriteBatch);
                _tileHighlight.Draw(spriteBatch);
            }

            DrawShop(spriteBatch);

            _spawnButton.Draw(spriteBatch);
            _resetButton.Draw(spriteBatch);
        }

        /// <summary>
        /// Draws the shop and all shop items to screen.
        /// </summary>
        private void DrawShop(SpriteBatch spriteBatch)
        {
            // Update the buttons
            foreach (ShopButton button in _shopButtons)
                button.Draw(spriteBatch);

            // Draw the currency text
            Vector2 pos = new Vector2(GameMain.RenderTargetWidth - 8.0f, GameMain.RenderTargetHeight - 2.0f);
            UIText.DrawString(spriteBatch, ContentLoader.FntPixelBold, CurrentCurrency + "$",
                pos, Align.Right, Align.Bottom);
        }

        /// <summary>
        /// Selects the shop item at the given index.
        /// </summary>
        /// <param name="itemIndex">The shop item index.</param>
        public void SelectShopItem(int itemIndex)
        {
            SelectedShopIndex = itemIndex;
            PlacingShopItem = true;
            _tilePhantom.Texture = ShopItemsData.Items[SelectedShopIndex].Texture;
        }

        /// <summary>
        /// Deselects the currently-selected item from the shop.
        /// </summary>
        public void DeselectShopItem()
        {
            SelectedShopIndex = -1;
            PlacingShopItem = false;
        }

        /// <summary>
        /// Gets the position of the tile in the tile map grid which is currently being hovered over.
        /// </summary>
        /// <returns>The mouse position snapped to the tile grid.</returns>
        public Vector2 GetMouseGridPosition()
        {
            Vector2 mousePos = GameMain.Instance.GetMousePosition();
            return new Vector2((int)(mousePos.X / 16.0f), (int)(mousePos.Y / 16.0f));
        }

        /// <summary>
        /// Used to restrict placement of tiles so tiles aren't placed while trying to click buttons.
        /// </summary>
        /// <param name="x">The X map tile position.</param>
        /// <param name="y">The Y map tile position.</param>
        /// <returns>True when the position is valid.</returns>
        public bool MapPosInPlaceableBounds(int x, int y)
        {
            // Prevent placing tiles outside the map or where the spawn/reset buttons are
            return LevelMap.IsPositionInBounds(x, y) && !(x >= 7 && x <= 12 && y <= 1);
        }

        /// <summary>
        /// Returns true if any ingredient is at the given tile position.
        /// </summary>
        /// <param name="x">The X map tile position.</param>
        /// <param name="y">The Y map tile position.</param>
        /// <returns>True when an ingredient is at the position.</returns>
        public bool MapPosIsOccupied(int x, int y)
        {
            Rectangle placementRect = new Rectangle(x * 16, y * 16, 16, 16);
            foreach (Ingredient ingredient in _ingredients)
            {
                Rectangle rect = ingredient.CollisionRect;
                if (rect.Intersects(placementRect))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Called when the user left-clicked a tile.
        /// </summary>
        /// <param name="x">The X position of the tile clicked.</param>
        /// <param name="y">The Y position of the tile clicked.</param>
        private void LeftClickedTile(int x, int y)
        {
            // Do nothing if no shop item is selected
            if (!PlacingShopItem)
                return;

            ShopItem item = ShopItemsData.Items[SelectedShopIndex];
            // Cannot place without enough currency!
            if (CurrentCurrency < item.Cost)
                return;

            // Get the tile currently at that position
            Tile existingTile = LevelMap.GetTile(x, y);

            // Prevents being able to place things like jump pads in the air
            if (item.CanOnlyBePlacedOnWalkable && LevelMap.IsPositionInBounds(x, y + 1))
            {
                Tile below = LevelMap.GetTile(x, y + 1);
                if (below == null || !below.IsWalkable)
                    return;
            }

            // Place a new tile if that spot is empty
            if (existingTile == null)
            {
                LevelMap.PlaceTile(ShopItemsData.GetNewTileInstance(SelectedShopIndex), x, y);

                // Use currency
                CurrentCurrency -= item.Cost;
            }
        }

        /// <summary>
        /// Called when the user right-clicked a tile.
        /// </summary>
        /// <param name="x">The X position of the tile clicked.</param>
        /// <param name="y">The Y position of the tile clicked.</param>
        private void RightClickedTile(int x, int y)
        {
            // Get the tile currently at that position
            Tile existingTile = LevelMap.GetTile(x, y);

            // Delete the tile at that position if editable
            if (existingTile != null && existingTile.IsEditable)
            {
                // Get the shop data for that tile so the currency can be refunded
                ShopItem shopItem = ShopItemsData.GetShopItem(existingTile);
                DeleteTile(x, y, existingTile, shopItem);
            }
        }

        /// <summary>
        /// Deletes the tile at the given position and any tiles above it that require ground.
        /// </summary>
        /// <param name="x">The X position of the tile clicked.</param>
        /// <param name="y">The Y position of the tile clicked.</param>
        private void DeleteTile(int x, int y, Tile existingTile, ShopItem shopItem)
        {
            // Refund currency
            CurrentCurrency += ShopItemsData.GetShopItem(existingTile).Cost;
            // Delete the tile
            LevelMap.PlaceTile(null, x, y);

            /* Next, check if there's a tile above that needs deleted because
             * it can only be placed on floors (IE: jump pads). First, if this position
             * is already at the top of the map, don't try and search beyond it */
            if (y <= 0)
                return;

            // Get the tile above the one that was just deleted
            existingTile = LevelMap.GetTile(x, y - 1);
            if (existingTile == null)
                return;

            shopItem = ShopItemsData.GetShopItem(existingTile);
            // If the tile above the one that was just deleted requires a floor...
            if (shopItem.CanOnlyBePlacedOnWalkable)
                DeleteTile(x, y - 1, existingTile, shopItem); // Recursively delete unsupported tiles
        }

        /// <summary>
        /// Spawns the ingredients for the level
        /// </summary>
        private void SpawnIngredients(object sender, System.EventArgs e)
        {
            // Start spawning the ingredients
            _startSpawning = true;
        }

        /// <summary>
        /// Resets the ingredients for the level, they can then be spawned again
        /// </summary>
        private void ResetIngredients(object sender, System.EventArgs e)
        {
            // Debug reset
            Debug.WriteLine("***************** RESET *****************");

            // Don't keep spawning ingredients
            _startSpawning = false;

            // Reset ingredients to 0
            _numIngredients = 0;

            // Reset spawn timer to instantly spawn them
            _spawnTimer = 1;

            // Clear out all of the ingredients
            _ingredients.Clear();

            // Reset number of ingredients passed
            _ingredientsPassed = 0;

            // Invoke OnReset event
            if(OnResetGame != null) { OnResetGame(); }
                
        }

        /// <summary>
        /// Called when an ingredient hits the end tile,
        /// removes the ingredient and helps move player towards victory
        /// </summary>
        /// <param name="ingredient">The ingredient that got to the end</param>
        public void IngredientPassed(Ingredient ingredient)
        {
            // Increment # of passed ingredients
            _ingredientsPassed++;

            // Remove the ingredient from the level
            _ingredients.Remove(ingredient);

            // Change to victory state if all the ingredients have passed the level
            if (_ingredientsPassed == LevelMap.NumIngredients)
            {
                GameMain.Instance.CurrentState = GameMain.Instance.Victory;
            }
        }

        /// <summary>
        /// Removes an ingredient from ingredient list
        /// </summary>
        /// <param name="index">Index of the the ingredient array to be deleted</param>
        public void IngredientDeath(int index)
        {
            // Check for valid index
            if(index >= 0 && index < _ingredients.Count)
            {
                _ingredients[index].IsDying = true;

                // Make a little bounce!
                Vector2 temp = _ingredients[index].MoveSpeed;
                temp.Y -= 150;
                temp.X += 25;
                _ingredients[index].MoveSpeed = temp;
            }
        }

        /// <summary>
        /// Removes an ingredient from ingredient list
        /// </summary>
        /// <param name="ing">The ingredient to be deleted</param>
        public void IngredientDeath(Ingredient ing)
        {
            for(int i = 0; i < _ingredients.Count; i++)
            {
                if (_ingredients[i] == ing)
                {
                    // Call other version of method that takes
                    // an index
                    IngredientDeath(i);
                }
            }
        }
    }

    /// <summary>
    /// A game object that renders semi-transparent.
    /// </summary>
    internal class TilePhantom : GameObject
    {
        public TilePhantom() : base(null) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, DrawingRect, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }
    }       
}
