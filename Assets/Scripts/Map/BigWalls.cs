using UnityEngine;

public class BigWalls : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    [Header("Setup")]
    [SerializeField]
    private int MapColliderLayerNumber = 13;

    void FixedUpdate()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == MapColliderLayerNumber)
        {
            Debug.Log("Spawn new map collider");
            MapManager.Instance.NotifyEndOfMap();
        }
    }
}
