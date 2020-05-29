using UnityEngine;

public class PopupNotification : Popup
{
    [Range(-1, 20)]
    [SerializeField] private float timeToLive = 6f;

    // ---- INTERN ----

    private bool isOpened = true;

    void Start()
    {
        if (timeToLive > 0)
        {
            Invoke("Close", timeToLive);
            Destroy(gameObject, timeToLive + 1f);
        }
    }
}
