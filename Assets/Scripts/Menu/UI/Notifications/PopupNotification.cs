using UnityEngine;

public class PopupNotification : Popup
{
    [Range(-1, 20)]
    [SerializeField] private float timeToLive = 6f;


    protected override void OnStart()
    {
        base.OnStart();

        if (timeToLive > 0)
        {
            Invoke("Close", timeToLive);
            Destroy(gameObject, timeToLive + 1f);
        }
    }
}
