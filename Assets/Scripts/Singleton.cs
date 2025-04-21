using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static Singleton<T> instance = null;

    public static T Instance
    {
        get
        {
            return (T)instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}