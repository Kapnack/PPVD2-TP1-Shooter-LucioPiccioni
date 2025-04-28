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

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
    }
}