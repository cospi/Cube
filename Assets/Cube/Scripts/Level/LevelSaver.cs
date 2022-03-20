using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace Cube.Level
{
    public static class LevelSaver
    {
        public static bool SaveLevel(LevelData level, string path)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            try
            {
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (FileStream stream = File.Open(path, FileMode.Create))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        Vector2Int size = level.GetSize();
                        writer.Write(size.x);
                        writer.Write(size.y);

                        Tile[,] tiles = level.Tiles;
                        for (int y = 0; y < size.y; ++y)
                        {
                            for (int x = 0; x < size.x; ++x)
                            {
                                writer.Write((int)tiles[y, x]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            Debug.Log($"Saved level to {path}.");
            return true;
        }
    }
}
