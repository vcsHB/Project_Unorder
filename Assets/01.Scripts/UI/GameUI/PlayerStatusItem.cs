using System.Collections;
using Project_Unorder.AgentSystem.PlayerManage;
using TMPro;
using UnityEngine;

namespace Project_Unorder.UIManage.InGameSceneUI
{

    public class PlayerStatusItem : MonoBehaviour, IWindowPanel
    {

        [SerializeField] private PlayerDataSO _playerData;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private UIGauge _healthGauge;
        [SerializeField] private float _textPrintDelay = 0.1f;
        [Header("Animation Setting")]
        [SerializeField] private string _openKey;
        [SerializeField] private string _closeKey;

        [SerializeField] private bool _isStartRefresh;

        private IColorChangeable[] _colorChangeables;
        private Player _owner;
        private WaitForSeconds _waitForSecond;
        private Animation _animation;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            Debug.Assert(_playerData);
            Debug.Assert(_nameText);
            Debug.Assert(_healthGauge);
            Debug.Assert(_animation);

            _waitForSecond = new WaitForSeconds(_textPrintDelay);
            _owner = _playerData.PlayerInstance;

            Initialize();
        }

        private void Start()
        {
            _playerData.PlayerInstance.HealthBody.OnHealthIncreaseEvent += HandleHealthChanged;
            _playerData.PlayerInstance.HealthBody.OnHealthDecreaseEvent += HandleHealthChanged;
        }



        private void Initialize()
        {
            _colorChangeables = GetComponentsInChildren<IColorChangeable>();
        }
        private void HandleHealthChanged(float current, float max)
        {
            _healthGauge.SetGaugeFill(current / max);
        }

        public void RefreshData()
        {
            _healthGauge.SetGaugeFillImmediately(0f);
            _healthGauge.SetGaugeFill(_owner.HealthBody.CurrentHealth / _owner.HealthBody.MaxHealth);
            SetDataInformation(_playerData);
            StartCoroutine(PrintNameTextCoroutine());
        }
        private IEnumerator PrintNameTextCoroutine()
        {
            _nameText.maxVisibleCharacters = 0;
            int nameLength = _playerData.playerType.ToString().Length;
            for (int i = 1; i <= nameLength; i++)
            {
                _nameText.maxVisibleCharacters = i;
                yield return _waitForSecond;
            }
        }

        private void SetColor(Color newColor)
        {
            if (_colorChangeables == null) return;

            for (int i = 0; i < _colorChangeables.Length; i++)
            {
                _colorChangeables[i].ChangeColor(newColor);
            }
        }

        public void SetPlayerData(PlayerDataSO data)
        {
            _playerData = data;

        }

        private void SetDataInformation(PlayerDataSO data)
        {
            _nameText.text = data.playerType.ToString().ToUpper();
            SetColor(data.personalColor);
        }
        public void Open()
        {
            _animation.Play(_openKey);
        }

        public void Close()
        {
            _animation.Play(_closeKey);
        }

    }
}