public interface IHealthSystem
{
    public void ReciveDamage(float damage);

    public void ResetHealth();

    public bool IsDead();
}