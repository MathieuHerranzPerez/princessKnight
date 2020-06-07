using System.Collections.Generic;
using UnityEngine;

public abstract class ResetableManager : MonoBehaviour
{
    protected static List<ResetableManager> listAllManager = new List<ResetableManager>();

    protected virtual void Awake()
    {
        listAllManager.Add(this);
    }

    public abstract void ResetScene();

    void OnDestroy()
    {
        listAllManager.Remove(this);
    }
}
