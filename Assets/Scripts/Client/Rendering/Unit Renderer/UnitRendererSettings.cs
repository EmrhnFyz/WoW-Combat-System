using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Unit Renderer Settings", menuName = "Game Data/Rendering/Unit Renderer Settings", order = 1)]
    public class UnitRendererSettings : ScriptableObject
    {
        [SerializeField] private float stealthTransparencyAlpha = 0.5f;
        [SerializeField] private float transparencyTransitionSpeed = 1.0f;
        [SerializeField] private float renderInterpolationSmoothTime = 0.05f;

        public float StealthTransparencyAlpha => stealthTransparencyAlpha;
        public float TransparencyTransitionSpeed => transparencyTransitionSpeed;
        public float RenderInterpolationSmoothTime => renderInterpolationSmoothTime;
    }
}