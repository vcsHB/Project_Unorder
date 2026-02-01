using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Project_Unorder.VolumeManage
{

    public class VignetteController : MonoBehaviour, IVolumeComponent
    {
        private Vignette _vignette;

        public void Initialize(VolumeManager manager)
        {
            if (manager.GlobalVolume.profile.TryGet(out Vignette vignette))
            {
                _vignette = vignette;
            }
        }

        public void SetIntensity(float intensity)
        {
            if (_vignette != null)
                _vignette.intensity.value = intensity;
        }
    }
}