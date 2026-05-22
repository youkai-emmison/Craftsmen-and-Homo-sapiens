// Script purpose: Defines the minimal contract for objects that can take damage.
// Key variables:
// - damageAmount: Amount passed by the attacker to the receiving object.
public interface IDamageable
{
    // Keep the first damage contract tiny until combat rules actually need more data.
    void TakeDamage(int damageAmount);
}
