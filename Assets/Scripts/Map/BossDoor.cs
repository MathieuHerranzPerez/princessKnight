using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Animator animator = default;

    public void Close()
    {
        animator.SetTrigger("Close");
    }

    public void Open()
    {
        animator.SetTrigger("Open");

        Destroy(gameObject, 2f);
    }
}
