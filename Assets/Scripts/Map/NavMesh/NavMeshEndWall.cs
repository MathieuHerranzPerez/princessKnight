using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NavMeshEndWall : MonoBehaviour
{
    // ---- INTERN ----
    private string[] listTag;

    void Start()
    {
        listTag = NavMeshManager.Instance.ListTag;
        GetComponent<Collider>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        bool hasOtherARightTag = false;
        foreach (string tag in listTag)
        {
            if (other.gameObject.tag == tag)
            {
                hasOtherARightTag = true;
            }
        }

        if (hasOtherARightTag)
        {
            INavMeshUnit agent = other.GetComponent<INavMeshUnit>();
            if (agent != null)
            {
                agent.LeaveNavMesh();
            }
        }
    }
}
