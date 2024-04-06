// Don't Put me on the Spot, 3/22/2024
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using ToppingTumble.TileClasses;

namespace ToppingTumble
{
    /// <summary>
    /// Represents the map for a level.
    /// </summary>
    internal class TileMap
    {
        // The actual map of tiles
        private Tile[,] _tileMap;

        /// <summary>
        /// The tile at which ingredients spawn.
        /// </summary>
        public Tile EntranceTile { get; private set; }
        /// <summary>
        /// Gets the coordinates of the entrance tile.
        /// </summary>
        public Vector2 SpawnLocation { get { return EntranceTile.TileRect.Location.ToVector2(); } }

        /// <summary>
        /// A list of all yellow switch door tiles currently loaded.
        /// </summary>
        public List<SwitchDoorTile> YellowDoors { get; private set; }
        /// <summary>
        /// A list of all green switch door tiles currently loaded.
        /// </summary>
        public List<SwitchDoorTile> GreenDoors { get; private set; }
        /// <summary>
        /// A list of all purple switch door tiles currently loaded.
        /// </summary>
        public List<SwitchDoorTile> PurpleDoors { get; private set; }

        /// <summary>
        /// Gets the number of ingredients for the level.
        /// </summary>
        public int NumIngredients { get; private set; }

        /// <summary>
        /// The default currency amount in this map.
        /// </summary>
        public int Currency { get; private set; }

        /// <summary>
        /// Gets an array determining which items are enabled.
        /// </summary>
        public bool[] EnabledItems { get; private set; }

        /// <summary>
        /// Gets the number of coins for the level.
        /// </summary>
        public int NumCoins { get; private set; }

        /// <summary>
        /// Creates a new tilemap based on level data.
        /// </summary>
        /// <param name="levelData">The level data to load from.</param>
        public TileMap(LevelData levelData)
        {
            LoadMap(levelData);

            // Update texture of static walls loaded from file
            // Loop through x values of the tilemap
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                // Loop through y values of the tilemap
                for (int y = 0; y < _tileMap.GetLength(1); y++)
                {
                    if (_tileMap[x, y] == null)
                        continue;

                    UpdateWallTexture(_tileMap[x, y], x, y);
                }
            }
        }

        // Methods
        /// <summary>
        /// Used to place a newly created (or null) tile.
        /// </summary>
        /// <param name="tile">The tile to place.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public void PlaceTile(Tile tile, int x, int y)
        {
            if (!IsPositionInBounds(x, y))
                throw new IndexOutOfRangeException($"Position ({x}, {y}) is outside the bounds of the map!");

            if (tile != null)
                tile.InitiailizeMapPos(x, y);
            _tileMap[x, y] = tile;

            // Update the textures of placeable walls impacted by the placement or removal of a wall
            if (tile is PlaceableWall || tile is null)
            {
                UpdateWallTexture(tile, x, y);
                UpdateNeighborWallTextures(x, y);
            }
        }

        /// <summary>
        /// Returns the tile at the given position in the map.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <returns>The tile at the given position.</returns>
        public Tile GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }

        /// <summary>
        /// Draws the map to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch object to draw with</param>
        public void DrawTiles(SpriteBatch sb)
        {
            // Loop through x values of the tilemap
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                // Loop through y values of the tilemap
                for (int y = 0; y < _tileMap.GetLength(1); y++)
                {
                    if (_tileMap[x, y] == null)
                        continue;

                    int width = 16;
                    int height = 16;

                    // Actually draw the tile
                    _tileMap[x, y].Draw(sb, x * width, y * height);
                }
            }
        }

        /// <summary>
        /// Updates the texture of a given tile according to its neighboring tiles.
        /// </summary>
        /// <param name="tile">The tile being updated</param>
        /// <param name="x">The x position on the tilemap</param>
        /// <param name="y">The y position on the tilemap</param>
        public void UpdateWallTexture(Tile tile, int x, int y)
        {
            int iteration = -1;
            int textureIndex = 0;

            // Loop through all neighboring tiles to check for walls
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    if (i == 0 && j == 0) continue; // Skip over self

                    iteration++;

                    int checkX = x + i; // Actual x position of checked wall on tilemap
                    int checkY = y + j; // Actual y position of checked wall on tilemap

                    // Check adjacent tiles for wall or out of bounds position (both affect this wall's texture)
                    if (!IsPositionInBounds(checkX, checkY) || _tileMap[checkX, checkY] is ITileable)
                    {
                        // Check corners for significance to the texture
                        // NOTE: adjacent walls are always significant
                        if (!IsPositionInBounds(checkX, y) || _tileMap[checkX, y] is ITileable)
                            if (!IsPositionInBounds(x, checkY) || _tileMap[x, checkY] is ITileable)
                            {
                                // This tile being checked is a significant wall,
                                // Add value of position checked to the total in order to choose the index of the tile's texture:
                                /*  1       2       4
                                 *  8       -       16
                                 *  32      64      128 */
                                textureIndex += (int)Math.Pow(2, iteration);
                            }
                    }
                }
            }

            // Array of unique integers corresponding to each texture on the tilesheet
            int[] textureIndices = { 0, 2, 8, 10, 11, 16, 18, 22, 24, 26, 27, 30, 31, 64, 66, 72, 74, 75, 80, 82, 86, 88, 90, 91, 94, 95, 104, 106, 107, 120, 122, 123, 126, 127, 208, 210, 214, 216, 218, 219, 222, 223, 248, 250, 251, 254, 255 };

            // Update the texture of this tile position if it is a placed wall (could be null)
            if (tile is ITileable)
            {
                ITileable wall = tile as ITileable;
                wall.TextureIndex = Array.IndexOf(textureIndices, textureIndex);
            }
        }

        /// <summary>
        /// Updates the textures of placed walls surrounding the position of a given x and y on the tilemap.
        /// </summary>
        /// <param name="x">The x position on the tilemap</param>
        /// <param name="y">The y position on the tilemap</param>
        public void UpdateNeighborWallTextures(int x, int y)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int i = -1; i <= 1; i++)
                {
                    int checkX = x + i; // Actual x position of checked wall on tilemap
                    int checkY = y + j; // Actual y position of checked wall on tilemap

                    if (!IsPositionInBounds(checkX, checkY)) continue;  // Ignore out-of-bounds positions
                    if (i == 0 && j == 0) continue;                     // Ignore self
                    if (_tileMap[checkX, checkY] == null) continue;     // Ignore empty tiles

                    // Update the texture of this neighboring wall
                    UpdateWallTexture(GetTile(checkX, checkY), checkX, checkY);
                }
            }
        }

        /// <summary>
        /// Updates all of the tiles
        /// </summary>
        /// <param name="gameTime">object to tell how much time has passed</param>
        public void UpdateTiles(GameTime gameTime)
        {
            // Loop through x values of the tilemap
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                // Loop through y values of the tilemap
                for (int y = 0; y < _tileMap.GetLength(1); y++)
                {
                    // Call the tile's update method
                    if (_tileMap[x, y] != null)
                        _tileMap[x, y].Update(gameTime);
                }
            }
        }

        public bool IsPositionInBounds(int x, int y)
        {
            return x >= 0 && x < _tileMap.GetLength(0) && y >= 0 && y < _tileMap.GetLength(1);
        }

        /// <summary>
        /// Builds the tile map using the provided level data.
        /// </summary>
        /// <param name="levelData">The level to load from.</param>
        /// <returns>The constructed tile map.</returns>
        private void LoadMap(LevelData levelData)
        {
            // Create a 2D array of tiles with proper dimensions
            _tileMap = new Tile[levelData.CharMap.GetLength(0), levelData.CharMap.GetLength(1)];

            // Prepare lists for caching tiles and tile positions
            YellowDoors = new List<SwitchDoorTile>();
            GreenDoors = new List<SwitchDoorTile>();
            PurpleDoors = new List<SwitchDoorTile>();

            // Create tiles based on the chars
            for (int y = 0; y < _tileMap.GetLength(1); y++)
            {
                for (int x = 0; x < _tileMap.GetLength(0); x++)
                {
                    // Add a different tile to the array depending on the character
                    Tile tile = null;
                    switch (levelData.CharMap[x, y]) {
                        case '.': // Empty tile
                            tile = null;
                            break;

                        case '#': // Wall
                            tile = new StaticWall();
                            break;

                        case 'S': // Start
                            tile = new StartTile();
                            EntranceTile = tile;
                            break;

                        case 'E': // End
                            tile = new EndTile();
                            break;

                        case 'C': // Coin
                            tile = new Coin();
                            break;

                        case '^': // Spike (UP)
                            tile = new SpikeTile(SpikeTileDirection.Up);
                            break;

                        case 'v': // Spike (DOWN)
                            tile = new SpikeTile(SpikeTileDirection.Down);
                            break;

                        case '<': // Spike (LEFT)
                            tile = new SpikeTile(SpikeTileDirection.Left);
                            break;

                        case '>': // Spike (RIGHT)
                            tile = new SpikeTile(SpikeTileDirection.Right);
                            break;

                        case 'Y': // Switch (YELLOW)
                            tile = new Switch(SwitchColor.Yellow);
                            break;

                        case 'G': // Switch (GREEN)
                            tile = new Switch(SwitchColor.Green);
                            break;

                        case 'P': // Switch (PURPLE)
                            tile = new Switch(SwitchColor.Purple);
                            break;

                        case 'y': // Door (YELLOW, ENABLED BY DEFAULT)
                            tile = new SwitchDoorTile(SwitchColor.Yellow, true);
                            YellowDoors.Add((SwitchDoorTile)tile);
                            break;

                        case 'g': // Door (GREEN, ENABLED BY DEFAULT)
                            tile = new SwitchDoorTile(SwitchColor.Green, true);
                            GreenDoors.Add((SwitchDoorTile)tile);
                            break;

                        case 'p': // Door (PURPLE, ENABLED BY DEFAULT)
                            tile = new SwitchDoorTile(SwitchColor.Purple, true);
                            PurpleDoors.Add((SwitchDoorTile)tile);
                            break;

                        case 'z': // Door (YELLOW, DISABLED BY DEFAULT)
                            tile = new SwitchDoorTile(SwitchColor.Yellow, false);
                            YellowDoors.Add((SwitchDoorTile)tile);
                            break;

                        case 'h': // Door (GREEN, DISABLED BY DEFAULT)
                            tile = new SwitchDoorTile(SwitchColor.Green, false);
                            GreenDoors.Add((SwitchDoorTile)tile);
                            break;

                        case 'q': // Door (PURPLE, DISABLED BY DEFAULT)
                            tile = new SwitchDoorTile(SwitchColor.Purple, false);
                            PurpleDoors.Add((SwitchDoorTile)tile);
                            break;

                        default:
                            tile = null;
                            break;
                    }

                    if (tile != null)
                        tile.InitiailizeMapPos(x, y);
                    _tileMap[x, y] = tile;
                }
            }

            NumIngredients = levelData.NumIngredients;
            Currency = levelData.Currency;
            EnabledItems = levelData.EnabledItems;
            NumCoins = levelData.NumCoins;
        }

        /// <summary>
        /// Loads all the data associated with a level and returns it in a struct.
        /// </summary>
        /// <param name="filePath">The path where the file is located.</param>
        /// <returns>All loaded level data.</returns>
        public static LevelData LoadLevelData(string filePath)
        {
            // Get a new stream to read from
            StreamReader input = new StreamReader(filePath);

            // Read in the character map used to specify what tiles get placed
            char[,] charMap = new char[20, 10];
            for (int y = 0; y < charMap.GetLength(1); y++)
            {
                // Make a string to read each line on
                string line = input.ReadLine();
                for (int x = 0; x < charMap.GetLength(0); x++)
                    charMap[x, y] = line[x];
            }

            // Read # ingredients
            int numIngredients = int.Parse(input.ReadLine());

            // Read currency
            int currency = int.Parse(input.ReadLine());

            // Read the enabled/disabled items
            bool[] enabledItems = GetEnabledItems(input.ReadLine());

            // Read # coins
            int numCoins = int.Parse(input.ReadLine());

            // Close the file
            input.Close();

            return new LevelData(filePath, charMap, numIngredients, currency, enabledItems, numCoins);
        }

        /// <summary>
        /// Helper method to get the items that are enabled from the
        /// level file by getting a string
        /// </summary>
        /// <param name="str">The string that contains info about items being enabled/disabled</param>
        /// <returns>A boolean array for which items are enabled/disabled
        ///          Index 0: true - blocks ENABLED, false - blocks DISABLED
        ///          Index 1: true - springs ENABLED, false - springs DISABLED
        ///          Index 2: true - speed boost ENABLED, false - speed boost DISABLED</returns>
        private static bool[] GetEnabledItems(string str)
        {
            // Make new boolean array to return
            bool[] toReturn = new bool[3];

            // Separate the values for enabling/disabling
            string[] enabledVals = str.Split(',');

            // Go through every possible item
            for (int i = 0; i < enabledVals.Length; i++)
            {
                switch(enabledVals[i])
                {
                    // Enable the item if it's a 1
                    case "1":
                        toReturn[i] = true;
                        break;

                    // Disable the item if it's a 0
                    case "0":
                        toReturn[i] = false;
                        break;
                }
            }

            return toReturn;
        }
    }
}
