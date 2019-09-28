using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{

    [SerializeField]
    private Transform target = default;
    [SerializeField]
    [Range(0.05f, 1.0f)]
    private float smoothFactor = 1f;

    // ---- INTERN ----
    private Vector3 cameraOffset;

    void Start()
    {
        // set the camera offset to scene offset
        cameraOffset = transform.position - target.transform.position;
    }

    // for movement to perform before it
    void LateUpdate()
    {
        Vector3 newPos = target.position + cameraOffset;

        // opti if smoothFactor == 1
        if (smoothFactor < 0.99f)
        {
            transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
        }
        else
        {
            transform.position = newPos;
        }
    }
}
