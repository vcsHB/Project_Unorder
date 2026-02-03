using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Project_Unorder.VolumeManage
{
    public class ChromaticController : MonoBehaviour, IVolumeComponent
    {
        private ChromaticAberration _chromatic;
        public void Initialize(VolumeManager manager)
        {
            manager.GlobalVolume.profile.TryGet(out _chromatic);
        }

        public void SetIntensity(float intensity)
        {
            if (_chromatic)
                _chromatic.intensity.value = intensity;
        }

        public void DoIntensity(float endValue, float duration, System.Action onComplete = null)
        {
            if (!_chromatic) return;
            DOTween.To(() => _chromatic.intensity.value, x => _chromatic.intensity.value = x, endValue, duration)
                .OnComplete(() => onComplete?.Invoke());
        }
    }

}