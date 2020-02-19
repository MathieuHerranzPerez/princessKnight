using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    string GetName();
    object GetData();
}

public interface IEventListener
{
    bool HandleEvent(IEvent evt);
}


public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private bool isQueueProcessingLimited = true;
    [SerializeField] private float maxTimeForProcessing = 0.1f;

    // ---- INTERN ----
    private Dictionary<string, ArrayList> tableListener = new Dictionary<string, ArrayList>();
    Queue<IEvent> queueEvent = new Queue<IEvent>();

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than one EventManager in the scene");
        }
        Instance = this;
    }

    void Update()
    {
        float time = 0f;
        bool stop = false;
        while(queueEvent.Count > 0 && !stop)
        {
            IEvent currentEvt = queueEvent.Dequeue();
            TriggerEvent(currentEvt);

            if (isQueueProcessingLimited)
            {
                time += Time.unscaledTime;
                if (time > maxTimeForProcessing)
                {
                    stop = true;
                }
            }
        }
    }

    public void AddListener(IEventListener listener, string eventName)
    {
        // if listener unknown, create entry in table
        if(!tableListener.ContainsKey(eventName))
        {
            tableListener.Add(eventName, new ArrayList());
        }

        ArrayList currentListEvent = tableListener[eventName];
        if (!currentListEvent.Contains(listener))
        {
            currentListEvent.Add(listener);
        }
    }

    public void DetachListener(IEventListener listener, string eventName)
    {
        tableListener[eventName].Remove(listener);
    }

    public void QueueEvent(IEvent evt)
    {
        if (!tableListener.ContainsKey(evt.GetName()))  // no listener, then leave
            return;

        queueEvent.Enqueue(evt);
    }

    // public if we want to trigger it now
    public void TriggerEvent(IEvent evt)
    {
        foreach(IEventListener listener in tableListener[evt.GetName()])
        {
            listener.HandleEvent(evt);
        }
    }
}
