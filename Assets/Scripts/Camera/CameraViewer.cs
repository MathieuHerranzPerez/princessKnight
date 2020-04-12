using UnityEngine;

public class CameraViewer : MonoBehaviour
{
    public static CameraViewer Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private MonoBehaviour[] arrayStoppableToStopWhenMoving = new MonoBehaviour[1];
    private IStoppable[] arrayStoppable;   // not shown in inspector...

    // ---- INTERN ----
    private Vector3 startPos = Vector3.zero;
    private Quaternion startRot = Quaternion.identity;

    private Transform lastMoveTransform = null;

    private Transform target = null;
    private float timeToGo = 0f;
    private float time = 0f;

    private bool isReseting = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        arrayStoppable = new IStoppable[arrayStoppableToStopWhenMoving.Length];
        int i = 0;
        // convert the Monobahavior list to IStoppable
        foreach (MonoBehaviour mb in arrayStoppableToStopWhenMoving)
        {
            arrayStoppable[i] = (IStoppable) mb;
            ++i;
        }
    }

    void Update()
    {
        if(target != null)
        {
            // position
            transform.position = Vector3.Lerp(startPos, target.position, time / timeToGo);
            // rotation
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, time / timeToGo);

            time += Time.unscaledDeltaTime;

            if(time >= timeToGo)
            {
                target = null;
            }
        }
        else if(isReseting)
        {
            // position
            transform.position = Vector3.Lerp(lastMoveTransform.position, startPos, time / timeToGo);
            // rotation
            transform.rotation = Quaternion.Lerp(lastMoveTransform.rotation, startRot, time / timeToGo);

            time += Time.unscaledDeltaTime;

            if (time >= timeToGo)
            {
                // put it well
                transform.position = startPos;
                transform.rotation = startRot;

                isReseting = false;
                foreach (IStoppable stoppable in arrayStoppable)
                {
                    stoppable.Continue();
                }
            }
        }
    }

    public void GoTo(Transform target, float timeToGo)
    {
        if (this.target != null)
            return;

        startPos = transform.position;
        startRot = transform.rotation;
        time = 0f;

        this.timeToGo = timeToGo;
        this.lastMoveTransform = target;

        // need to stop all stoppable when moving
        foreach (IStoppable stoppable in arrayStoppable)
        {
            stoppable.StopAction();
        }

        this.target = target;
    }

    public void Reset(float timeToGo)
    {
        time = 0f;
        this.timeToGo = timeToGo;

        isReseting = true;
    }
}
