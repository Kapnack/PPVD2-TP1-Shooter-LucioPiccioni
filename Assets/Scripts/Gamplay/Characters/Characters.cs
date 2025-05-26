using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] public int maxHealth = 100;
    private float _actualHealth;

    [Header("Shield")]
    [SerializeField] public int maxShield = 50;
    private float _actualShield;

    public float ActualHealth
    {
        set => _actualHealth = Mathf.Clamp(value, 0, maxHealth);
        get => _actualHealth;
    }

    public float ActualShield
    {
        set => _actualShield = Mathf.Clamp(value, 0, maxShield);
        get => _actualShield;
    }


    public virtual void ReciveDamage(float damage)
    {
        if (_actualShield > 0)
        {
            _actualShield -= damage;

            if (_actualShield < 0)
            {
                float remainingDamage = -_actualShield;
                _actualShield = 0;
                ActualHealth -= remainingDamage;
            }
        }
        else
        {
            ActualHealth -= damage;
        }

        if(IsDead())
            OnDead();
    }

    public virtual bool IsDead() => ActualShield == 0 && ActualHealth == 0;

    protected abstract void OnDead();

    public void OnValidate()
    {
        ActualHealth = maxHealth;

        ActualShield = maxShield;
    }
}
