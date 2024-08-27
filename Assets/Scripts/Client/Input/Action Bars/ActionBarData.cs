using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class ActionBarData
    {
        [SerializeField] private List<ActionButtonData> buttons = new();

        public List<ActionButtonData> Buttons => buttons;
    }
}
