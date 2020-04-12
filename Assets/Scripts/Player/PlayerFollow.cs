using UnityEngine;

public class PlayerFollow : MonoBehaviour, IStoppable
{
    [SerializeField]
    private Transform target = default;
    [SerializeField]
    [Range(0.05f, 1.0f)]
    private float smoothFactor = 1f;
    [SerializeField] private float maxDistanceOnSides = 5f;
    [SerializeField] private float maxDistanceTargetCanComeback = 30f;

    // ---- INTERN ----
    private Vector3 cameraOffset;
    private float maxTargetZPos;
    private float lastTargetZPos;
    private float firstXPos;

    private bool isStopped = false;

    void Start()
    {
        // set the camera offset to scene offset
        cameraOffset = transform.position - target.transform.position;

        maxTargetZPos = target.position.z;
        lastTargetZPos = target.position.z;
        firstXPos = transform.position.x;
    }

    // for movement to perform before it
    void LateUpdate()
    {
        if (isStopped)
            return;

        Vector3 newPos = target.position + cameraOffset;


        if(target.position.x > firstXPos + maxDistanceOnSides)
        {
            newPos.x = maxDistanceOnSides;
        }
        else if(target.position.x < firstXPos - maxDistanceOnSides)
        {
            newPos.x = -maxDistanceOnSides;
        }

        // to stop the camera if the player comeback to much
        if(target.position.z < maxTargetZPos - maxDistanceTargetCanComeback)
        {
            newPos.z = lastTargetZPos + cameraOffset.z;
        }
        else
        {
            lastTargetZPos = target.position.z;
        }

        // TODO vibration on cam ?
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
    }

    public void StopAction()
    {
        isStopped = true;
    }

    public void Continue()
    {
        isStopped = false;
    }
}
