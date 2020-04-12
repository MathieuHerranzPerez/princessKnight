using System.Collections;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Animator animator = default;
    [SerializeField] private Transform[] arrayCameraPoint = new Transform[1];
    [SerializeField] private float timeToShowDoor = 2f;

    public void Close()
    {
        animator.SetTrigger("Close");
    }

    public void Open()
    {
        StartCoroutine(OpenDoorAfterDelay(2f));
    }

    private IEnumerator OpenDoorAfterDelay(float delaySec)
    {
        yield return new WaitForSeconds(delaySec);

        // freeze time
        Debug.Log("Freeze"); // affD
        TimeManager.Instance.Freeze();

        // TODO
        System.Random random = new System.Random();
        CameraViewer.Instance.GoTo(arrayCameraPoint[random.Next(0, arrayCameraPoint.Length)], 0.5f);

        yield return new WaitForSecondsRealtime(0.6f);                  // wait the camera to move

        // open the door
        animator.SetTrigger("Open");
        yield return new WaitForSecondsRealtime(timeToShowDoor);        // watch the door opening

        CameraViewer.Instance.Reset(1.5f);

        yield return new WaitForSecondsRealtime(1.6f);                  // wait the camera to move

        // unfreeze
        Debug.Log("Unfreeze"); // affD
        TimeManager.Instance.UnFreeze();

        Destroy(gameObject, 1f);
    }
}
