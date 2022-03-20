using System;

using UnityEngine;

namespace Cube.Level
{
    public sealed class TileSetService
    {
        public readonly TileSet TileSet = null;
        public readonly Material GameplayMaterial = null;
        public readonly Material LevelEditorMaterial = null;

        public TileSetService(TileSet tileSet, Material gameplayMaterialAsset, Material levelEditorMaterialAsset)
        {
            if (gameplayMaterialAsset == null)
            {
                throw new ArgumentNullException(nameof(gameplayMaterialAsset));
            }

            if (levelEditorMaterialAsset == null)
            {
                throw new ArgumentNullException(nameof(levelEditorMaterialAsset));
            }

            if (tileSet == null)
            {
                throw new ArgumentNullException(nameof(tileSet));
            }

            TileSet = tileSet;
            Texture texture = tileSet.GetTexture();
            GameplayMaterial = CreateMaterial(gameplayMaterialAsset, texture);
            LevelEditorMaterial = CreateMaterial(levelEditorMaterialAsset, texture);
        }

        private static Material CreateMaterial(Material materialAsset, Texture texture)
        {
            Material material = new Material(materialAsset);
            material.mainTexture = texture;
            return material;
        }
    }
}
