using System;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Nameplate Settings", menuName = "Game Data/Interface/Nameplate Settings", order = 2)]
    public class NameplateSettings : ScriptableObject
    {
        [Serializable]
        public class HostilitySettings
        {
            [SerializeField] private bool showName;
            [SerializeField] private bool showCast;
            [SerializeField] private bool showHealth;
            [SerializeField] private Color healthColor;
            [SerializeField] private Color nameWithoutPlateColor;
            [SerializeField] private Color nameWithPlateColor;
            [SerializeField] private float selectedGeneralAlpha;
            [SerializeField] private float deselectedGeneralAlpha;
            [SerializeField] private bool applyScaling;

            public bool ShowName => showName;
            public bool ShowCast => showCast;
            public bool ShowHealth => showHealth;
            public Color HealthColor => healthColor;
            public Color NameWithoutPlateColor => nameWithoutPlateColor;
            public Color NameWithPlateColor => nameWithPlateColor;
            public float SelectedGeneralAlpha => selectedGeneralAlpha;
            public float DeselectedGeneralAlpha => deselectedGeneralAlpha;
            public bool ApplyScaling => applyScaling;
        }

        [SerializeField] private HostilitySettings self;
        [SerializeField] private HostilitySettings friendly;
        [SerializeField] private HostilitySettings neutral;
        [SerializeField] private HostilitySettings enemy;
        [SerializeField] private AnimationCurve scaleOverDistance;
        [SerializeField] private float maxDistance;
        [SerializeField] private float detailedDistance;
        [SerializeField] private float distanceThreshold;
        [SerializeField] private float healthAlphaTrasitionSpeed;
        [SerializeField, HideInInspector] private float maxDistanceSqr;

        public HostilitySettings Self => self;
        public HostilitySettings Friendly => friendly;
        public HostilitySettings Neutral => neutral;
        public HostilitySettings Enemy => enemy;
        public AnimationCurve ScaleOverDistance => scaleOverDistance;
        public float MaxDistance => maxDistance;
        public float DetailedDistance => detailedDistance;
        public float DistanceThreshold => distanceThreshold;
        public float MaxDistanceSqr => maxDistanceSqr;
        public float HealthAlphaTrasitionSpeed => healthAlphaTrasitionSpeed;


        private void OnValidate()
        {
            maxDistanceSqr = maxDistance;
        }
    }
}
