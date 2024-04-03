public interface IDamageable
{
    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    
    void Damage(float amount);

    void Die();
}