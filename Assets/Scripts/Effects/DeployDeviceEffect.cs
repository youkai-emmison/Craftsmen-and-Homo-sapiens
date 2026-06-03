// Script purpose: Deploys a configured Device prefab on the ground near the player.
// Key Inspector variables:
// - deviceData: Device data containing the turret prefab, projectile, and combat numbers.
// - groundLayer: Layer mask used by the downward raycast that finds the landing point.
// - deployOffset: Horizontal/vertical offset from the player before snapping to ground.
using UnityEngine;

[CreateAssetMenu(fileName = "DeployDevice", menuName = "Item Effects/Deploy Device")]
public class DeployDeviceEffect : ItemEffect
{
    [Header("Deploy Config")]
    public DeviceData deviceData;

    [Header("Ground Snap")]
    public LayerMask groundLayer;
    public Vector2 deployOffset = new Vector2(1f, 0f);
    public float raycastStartHeight = 1.5f;
    public float raycastDistance = 5f;
    public float groundYOffset = 0.28f;

    private GameObject deployedDevice;

    public override void ExecuteEffect(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("DeployDeviceEffect: target is null.");
            return;
        }

        if (deviceData == null || deviceData.devicePrefab == null)
        {
            Debug.LogWarning("DeployDeviceEffect: deviceData or devicePrefab is not assigned.");
            return;
        }

        if (!CanDeployDevice())
        {
            Debug.Log($"DeployDeviceEffect: cannot deploy {deviceData.deviceName}; device limit reached.");
            return;
        }

        PlayerStats playerStats = target.GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogWarning("DeployDeviceEffect: PlayerStats is not found on the target.");
            return;
        }

        if (!TryGetGroundDeployPosition(target, out Vector2 deployPosition))
        {
            Debug.LogWarning("DeployDeviceEffect: no ground found below the deploy point.");
            return;
        }

        deployedDevice = Instantiate(deviceData.devicePrefab, deployPosition, Quaternion.identity);
        InitializeDevice(playerStats);
    }

    public override void CancelEffect(Transform target)
    {
        // Deployed devices keep running until their duration or durability ends.
    }

    private bool CanDeployDevice()
    {
        if (DeviceManager.Instance == null)
        {
            Debug.LogWarning("DeployDeviceEffect: DeviceManager is missing in the scene.");
            return false;
        }

        return DeviceManager.Instance.CanDeploy(deviceData.deviceName, deviceData.maxSameType);
    }

    private bool TryGetGroundDeployPosition(Transform target, out Vector2 deployPosition)
    {
        float facingDirection = GetFacingDirection(target);
        Vector2 castOrigin = (Vector2)target.position
            + new Vector2(deployOffset.x * facingDirection, deployOffset.y + raycastStartHeight);

        RaycastHit2D hit = Physics2D.Raycast(castOrigin, Vector2.down, raycastDistance, groundLayer);
        if (hit.collider == null)
        {
            deployPosition = Vector2.zero;
            return false;
        }

        deployPosition = hit.point + Vector2.up * groundYOffset;
        return true;
    }

    private float GetFacingDirection(Transform target)
    {
        PlayerAttackFacingController facingController = target.GetComponent<PlayerAttackFacingController>();
        if (facingController != null)
            return facingController.FacingDirectionX;

        return target.localScale.x >= 0f ? 1f : -1f;
    }

    private void InitializeDevice(PlayerStats playerStats)
    {
        DeviceController controller = deployedDevice.GetComponent<DeviceController>();
        if (controller != null)
            controller.Initialize(deviceData, playerStats);
    }
}
