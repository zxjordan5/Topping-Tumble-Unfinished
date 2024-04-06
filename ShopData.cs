// Don't Put me on the Spot, 3/21/2024
using Microsoft.Xna.Framework.Graphics;
using ToppingTumble.TileClasses;

namespace ToppingTumble
{
    /// <summary>
    /// Used to store and access shop items/data.
    /// </summary>
    internal class ShopData
    {
        /// <summary>
        /// Stores the shop data (IE: the items and costs).
        /// </summary>
        public ShopItem[] Items { get; private set; }

        /// <summary>
        /// Creates a new ShopData object with only certain items enabled
        /// </summary>
        /// <param name="enabledItems">An array determining which items are to be enabled</param>
        public ShopData(bool[] enabledItems)
        {
            // Get the number of items for the shop
            int numItems = 0;

            // Go through enable array, increment the number of items for each item
            // that is enabled
            for (int i = 0; i < enabledItems.Length; i++)
            {
                if (enabledItems[i])
                {
                    numItems++;
                }
            }

            // Initialize items array
            Items = new ShopItem[numItems];

            // Add the intended items to the shop
            int shopIndex = 0;

            for (int i = 0; i < enabledItems.Length; i++)
            {
                // i will determine which item is being enabled/disabled
                switch(i)
                {
                    // Check for Blocks
                    case 0:
                        if (enabledItems[i])
                        {
                            // Add a block and increment shop index
                            Items[shopIndex] = new ShopItem(typeof(PlaceableWall), ContentLoader.TexPlaceTile, ContentLoader.TexPlaceTile, 1);
                            shopIndex++;
                        }
                        break;

                    // Check for Springs
                    case 1:
                        if (enabledItems[i])
                        {
                            // Add a spring and increment shop index
                            Items[shopIndex] = new ShopItem(typeof(Spring), ContentLoader.TexSpringStill, ContentLoader.TexSpringStill, 2, true);
                            shopIndex++;
                        }
                        break;

                    // Check for Speed Boost
                    case 2:
                        if (enabledItems[i])
                        {
                            // Add the speed boost and increment shop index
                            Items[shopIndex] = new ShopItem(typeof(SpeedTile), ContentLoader.TexTestingTerry, ContentLoader.TexTestingTerry, 2, false);
                            shopIndex++;
                        }
                        break;

                }
            }

            /*
            // Initialize shop data
            Items = new ShopItem[]
            {
                new ShopItem(typeof(PlaceableWall), ContentLoader.TexPlaceTile, ContentLoader.TexPlaceTile, 1),
                new ShopItem(typeof(JumpPad), ContentLoader.TexJumpPad, ContentLoader.TexJumpPad, 2, true)
            };
            */
        }

        /// <summary>
        /// Returns a new instance of a tile for a given shop item.
        /// </summary>
        /// <param name="itemIndex">The shop item index.</param>
        /// <returns>The new tile instance.</returns>
        public Tile GetNewTileInstance(int itemIndex)
        {
            /*
            switch (itemIndex)
            {
                default:
                    throw new System.IndexOutOfRangeException("Invalid shop item index!");
                case 0:
                    return new PlaceableWall();
                case 1:
                    return new JumpPad();
            }
            */

            // Determines which item to place by comparing textures
            if (Items[itemIndex].Texture.Equals(ContentLoader.TexPlaceTile))
            {
                return new PlaceableWall();
            }
            else if (Items[itemIndex].Texture.Equals(ContentLoader.TexSpringStill))
            {
                return new Spring();
            }
            else if (Items[itemIndex].Texture.Equals(ContentLoader.TexTestingTerry))
            {
                return new SpeedTile();
            }
            else
            {
                throw new System.IndexOutOfRangeException("Invalid shop item index!");
            }
        }

        /// <summary>
        /// Gets the shop item data for the type of the given tile.
        /// </summary>
        /// <param name="tile">The tile whose type to search for.</param>
        /// <returns>The shop data. Returns an empty default if none found.</returns>
        public ShopItem GetShopItem(Tile tile)
        {
            foreach (ShopItem item in Items)
            {
                if (item.TileType == tile.GetType())
                    return item;
            }
            return new ShopItem();
        }
    }
}
