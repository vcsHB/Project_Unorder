using Project_Unorder.AgentSystem.PlayerManage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Project_Unorder.UIManage.InGameSceneUI
{

    public class PlayerStatusItem : MonoBehaviour
    {

        [SerializeField] private PlayerDataSO _playerData;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _mainGaugeImage;


        private IColorChangeable[] _colorChangeables;

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            _colorChangeables = GetComponentsInChildren<IColorChangeable>();
        }


        private void SetColor(Color newColor)
        {
            if (_colorChangeables == null) return;

            for (int i = 0; i < _colorChangeables.Length; i++)
            {
                _colorChangeables[i].ChangeColor(newColor);
            }
        }

        private void SetDataInformation(PlayerDataSO data)
        {
            _nameText.text = data.playerType.ToString().ToUpper();
            SetColor(data.personalColor);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_playerData == null) return;
            
            Initialize();
            SetDataInformation(_playerData);
        }
#endif
    }
}