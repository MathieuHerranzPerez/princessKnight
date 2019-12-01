using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    // ---- INTERN ----
    private Vector2 screenBounds;

    void Start()
    {
        Debug.Log(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10)));
        Debug.Log(Camera.main.orthographicSize);
        Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)));
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));
        Debug.Log(screenBounds);
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        Debug.Log(viewPos);
        Debug.Log("ScreenBound X : " + screenBounds.x);
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x, screenBounds.x * -1);
        viewPos.z = Mathf.Clamp(viewPos.z, screenBounds.y, screenBounds.y * -1);
        transform.position = viewPos;
    }

    void OnDrawGizmos()
    {
        Camera camera = Camera.main;
        Vector3 p = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 0.1F);
    }
}
