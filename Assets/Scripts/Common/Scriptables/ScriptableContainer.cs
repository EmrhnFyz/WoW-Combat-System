using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Maintains scriptable reference collection and updates them.
    /// </summary>
    [CreateAssetMenu(fileName = "Scriptable Container", menuName = "Game Data/Scriptable/Container", order = 1)]
    public class ScriptableContainer : ScriptableObject
    {
        [SerializeField] private List<ScriptableReference> scriptableReferences;

        public void Register()
        {
            for (var i = 0; i < scriptableReferences.Count; i++)
            {
                scriptableReferences[i].Register();
            }
        }

        public void Unregister()
        {
            for (var i = scriptableReferences.Count - 1; i >= 0; i--)
            {
                scriptableReferences[i].Unregister();
            }
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (ScriptableReference scriptableReference in scriptableReferences)
            {
                scriptableReference.DoUpdate(deltaTime);
            }
        }

        public void DoUpdate(int deltaTime)
        {
            foreach (ScriptableReference scriptableReference in scriptableReferences)
            {
                scriptableReference.DoUpdate(deltaTime);
            }
        }
    }
}
