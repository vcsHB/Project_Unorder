using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Project_Unorder.VolumeManage
{

    public class VignetteController : MonoBehaviour, IVolumeComponent
    {
        private Vignette _vignette;
        public void Initialize(VolumeManager manager)
        {
            manager.GlobalVolume.profile.TryGet(out _vignette);
        }

        public void SetIntensity(float intensity)
        {
            if (_vignette)
                _vignette.intensity.value = intensity;
        }

        public void DoIntensity(float endValue, float duration, System.Action onComplete = null)
        {
            if (!_vignette) return;
            DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, endValue, duration)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}