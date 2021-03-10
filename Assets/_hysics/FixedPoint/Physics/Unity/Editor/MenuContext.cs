using System.IO;
using UnityEditor;
using UnityEngine;

namespace Spax {

    /**
    * @brief Represents the FixedPoint menu context bar.
    **/
    public class MenuContext {

        private static string ASSETS_PREFABS_PATH = "Assets/FixedPoint/Unity/Prefabs/{0}.prefab";

        private static void InstantiatePrefab(string path) {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format(ASSETS_PREFABS_PATH, path));
            PrefabUtility.InstantiatePrefab(prefab);
        }

        private static void CreateFixedPointConfigAsset() {
            FixedPointConfig asset = ScriptableObject.CreateInstance<FixedPointConfig>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            } else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/FixedPointConfig.asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        [MenuItem("Assets/Create/FixedPointConfig", false, 0)]
        static void CreateFixedPointConfig() {
            CreateFixedPointConfigAsset();
        }

        [MenuItem("GameObject/FixedPoint/Cube", false, 0)]
        static void CreatePrefabCube() {
            InstantiatePrefab("Basic/Cube");
        }

        [MenuItem("GameObject/FixedPoint/Sphere", false, 0)]
        static void CreatePrefabSphere() {
            InstantiatePrefab("Basic/Sphere");
        }

        [MenuItem("GameObject/FixedPoint/Capsule", false, 0)]
        static void CreatePrefabCapsule() {
            InstantiatePrefab("Basic/Capsule");
        }

        [MenuItem("GameObject/FixedPoint/FixedPointManager", false, 11)]
        static void CreatePrefabFixedPoint() {            
            InstantiatePrefab("FixedPointManager");
        }

    }

}