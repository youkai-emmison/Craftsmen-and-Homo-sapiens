// Script purpose: Labels important greybox placement points in the scene.
// Key Inspector variables:
// - markerType: The gameplay object that should be placed at this marker.
// - note: Short setup note for teammates reading the Inspector.
using UnityEngine;

public class LevelBlockoutMarker : MonoBehaviour
{
    public enum MarkerType
    {
        PlayerSpawn,
        EarlyEnemy,
        MidEnemy,
        DemoBoss,
        EarlyExitDoor,
        MidExitDoor,
        BossExitDoor,
        CameraStart,
        RoomController
    }

    // Marker purpose shown in the Inspector.
    public MarkerType markerType;

    // Short setup note for manual scene wiring.
    public string note;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GetMarkerColor();
        Gizmos.DrawWireSphere(transform.position, 0.35f);
        Gizmos.DrawLine(transform.position + Vector3.left * 0.45f, transform.position + Vector3.right * 0.45f);
        Gizmos.DrawLine(transform.position + Vector3.down * 0.45f, transform.position + Vector3.up * 0.45f);
    }

    private Color GetMarkerColor()
    {
        if (markerType == MarkerType.PlayerSpawn)
        {
            return Color.green;
        }

        if (markerType == MarkerType.DemoBoss)
        {
            return Color.red;
        }

        if (markerType == MarkerType.EarlyExitDoor || markerType == MarkerType.MidExitDoor || markerType == MarkerType.BossExitDoor)
        {
            return Color.yellow;
        }

        return Color.magenta;
    }
}
