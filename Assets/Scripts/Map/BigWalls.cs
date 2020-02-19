using UnityEngine;

public class BigWalls : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private int MapColliderLayerNumber = 13;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == MapColliderLayerNumber)
        {
            other.gameObject.SetActive(false);
            Debug.Log("Spawn new map collider");
            MapManager.Instance.NotifyEndOfMap();
        }
    }
}
