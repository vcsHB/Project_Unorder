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
            if (_lensDistortion) _lensDistortion.intensity.value = intensity;
        }

        public void SetScale(float scale)
        {
            if (_lensDistortion) _lensDistortion.scale.value = scale;
        }
    }
}