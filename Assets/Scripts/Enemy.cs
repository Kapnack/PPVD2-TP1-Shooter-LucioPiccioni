using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;

    private CharacterStats actualStats;

    void Awake()
    {
        actualStats = Instantiate(stats);
    }

    void Update()
    {

    }

    public void OnDamage(int damage)
    {
        if (actualStats.actualShield > 0)
        {
            actualStats.actualShield -= damage;
        }
        else
        {
            actualStats.actualHealth -= damage;
        }

        if (actualStats.actualShield <= 0 && actualStats.actualHealth <= 0)
        {
            Debug.Log(gameObject.name + " Is Dead");
            Destroy(gameObject);
        }
    }
}
