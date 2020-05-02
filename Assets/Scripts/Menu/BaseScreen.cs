using System.Collections;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    public ScreenType Type { get { return type; } }


    [SerializeField] protected ScreenType type = default;
    [SerializeField] protected bool destroyOnExit = true;


    // ---- INTERN ----
    protected Hashtable parameters;


    void Awake()
    {
        OnAwake();
    }
    protected virtual void OnAwake() { }

    void Start()
    {
        OnStart();
    }
    protected virtual void OnStart() { }

    void Update()
    {
        OnUpdate();
    }
    protected virtual void OnUpdate() { }

    public void Exit()
    {
        OnExit();
        if(destroyOnExit)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    public void NavigateTo(ScreenType screenType)
    {
        ScreenManager.Instance.ShowScreen(screenType);
    }

    public void SetParams(Hashtable parameters)
    {
        if(parameters != null)
        {
            foreach(Object key in parameters.Keys)
            {
                if(this.parameters.Contains(key))
                {
                    this.parameters[key] = parameters[key];
                }
                else
                {
                    this.parameters.Add(key, parameters[key]);
                }
            }
        }
    }

    protected virtual void OnExit() { }
}
