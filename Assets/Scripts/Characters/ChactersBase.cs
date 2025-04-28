using UnityEngine;

public abstract class ChactersBase : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] public int maxHealth;
    private int _actualHealth;

    [Header("Shield")]
    [SerializeField] public int maxShield;
    private int _actualShield;

    public int ActualHealth
    {
        set => _actualHealth = Mathf.Clamp(value, 0, maxHealth);
        get => _actualHealth;
    }

    public int ActualShield
    {
        set => _actualHealth = Mathf.Clamp(value, 0, maxShield);
        get => _actualHealth;
    }


    public void ReduceHealth(int damage)
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

        if (ActualShield == 0 && ActualHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void OnDead();

    public void OnValidate()
    {
        ActualHealth = maxHealth;

        ActualShield = maxShield;
    }
}
