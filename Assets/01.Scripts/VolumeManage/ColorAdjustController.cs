using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Project_Unorder.VolumeManage
{
    public class ColorAdjustController : MonoBehaviour, IVolumeComponent
    {
        private ColorAdjustments _colorAdjust;

        public void Initialize(VolumeManager manager)
        {
            manager.GlobalVolume.profile.TryGet(out _colorAdjust);
        }

        public void SetColorAdjust(ColorAdjustSettings settings)
        {
            if (_colorAdjust)
            {
                _colorAdjust.postExposure.value = settings.exposure;
                _colorAdjust.contrast.value = settings.contrast;
                _colorAdjust.saturation.value = settings.saturation;
                _colorAdjust.colorFilter.value = settings.filterColor;
            }
        }

        public void DoColorAdjust(ColorAdjustSettings settings, float duration, Ease ease, System.Action onComplete)
        {
            if (!_colorAdjust) { onComplete?.Invoke(); return; }

            Sequence seq = DOTween.Sequence();

            // Post Exposure
            seq.Join(DOTween.To(() => _colorAdjust.postExposure.value,
                x => _colorAdjust.postExposure.value = x, settings.exposure, duration));

            // Contrast
            seq.Join(DOTween.To(() => _colorAdjust.contrast.value,
                x => _colorAdjust.contrast.value = x, settings.contrast, duration));

            // Saturation
            seq.Join(DOTween.To(() => _colorAdjust.saturation.value,
                x => _colorAdjust.saturation.value = x, settings.saturation, duration));

            // Color Filter
            seq.Join(DOTween.To(() => _colorAdjust.colorFilter.value,
                x => _colorAdjust.colorFilter.value = x, settings.filterColor, duration));

            seq.SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }
    }

    [System.Serializable]
    public struct ColorAdjustSettings
    {
        public float exposure;
        public float contrast;
        public float saturation;
        [ColorUsage(false, true)] public Color filterColor;
    }
}