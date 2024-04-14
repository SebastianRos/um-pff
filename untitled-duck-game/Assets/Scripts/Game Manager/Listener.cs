using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

interface IListener {
    public void Callback(string evt);
}

public abstract class Listener : MonoBehaviour, IListener
{    public abstract void Callback(string evt);
}
