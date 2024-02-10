using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonColliderGizmoDrawer : MonoBehaviour
{
    public Color color = Color.red;
    private void OnDrawGizmos()
    {
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            Gizmos.color = color; // Set the color of the gizmo lines

            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                Vector2[] path = polygonCollider.GetPath(i);
                for (int j = 0; j < path.Length; j++)
                {
                    // Draw line from each point to the next, and connect the last to the first
                    Vector2 startPoint = transform.TransformPoint(path[j]);
                    Vector2 endPoint = transform.TransformPoint(path[(j + 1) % path.Length]);
                    Gizmos.DrawLine(startPoint, endPoint);
                }
            }
        }
    }
}