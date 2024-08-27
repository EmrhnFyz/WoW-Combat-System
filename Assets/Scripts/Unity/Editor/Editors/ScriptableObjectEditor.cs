using Core;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Arena.Editor
{
    [CustomEditor(typeof(AuraInfo), true), CanEditMultipleObjects]
    internal class AuraInfoEditor : ScriptableObjectEditor { }

    [CustomEditor(typeof(AuraEffectInfo), true), CanEditMultipleObjects]
    internal class AuraEffectInfoEditor : ScriptableObjectEditor { }

    [CustomEditor(typeof(SpellEffectInfo), true), CanEditMultipleObjects]
    internal class SpellEffectInfoEditor : ScriptableObjectEditor { }

    [CustomEditor(typeof(AuraScriptable), true), CanEditMultipleObjects]
    internal class AuraScriptableEditor : ScriptableObjectEditor { }

    [CanEditMultipleObjects]
    internal class ScriptableObjectEditor : UnityEditor.Editor
    {
        private ScriptableObject spellInfo;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Set as child to parent asset: ", new GUIStyle() { fontStyle = FontStyle.Bold });

            EditorGUILayout.BeginHorizontal();
            spellInfo = (ScriptableObject)EditorGUILayout.ObjectField(spellInfo, typeof(SpellInfo), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add to Parent") && spellInfo != null)
            {
                if (AssetDatabase.IsSubAsset(target))
                {
                    AssetDatabase.RemoveObjectFromAsset(target);
                }
                else
                {
                    var assetPath = AssetDatabase.GetAssetPath(target);
                    AssetDatabase.RemoveObjectFromAsset(target);
                    AssetDatabase.DeleteAsset(assetPath);
                }

                AssetDatabase.AddObjectToAsset(target, spellInfo);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                spellInfo = null;
            }
            if (GUILayout.Button("Remove From Parent") && AssetDatabase.IsSubAsset(target))
            {
                var assetPath = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(target))}{'\\'}{target.name}.asset";
                AssetDatabase.RemoveObjectFromAsset(target);
                AssetDatabase.CreateAsset(target, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
    }
}
