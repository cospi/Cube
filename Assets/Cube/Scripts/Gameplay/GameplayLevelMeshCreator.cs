using System;
using System.Collections.Generic;

using UnityEngine;

using Cube.Level;
using Cube.Utils;

namespace Cube.Gameplay
{
    public static class GameplayLevelMeshCreator
    {
        private static class TileSideFlags
        {
            public const int Floor = 1;
            public const int WallFront = 2;
            public const int WallBack = 4;
            public const int WallLeft = 8;
            public const int WallRight = 16;
            public const int WallTop = 32;
        }

        private const int TileSideCount = 6;

        private static readonly Vector3[] TileSideVertices = new Vector3[]
        {
            // Floor
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(1f, 0f, 1f),
            new Vector3(1f, 0f, 0f),
            // Wall front
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 0f, 0f),
            // Wall back
            new Vector3(1f, 0f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 0f, 1f),
            // Wall left
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 0f, 0f),
            // Wall right
            new Vector3(1f, 0f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 0f, 1f),
            // Wall top
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 1f),
            new Vector3(1f, 1f, 1f),
            new Vector3(1f, 1f, 0f)
        };

        private static readonly Vector3[] TileSideNormals = new Vector3[]
        {
            // Floor
            Vector3.up,
            // Wall front
            Vector3.back,
            // Wall back
            Vector3.forward,
            // Wall left
            Vector3.left,
            // Wall right
            Vector3.right,
            // Wall top
            Vector3.up
        };

        private const int VerticesPerTileSide = 4;

        public static Mesh CreateLevelMesh(LevelData level, TileSet tileSet)
        {
            if (level == null)
            {
                throw new ArgumentNullException(nameof(level));
            }

            if (tileSet == null)
            {
                throw new ArgumentNullException(nameof(tileSet));
            }

            Tile[,] tiles = level.Tiles;
            Vector2Int size = level.GetSize();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            for (int y = 0; y < size.y; ++y)
            {
                float yF = (float)y;
                for (int x = 0; x < size.x; ++x)
                {
                    Tile tile = tiles[y, x];
                    if (tile == Tile.Void)
                    {
                        continue;
                    }

                    float xF = (float)x;
                    PushTile(
                        xF,
                        yF,
                        (tile == Tile.Wall) ? GetWallTileSideFlags(x, y, tiles, size) : TileSideFlags.Floor,
                        SpriteUtils.GetSpriteUVRect(tileSet.GetTileSprite(tile)),
                        vertices,
                        uv,
                        normals,
                        indices
                    );
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.normals = normals.ToArray();
            mesh.triangles = indices.ToArray();
            return mesh;
        }

        private static int GetWallTileSideFlags(int x, int y, Tile[,] tiles, Vector2Int size)
        {
            int flags = TileSideFlags.WallTop;
            // Prevent unnecessary sides between adjacent wall tiles.
            if (!IsWallTile(x, y - 1))
            {
                flags |= TileSideFlags.WallFront;
            }
            if (!IsWallTile(x, y + 1))
            {
                flags |= TileSideFlags.WallBack;
            }
            if (!IsWallTile(x - 1, y))
            {
                flags |= TileSideFlags.WallLeft;
            }
            if (!IsWallTile(x + 1, y))
            {
                flags |= TileSideFlags.WallRight;
            }
            return flags;

            bool IsWallTile(int xx, int yy)
            {
                return (xx >= 0) && (xx < size.x) && (yy >= 0) && (yy < size.y) && (tiles[yy, xx] == Tile.Wall);
            }
        }

        private static void PushTile(
            float x,
            float y,
            int sideFlags,
            Rect uvRect,
            List<Vector3> vertices,
            List<Vector2> uv,
            List<Vector3> normals,
            List<int> indices
        )
        {
            for (int i = 0; i < TileSideCount; ++i)
            {
                if ((sideFlags & GetTileSideFlagBit(i)) != 0)
                {
                    PushTileSide(x, y, i, uvRect, vertices, uv, normals, indices);
                }
            }
        }

        private static int GetTileSideFlagBit(int side)
        {
            return 1 << side;
        }

        private static void PushTileSide(
            float x,
            float y,
            int side,
            Rect uvRect,
            List<Vector3> vertices,
            List<Vector2> uv,
            List<Vector3> normals,
            List<int> indices
        )
        {
            int vertexOffset = vertices.Count;
            PushTileSideVertices(x, y, side, vertices);
            PushTileSideUV(uvRect, uv);
            PushTileSideNormals(side, normals);
            PushTileSideIndices(vertexOffset, indices);
        }

        private static void PushTileSideVertices(float x, float y, int side, List<Vector3> vertices)
        {
            int offset = side * VerticesPerTileSide;
            for (int i = 0; i < VerticesPerTileSide; ++i)
            {
                Vector3 vertex = TileSideVertices[offset + i];
                vertex.x += x;
                vertex.z += y;
                vertices.Add(vertex);
            }
        }

        private static void PushTileSideUV(Rect uvRect, List<Vector2> uv)
        {
            Vector2 min = uvRect.min;
            Vector2 max = uvRect.max;
            uv.Add(min);
            uv.Add(new Vector2(min.x, max.y));
            uv.Add(max);
            uv.Add(new Vector2(max.x, min.y));
        }

        private static void PushTileSideNormals(int side, List<Vector3> normals)
        {
            Vector3 normal = TileSideNormals[side];
            for (int i = 0; i < VerticesPerTileSide; ++i)
            {
                normals.Add(normal);
            }
        }

        private static void PushTileSideIndices(int vertexOffset, List<int> indices)
        {
            indices.Add(vertexOffset);
            indices.Add(vertexOffset + 1);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 3);
        }
    }
}
