using UnityEngine;

namespace Project_Unorder.AgentSystem.InteractSystem
{
    public class DragZoneVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _edgePanel;
        [SerializeField] private Transform _backgroundTrm;

        public void UpdateSize(Vector2 start, Vector2 current)
        {
            Vector2 delta = current - start;

            float width = Mathf.Abs(delta.x);
            float height = Mathf.Abs(delta.y);

            // 사이즈 적용
            _edgePanel.size = new Vector2(width, height);
            _backgroundTrm.localScale = new Vector3(
                width * 0.5f,
                height * 0.5f,
                1f
            );

            // 방향 보정 (하나로 통일)
            Vector3 offset = new Vector3(
                delta.x < 0 ? -width : 0f,
                delta.y > 0 ? height : 0f,
                0f
            );

            _edgePanel.transform.localPosition = offset;
            _backgroundTrm.localPosition = offset;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
