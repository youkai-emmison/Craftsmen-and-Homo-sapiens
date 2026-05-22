// Script purpose: Marks important greybox placement points in the Unity Editor.
// Key Inspector variables:
// - markerType: Type of object that should be placed at this marker.
// - note: Short setup reminder visible in the Inspector.
using UnityEngine;

public class LevelBlockoutMarker : MonoBehaviour
{
    // Marker categories used by StageBlockoutBuilder.
    public enum MarkerType
    {
        PlayerSpawn,
        TrainingTarget,
        BasicEnemy,
        ExitDoor,
        CameraStart
    }

    // Selected marker category for this point.
    public MarkerType markerType;

    // Human-readable setup note for collaborators.
    public string note;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GetMarkerColor();
        Gizmos.DrawWireSphere(transform.position, 0.35f);
        Gizmos.DrawLine(transform.position + Vector3.down * 0.5f, transform.position + Vector3.up * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.right * 0.5f);
    }

    private Color GetMarkerColor()
    {
        switch (markerType)
        {
            case MarkerType.PlayerSpawn:
                return Color.cyan;
            case MarkerType.TrainingTarget:
                return Color.magenta;
            case MarkerType.BasicEnemy:
                return new Color(0.45f, 0.2f, 0.7f);
            case MarkerType.ExitDoor:
                return Color.yellow;
            case MarkerType.CameraStart:
                return Color.white;
            default:
                return Color.gray;
        }
    }
}
