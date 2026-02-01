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
            if (_chromatic) _chromatic.intensity.value = intensity;
        }
    }

}