using UnityEngine;

public class MapFragmentDoor : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject displayedOnNonDoorGO = default;
    [SerializeField] private GameObject displayedOnDoorGO = default;

    // ---- INTERN ----
    private bool hasBeenActivated = false;

    public void Activate()
    {
        displayedOnNonDoorGO.SetActive(false);
        displayedOnDoorGO.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasBeenActivated && other.tag == "Player")
        {
            hasBeenActivated = true;
            MapManager2.Instance.NotifyEndOfMap();
        }
    }
}
