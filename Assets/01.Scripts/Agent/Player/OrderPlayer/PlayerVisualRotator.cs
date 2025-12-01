using UnityEngine;

public class PlayerVisualRotator : MonoBehaviour
{
    [Header("Rolling Settings")]
    [SerializeField] private float _baseRollSpeed = 360f;
    [SerializeField] private float _inertiaDuration = 1.5f; // Inertia holding time
    [SerializeField] private float _deceleration = 2f; // deceleration rate

    private Vector2 _direction = Vector2.zero;
    private float _currentRoll = 0f;
    private float _currentRollSpeed = 0f;
    private float _inertiaTimer = 0f;
    private float _lastRollSign = 0f;


    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }


    private void Update()
    {
        //_direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_direction.sqrMagnitude > 0.01f)
        {
            RotateByDirection(_direction);
            _inertiaTimer = 0f; // Reset Inertial Timer
        }
        else
        {
            ApplyInertia();
        }

        transform.rotation = Quaternion.Euler(0f, 0f, _currentRoll);
    }

    private void RotateByDirection(Vector2 dir)
    {
        float rollSign = GetRollSign(dir);

        _currentRollSpeed = _baseRollSpeed * rollSign;
        _lastRollSign = rollSign;

        _currentRoll += _currentRollSpeed * Time.deltaTime;
    }

    private void ApplyInertia()
    {
        if (Mathf.Abs(_currentRollSpeed) > 0.01f)
        {
            _currentRollSpeed = Mathf.MoveTowards(_currentRollSpeed, 0f, _baseRollSpeed * _deceleration * Time.deltaTime);

            _currentRoll += _currentRollSpeed * Time.deltaTime;
            _inertiaTimer += Time.deltaTime;
        }
        else
        {
            _currentRollSpeed = 0f;
        }
    }

    private float GetRollSign(Vector2 dir)
    {
        if (dir.x > 0.1f || dir.y > 0.1f)
            return -1f;
        if (dir.x < -0.1f || dir.y < -0.1f)
            return 1f;

        return _lastRollSign;
    }
}
