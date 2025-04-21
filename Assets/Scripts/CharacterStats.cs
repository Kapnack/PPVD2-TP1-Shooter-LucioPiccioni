using UnityEngine;

[CreateAssetMenu(fileName = "New Character Stats", menuName = "CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Life")]
    [SerializeField][Range(0, 100)] public int maxHealth;
    [HideInInspector] public int actualHealth;

    [Header("Shield")]
    [SerializeField][Range(0, 100)] public int maxShield;
    [SerializeField] public int actualShield;

    private void OnValidate()
    {
        actualHealth = maxHealth;

        actualShield = Mathf.Clamp(actualShield, 0, maxShield);
    }
}
