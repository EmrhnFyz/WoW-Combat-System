using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public abstract class ScriptableUniqueInfo<TUnique> : ScriptableObject, IScriptablePostProcess where TUnique : ScriptableUniqueInfo<TUnique>
    {
        [SerializeField, HideInInspector] private int id;

        protected int Id => id;
        protected abstract TUnique Data { get; }
        protected abstract ScriptableUniqueInfoContainer<TUnique> Container { get; }

        internal void Register() => OnRegister();

        internal void Unregister() => OnUnregister();

        protected virtual void OnRegister()
        {
        }

        protected virtual void OnUnregister()
        {
        }

        bool IScriptablePostProcess.OnPostProcess(bool isDeleted)
        {
            var hasChanges = false;
#if UNITY_EDITOR
            var takenIds = new HashSet<int>();

            if (Container != null)
            {
                if (isDeleted)
                {
                    Container.EditorList.Remove(Data);
                    UnityEditor.EditorUtility.SetDirty(Container);
                    return true;
                }

                if (Container is IScriptablePostProcess postProcessContainer)
                {
                    hasChanges |= postProcessContainer.OnPostProcess(false);
                }

                if (Container.EditorList.Contains(Data))
                {
                    return false;
                }

                foreach (TUnique item in Container.ItemList)
                {
                    takenIds.Add(item.Id);
                }

                if (id != 0)
                {
                    if (!takenIds.Contains(Id))
                    {
                        Container.EditorList.Add(Data);
                        UnityEditor.EditorUtility.SetDirty(Container);
                    }
                    else
                    {
                        UnityEditor.EditorUtility.SetDirty(this);
                        id = 0;
                    }

                    hasChanges = true;
                }
            }

            if (id <= 0)
            {
                foreach (var guid in UnityEditor.AssetDatabase.FindAssets($"t:{GetType()}", null))
                {
                    var infoAssetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    ScriptableUniqueInfo<TUnique> anotherInfo = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableUniqueInfo<TUnique>>(infoAssetPath);
                    takenIds.Add(anotherInfo.Id);
                }

                for (var i = 1; i < int.MaxValue; i++)
                {
                    if (!takenIds.Contains(i))
                    {
                        id = i;
                        Debug.Log($"Assigned id:{i} to {GetType().Name}: {name}");

                        if (Container != null && !Container.EditorList.Contains(Data))
                        {
                            Debug.Log($"Added new item: {GetType().Name}: {name} id:{id} to container {Container.GetType()}");
                            Container.EditorList.Add(Data);
                            UnityEditor.EditorUtility.SetDirty(Container);
                        }

                        UnityEditor.EditorUtility.SetDirty(this);
                        hasChanges = true;
                        break;
                    }
                }
            }

            if (hasChanges && Container != null)
            {
                Container.EditorList.Sort((x, y) => x.Id.CompareTo(y.Id));
            }
#endif
            return hasChanges;
        }
    }
}
