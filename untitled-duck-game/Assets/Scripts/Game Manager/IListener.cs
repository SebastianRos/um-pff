using UnityEngine;

public interface IListener {
    public void Callback(string evt);
}

public abstract class Listener : MonoBehaviour, IListener
{    public abstract void Callback(string evt);
}
