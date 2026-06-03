// Script purpose: Executes equipped item effects from combat hits and hotkey item use.
// Key Inspector variables:
// - inventory: Player inventory source for equipped gear and device hot slots.
// - flaskCooldown: Cooldown for flask effects.
// - deviceDeployKey: Key that deploys the first Device equipped in weapon hot slots.
using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private float flaskCooldown = 5f;
    [SerializeField] private KeyCode deviceDeployKey = KeyCode.Alpha5;

    private float flaskCooldownTimer;

    private void Awake()
    {
        if (inventory == null) inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (flaskCooldownTimer > 0f)
            flaskCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseFlask();

        if (Input.GetKeyDown(deviceDeployKey))
            UseFirstDeviceInWeaponSlots();
    }

    public void OnAttackHit(Transform target)
    {
        if (inventory == null) return;

        for (int i = 0; i < inventory.MaxWeaponSlots; i++)
        {
            EquipmentData weapon = inventory.GetEquippedWeapon(i);
            if (weapon == null || weapon.equipmentType != EquipmentType.Weapon) continue;
            ExecuteEffects(weapon, target);
        }
    }

    public void OnDamaged(Transform attacker)
    {
        EquipmentData armor = inventory.GetEquipped(EquipmentType.Armor);
        if (armor == null || armor.effects == null) return;
        foreach (var effect in armor.effects)
            if (effect != null) effect.ExecuteEffect(attacker);
    }

    public void UseFlask()
    {
        if (flaskCooldownTimer > 0f) return;

        EquipmentData flask = inventory.GetEquipped(EquipmentType.Flask);
        if (flask == null || flask.effects == null) return;

        foreach (var effect in flask.effects)
            if (effect != null) effect.ExecuteEffect(transform);

        flaskCooldownTimer = flaskCooldown;
    }

    public void UseFirstDeviceInWeaponSlots()
    {
        if (inventory == null) return;

        for (int i = 0; i < inventory.MaxWeaponSlots; i++)
        {
            EquipmentData equipmentData = inventory.GetEquippedWeapon(i);
            if (equipmentData == null || equipmentData.equipmentType != EquipmentType.Device) continue;

            ExecuteEffects(equipmentData, transform);
            return;
        }

        Debug.Log("ItemEffectManager: no Device equipped in weapon slots.");
    }

    private void ExecuteEffects(EquipmentData equipmentData, Transform target)
    {
        if (equipmentData == null || equipmentData.effects == null) return;

        foreach (var effect in equipmentData.effects)
            if (effect != null) effect.ExecuteEffect(target);
    }
}
