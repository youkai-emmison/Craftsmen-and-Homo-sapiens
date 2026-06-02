using UnityEngine;

public class ItemEffectManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private float flaskCooldown = 5f;

    private float flaskCooldownTimer;

    private void Awake()
    {
        if (inventory == null) inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (flaskCooldownTimer > 0f)
            flaskCooldownTimer -= Time.deltaTime;

        // 按键 4 使用药水
        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseFlask();
    }

    public void OnAttackHit(Transform target)
    {
        EquipmentData weapon = inventory.GetEquipped(EquipmentType.Weapon);
        if (weapon == null || weapon.effects == null) return;
        foreach (var effect in weapon.effects)
            if (effect != null) effect.ExecuteEffect(target);
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
}
