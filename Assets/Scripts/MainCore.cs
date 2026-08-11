using UnityEngine;

public class MainCore
{
    private static volatile MainCore _instance; 
    private static object _lock = new object();

    static public MainCore Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                lock (_lock) ;
                if (_instance == null)
                {
                    _instance = new MainCore();
                }
            }
            return _instance;
        }
    }

    private MainCore()
    {

    }
}
