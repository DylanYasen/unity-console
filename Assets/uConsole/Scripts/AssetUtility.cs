using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UConsole
{
    public static class AssetUtility
    {
        private const string AssetRootFolder = "Assets/Resources";

        public static T CreateAsset<T>(string folder) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            if (!AssetDatabase.IsValidFolder(AssetRootFolder))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            string folderPath = Path.Combine(AssetRootFolder, folder);
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(AssetRootFolder, folder);
            }

            string filename = typeof(T).Name.ToString() + ".asset";
            string fileguid = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folderPath, filename));

            AssetDatabase.CreateAsset(asset, fileguid);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }

        public static T GetAsset<T>(string folder) where T : ScriptableObject
        {
            string folderPath = Path.Combine(AssetRootFolder, folder);
            string filename = typeof(T).Name.ToString() + ".asset";
            string assetPath = Path.Combine(folderPath, filename);
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }
    }
}
