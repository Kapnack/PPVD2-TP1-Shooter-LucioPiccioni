using UnityEngine;

public abstract class Characters : MonoBehaviour, IHealthSystem
{
    [Header("Life")]
    [SerializeField] public int _maxHealth = 100;
    private float _currentHealth;

    [Header("Shield")]
    [SerializeField] public int _maxShield = 50;
    private float _currentShield;

    public float CurrentHealth
    {
        set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
        get => _currentHealth;
    }

    public float CurrentShield
    {
        get => _currentShield;
    }

    public float MaxHealth
    {
        get => _maxHealth;
    }

    public float MaxShield
    {
        get => _maxShield;
    }

    protected void OnAwake()
    {
        _currentHealth = _maxHealth;
        _currentShield = _maxShield;
    }

    public virtual void ReciveDamage(float damage)
    {
        if (_currentShield > 0)
        {
            _currentShield -= damage;

            if (_currentShield < 0)
            {
                float remainingDamage = -_currentShield;
                _currentShield = 0;
                CurrentHealth -= remainingDamage;
            }
        }
        else
        {
            CurrentHealth -= damage;
        }

        if (IsDead())
            OnDead();
    }

    public bool IsDead()
    {
        return CurrentShield == 0 && CurrentHealth == 0;
    }

    protected abstract void OnDead();

    private void OnValidate()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        _currentShield = _maxShield;
    }
}
