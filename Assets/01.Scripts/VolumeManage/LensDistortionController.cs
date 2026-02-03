using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Project_Unorder.VolumeManage
{
    public class LensDistortionController : MonoBehaviour, IVolumeComponent
    {
        private LensDistortion _lensDistortion;

        public void Initialize(VolumeManager manager)
        {
            manager.GlobalVolume.profile.TryGet(out _lensDistortion);
        }

        public void SetIntensity(float intensity)
        {
            if (_lensDistortion)
                _lensDistortion.intensity.value = intensity;
        }

        public void DoIntensity(float endValue, float duration, System.Action onComplete = null)
        {
            if (!_lensDistortion) return;
            DOTween.To(() => _lensDistortion.intensity.value, x => _lensDistortion.intensity.value = x, endValue, duration)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}