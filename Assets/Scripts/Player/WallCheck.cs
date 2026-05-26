using UnityEngine;

/// <summary>
/// 挂载在 Player 子物体 WallCheck 上，负责墙面射线检测。
/// 子物体的位置即为射线起点。
/// </summary>
public class WallCheck : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkDistance = 0.3f;

    public bool IsWallDetected(int facingDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, checkDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * checkDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * checkDistance);
    }
}
