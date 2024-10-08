using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : class
{
    private static T _instance;
    private static object _locker = new();
    
    public static T Instance 
    {
        get
        {
            lock (_locker)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;

                    if (_instance == null)
                    {
                        Debug.LogError("Instance does not exist in game.");
                        return null;
                    }
                }

                return _instance;
            }
        }
    }
}