using UnityEngine;

namespace Cube.Level
{
    [CreateAssetMenu(fileName = "New TileSet", menuName = "Cube/Level/TileSet")]
    public sealed class TileSet : ScriptableObject
    {
        // All Sprites should be within the same SpriteAtlas.
        public Sprite Void = null;
        public Sprite Floor = null;
        public Sprite Wall = null;
        public Sprite Start = null;
        public Sprite Goal = null;

        public Texture GetTexture()
        {
            // All Sprites are within the same SpriteAtlas, so the texture is the same for all sprites.
            return Void.texture;
        }

        public Sprite GetTileSprite(Tile tile)
        {
            switch (tile)
            {
                case Tile.Void:
                    return Void;
                case Tile.Floor:
                    return Floor;
                case Tile.Wall:
                    return Wall;
                case Tile.Start:
                    return Start;
                case Tile.Goal:
                    return Goal;
                default:
                    return null;
            }
        }
    }
}
