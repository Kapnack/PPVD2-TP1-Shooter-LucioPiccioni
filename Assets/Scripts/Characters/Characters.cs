using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] public int maxHealth = 100;
    private int _actualHealth;

    [Header("Shield")]
    [SerializeField] public int maxShield = 50;
    private int _actualShield;

    public int ActualHealth
    {
        set => _actualHealth = Mathf.Clamp(value, 0, maxHealth);
        get => _actualHealth;
    }

    public int ActualShield
    {
        set => _actualShield = Mathf.Clamp(value, 0, maxShield);
        get => _actualShield;
    }


    public bool IsDeadAfterDamage(int damage)
    {
        if (_actualShield > 0)
        {
            _actualShield -= damage;

            if (_actualShield < 0)
            {
                int remainingDamage = -_actualShield;
                _actualShield = 0;
                ActualHealth -= remainingDamage;
            }
        }
        else
        {
            ActualHealth -= damage;
        }

        if (isDead())
        {
            OnDead();
            return true;
        }

        return false;
    }

    private bool isDead() => ActualShield == 0 && ActualHealth == 0;

    protected abstract void OnDead();

    public void OnValidate()
    {
        ActualHealth = maxHealth;

        ActualShield = maxShield;
    }
}
