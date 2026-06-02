using UnityEngine;

/// <summary>
/// 挂载在 Player 子物体 GroundCheck 上，负责地面射线检测。
/// 子物体的位置即为射线起点。
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkDistance = 0.1f;

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * checkDistance);
    }
}
