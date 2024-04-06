// Don't Put me on the Spot, 3/21/2024
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ToppingTumble
{
    /// <summary>
    /// Contains grouped data for shop items.
    /// </summary>
    internal struct ShopItem
    {
        public Type TileType { get; private set; }
        public Texture2D Texture { get; private set; }
        public Texture2D HoverTexture { get; private set; }
        public int Cost { get; private set; }
        public bool CanOnlyBePlacedOnWalkable { get; private set; }

        public ShopItem()
        {
            TileType = null;
            Texture = null;
            HoverTexture = null;
            Cost = 0;
            CanOnlyBePlacedOnWalkable = false;
        }

        public ShopItem(Type tileType, Texture2D texture, Texture2D hoverTexture, int cost, bool canOnlyBePlacedOnWalkable = false)
        {
            TileType = tileType;
            Texture = texture;
            HoverTexture = hoverTexture;
            Cost = cost;
            CanOnlyBePlacedOnWalkable = canOnlyBePlacedOnWalkable;
        }
    }
}
