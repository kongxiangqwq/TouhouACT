using UnityEngine;

public class GameObjectNULLException : System.ApplicationException
{
    public GameObjectNULLException() : base("GameObject is NULL!!!")
    {
    }
}
