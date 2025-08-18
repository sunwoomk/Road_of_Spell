using UnityEngine;

public class Singleton<T> where T : new()
{
    static private T _instance;

    static public T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }
}
