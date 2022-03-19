using UnityEngine;

using Cube.Level;

namespace Cube.LevelEditor
{
    [CreateAssetMenu(fileName = "New LevelEditorTileSet", menuName = "Cube/LevelEditor/LevelEditorTileSet")]
    public sealed class LevelEditorTileSet : ScriptableObject
    {
        // All Sprites should be within the same SpriteAtlas.
        public Sprite Void = null;
        public Sprite Floor = null;
        public Sprite Wall = null;

        public Texture GetTexture()
        {
            // All Sprites are within the same SpriteAtlas, so the texture is the same for all sprites.
            return Void.texture;
        }

        public Sprite GetTileSprite(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Void:
                    return Void;
                case TileType.Floor:
                    return Floor;
                case TileType.Wall:
                    return Wall;
                default:
                    return null;
            }
        }
    }
}
