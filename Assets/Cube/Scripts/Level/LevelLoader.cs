using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace Cube.Level
{
    public static class LevelLoader
    {
        public static LevelData LoadLevel(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            LevelData level = null;
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open))
                {
                    level = LoadLevel(stream);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            if (level != null)
            {
                Debug.Log($"Loaded level from {path}.");
            }
            return level;
        }

        public static LevelData LoadLevel(TextAsset textAsset)
        {
            if (textAsset == null)
            {
                throw new ArgumentNullException(nameof(textAsset));
            }

            LevelData level = null;
            try
            {
                using (MemoryStream stream = new MemoryStream(textAsset.bytes))
                {
                    level = LoadLevel(stream);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return level;
        }

        private static LevelData LoadLevel(Stream stream)
        {
            LevelData level = null;
            try
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    Vector2Int size = new Vector2Int(reader.ReadInt32(), reader.ReadInt32());
                    if ((size.x <= 0) || (size.y <= 0))
                    {
                        Debug.LogError("Invalid level size.");
                        return null;
                    }

                    Tile[,] tiles = new Tile[size.y, size.x];
                    for (int y = 0; y < size.y; ++y)
                    {
                        for (int x = 0; x < size.x; ++x)
                        {
                            tiles[y, x] = (Tile)reader.ReadInt32();
                        }
                    }
                    level = new LevelData(tiles);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            return level;
        }
    }
}
