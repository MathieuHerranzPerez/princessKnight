using UnityEngine;

public class BigWalls : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    void FixedUpdate()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // todo if other == ?
        MapManager.Instance.NotifyEndOfMap();
    }
}
