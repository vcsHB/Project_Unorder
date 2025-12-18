using System.Collections;
using UnityEngine;

namespace Project_Unorder.CameraSystem
{
    public class CameraZoomComponent : CameraComponentBase
    {
        [SerializeField] private float _defaultZoomLevel;
        private Coroutine _zoomRoutine;

        public override void Initialize(CameraManager manager)
        {
            base.Initialize(manager);
            _defaultZoomLevel = Manager.MainCamera.Lens.OrthographicSize;

        }

        public void SetZoomDefaultImmediately()
        {
            Manager.MainCamera.Lens.OrthographicSize = _defaultZoomLevel;
        }

        public void SetZoomDefaut(float duration)
        {
            StartZoom(_defaultZoomLevel, duration);
        }

        public void StartZoom(float targetSize, float duration)
        {
            if (_zoomRoutine != null)
                StopCoroutine(_zoomRoutine);

            _zoomRoutine = StartCoroutine(ZoomRoutine(targetSize, duration));
        }

        private IEnumerator ZoomRoutine(float target, float duration)
        {
            float start = Manager.MainCamera.Lens.OrthographicSize;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                Manager.MainCamera.Lens.OrthographicSize =
                    Mathf.Lerp(start, target, time / duration);
                yield return null;
            }

            Manager.MainCamera.Lens.OrthographicSize = target;
            _zoomRoutine = null;
        }
    }
}
