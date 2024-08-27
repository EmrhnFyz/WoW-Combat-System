using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Class Info Container", menuName = "Game Data/Containers/Class Info", order = 1)]
    public class ClassInfoContainer : ScriptableUniqueInfoContainer<ClassInfo>
    {
        [SerializeField] private List<ClassInfo> classes;

        protected override List<ClassInfo> Items => classes;
    }
}
