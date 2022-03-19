namespace Cube.Level
{
    public struct TileData
    {
        public TileType Type;

        public TileData(TileType type)
        {
            Type = type;
        }

        public bool Equals(TileData tile)
        {
            return Type == tile.Type;
        }
    }
}
