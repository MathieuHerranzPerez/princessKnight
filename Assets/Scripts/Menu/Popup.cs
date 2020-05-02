using System;
using UnityEngine;

public enum PopupResult
{
    Close,
    Yes,
    No,
    Ok,
    Cancel,
    FirstOption,
    SecondOption,
    ThirdOption,
    AutoClose,
    Error
}

public class Popup : MonoBehaviour
{
    public event Action<Popup> OnOpened;
    public event Action<Popup> OnClosed;

    public bool DontDestroyOnClose { get { return !destroyOnClose; } }

    [SerializeField] protected bool destroyOnClose = true;
    [SerializeField] protected PopupResult defaultResult = default;

    // ---- INTERN ----
    protected PopupResult result;

    void Awake()
    {
        result = defaultResult;
        OnAwake();
    }
    protected virtual void OnAwake() { }

    void Start()
    {
        OnStart();
        OnOpened?.Invoke(this);
    }
    protected virtual void OnStart() { }

    void Update()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate() { }

    void OnDestroy()
    {
        OnDestroyed();
    }
    protected virtual void OnDestroyed() { }

    public void ClaseDefault()
    {
        Close(defaultResult);
    }

    public virtual void Close(PopupResult result = PopupResult.Close)
    {
        this.result = result;
        Close();
    }

    public virtual void Close()
    {
        if(destroyOnClose)
        {
            // close animation
        }

        InternalClose();
    }

    protected virtual void InternalClose()
    {
        if(destroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            OnClosed?.Invoke(this);
        }
    }
}
