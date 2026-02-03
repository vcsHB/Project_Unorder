using Project_Unorder.VolumeManage;
using UnityEngine;
using DG.Tweening;

namespace Project_Unorder.FlowSystem
{
    public class VolumeControlCue : FlowCue
    {
        public enum VolumeEffectType
        {
            ColorAdjustments,
            Vignette,
            ChromaticAberration,
            LensDistortion
        }

        [System.Serializable]
        public struct EffectValueHolder
        {
            public const float DefaultExposure = 0f;
            public const float DefaultContrast = 0f;
            public const float DefaultSaturation = 0f;
            public const float DefaultIntensity = 0f;

            [Header("Single Value (Intensity / Exposure)")]
            [Range(-5f, 5f)] public float Intensity; 

            [Header("Color Adjust Extra")]
            [Range(-100f, 100f)] public float Contrast;
            [Range(-100f, 100f)] public float Saturation;
            [ColorUsage(false, true)] public Color FilterColor;

            public static EffectValueHolder Default => new EffectValueHolder
            {
                Intensity = DefaultIntensity,
                Contrast = DefaultContrast,
                Saturation = DefaultSaturation,
                FilterColor = Color.white
            };
        }

        [Header("General Settings")]
        [SerializeField] private VolumeEffectType _effectType;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _easeType = Ease.OutQuad;

        [Header("Target Values")]
        [SerializeField] private EffectValueHolder _targetValues = EffectValueHolder.Default;

        public override void Execute()
        {
            System.Action onDone = () => InvokeCueComplete();
            var volMgr = VolumeManager.Instance;

            switch (_effectType)
            {
                case VolumeEffectType.ColorAdjustments:
                    var colorSettings = new ColorAdjustSettings {
                        exposure = _targetValues.Intensity,
                        contrast = _targetValues.Contrast,
                        saturation = _targetValues.Saturation,
                        filterColor = _targetValues.FilterColor
                    };
                    volMgr.GetCompo<ColorAdjustController>()?.DoColorAdjust(colorSettings, _duration, _easeType, onDone);
                    break;

                case VolumeEffectType.Vignette:
                    volMgr.GetCompo<VignetteController>()?.DoIntensity(_targetValues.Intensity, _duration, onDone);
                    break;

                case VolumeEffectType.ChromaticAberration:
                    volMgr.GetCompo<ChromaticController>()?.DoIntensity(_targetValues.Intensity, _duration, onDone);
                    break;

                case VolumeEffectType.LensDistortion:
                    volMgr.GetCompo<LensDistortionController>()?.DoIntensity(_targetValues.Intensity, _duration, onDone);
                    break;

                default:
                    onDone();
                    break;
            }
        }
    }
}