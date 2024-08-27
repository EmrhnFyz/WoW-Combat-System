using UnityEditor;

namespace Editor
{

    public static class AssetUtils
    {
        [MenuItem("Assets/Reserialize All")]
        private static void ReserializeEntireProject() => AssetDatabase.ForceReserializeAssets();
    }
}
