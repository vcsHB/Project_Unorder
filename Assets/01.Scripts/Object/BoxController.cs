using UnityEngine;
namespace Project_Unorder.ObjectManage
{

    public class BoxController : MonoBehaviour
    {
        [SerializeField] private MapBox _mapbox;

        public Transform BoxTransform => _mapbox.transform;

        
        
    }
}