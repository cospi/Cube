using System;
using System.IO;

using UnityEngine;

namespace Cube.Level
{
    public static class LevelDeleter
    {
        public static bool DeleteLevel(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            Debug.Log($"Deleted level {path}.");
            return true;
        }
    }
}
