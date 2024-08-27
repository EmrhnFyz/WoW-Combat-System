using Common;
using UnityEditor;
using UnityEngine;

namespace Arena.Editor
{

    internal class ScriptableAssetPostProcessor : AssetPostprocessor
    {
        internal static bool HasDeletedPostprocessableAssets { get; set; }


        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var hasChanged = false;
            foreach (var path in importedAssets)
            {
                if (path.StartsWith("Assets/Scenes/"))
                {
                    continue;
                }

                foreach (Object asset in AssetDatabase.LoadAllAssetsAtPath(path))
                {
                    if (asset is IScriptablePostProcess postProcessable)
                    {
                        hasChanged |= postProcessable.OnPostProcess(false);
                    }
                }
            }

            if (HasDeletedPostprocessableAssets)
            {
                HasDeletedPostprocessableAssets = false;
                hasChanged = true;
            }

            if (hasChanged)
            {
                AssetDatabase.SaveAssets();
            }
        }
    }


    internal class ScriptableAssetModificationProcessor : AssetModificationProcessor
    {

        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions removeOptions)
        {
            if (path.StartsWith("Assets/Scenes/"))
            {
                return AssetDeleteResult.DidNotDelete;
            }

            PostProcessDeletedAsset(AssetDatabase.LoadAssetAtPath<Object>(path));

            foreach (Object subAsset in AssetDatabase.LoadAllAssetRepresentationsAtPath(path))
            {
                PostProcessDeletedAsset(subAsset);
            }

            return AssetDeleteResult.DidNotDelete;

            void PostProcessDeletedAsset(Object asset)
            {
                if (asset is IScriptablePostProcess postProcessable)
                {
                    ScriptableAssetPostProcessor.HasDeletedPostprocessableAssets = true;
                    postProcessable.OnPostProcess(true);
                }
            }
        }
    }
}
