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

        public void SetExposure(float value) { if (_colorAdjust) _colorAdjust.postExposure.value = value; }
        public void SetContrast(float value) { if (_colorAdjust) _colorAdjust.contrast.value = value; }
        public void SetColorFilter(Color color) { if (_colorAdjust) _colorAdjust.colorFilter.value = color; }
        public void SetSaturation(float saturation) { if (_colorAdjust) _colorAdjust.saturation.value = saturation; }
    }


}