// Don't Put me on the Spot, 3/22/2024
namespace ToppingTumble
{
    /// <summary>
    /// Contains grouped data for a level (but not the loaded tilemap!)
    /// </summary>
    internal struct LevelData
    {
        public string FilePath { get; private set; }
        public char[,] CharMap { get; private set; }
        public int NumIngredients { get; private set; }
        public int Currency { get; private set; }
        public bool[] EnabledItems { get; private set; }
        public int NumCoins { get; private set; }

        public LevelData(string filePath, char[,] charMap, int numIngredients, int currency, bool[] enabledItems, int numCoins)
        {
            FilePath = filePath;
            CharMap = charMap;
            NumIngredients = numIngredients;
            Currency = currency;
            EnabledItems = enabledItems;
            NumCoins = numCoins;
        }
    }
}
