using UnityEngine;

public class SliceTester : MonoBehaviour
{
    public RectMeshCutter cutter; 
    public LayerMask targetLayer; 
    private Vector2 startPos;
    private Vector2 endPos;

    private void Update()
    {
        // TEST CODE
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DetectAndSlice();
        }
    }

    void DetectAndSlice()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(startPos, endPos, targetLayer);

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                cutter.Slice(hit.collider.gameObject, startPos, endPos);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
    }
}