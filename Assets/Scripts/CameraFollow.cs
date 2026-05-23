using UnityEngine;

/// <summary>
/// 带死区的相机跟随。
/// 玩家在死区内移动时相机静止，超出死区边缘后相机跟随并尝试将中心对准玩家。
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    [SerializeField] private Transform target;

    [Header("死区（屏幕中心为原点）")]
    [SerializeField] private float deadZoneWidth = 2f;   // 死区半宽
    [SerializeField] private float deadZoneHeight = 1f;  // 死区半高

    [Header("跟随平滑")]
    [SerializeField] private float smoothSpeed = 5f;

    [Header("相机高度")]
    [SerializeField] private float zOffset = -10f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position;

        // X 轴：超出死区才跟随
        float deltaX = targetPos.x - camPos.x;
        if (Mathf.Abs(deltaX) > deadZoneWidth)
            camPos.x = Mathf.Lerp(camPos.x, targetPos.x - Mathf.Sign(deltaX) * deadZoneWidth, smoothSpeed * Time.deltaTime);

        // Y 轴：超出死区才跟随
        float deltaY = targetPos.y - camPos.y;
        if (Mathf.Abs(deltaY) > deadZoneHeight)
            camPos.y = Mathf.Lerp(camPos.y, targetPos.y - Mathf.Sign(deltaY) * deadZoneHeight, smoothSpeed * Time.deltaTime);

        camPos.z = zOffset;

        transform.position = camPos;
    }

    private void OnDrawGizmosSelected()
    {
        // 在 Scene 视图中绘制死区范围
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireCube(transform.position, new Vector3(deadZoneWidth * 2, deadZoneHeight * 2, 0));
    }
}
