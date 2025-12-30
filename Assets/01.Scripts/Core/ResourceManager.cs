using Core.TextUtil;
using UnityEngine;
namespace Project_Unorder.Core
{

    // High priority: highest execution order
    public class ResourceManager : MonoBehaviour
    {
        private void Awake()
        {
            TextUtil.Instance.Initialize();
 
        }
    }
}