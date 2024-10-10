namespace Backend.Server.ETC;

public class Singleton<T> where T : new()
{
    private static T? _instance;
    private static readonly object _locker = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_locker)
                {
                    _instance = new T();
                }
            }

            return _instance;
        }
    }
}