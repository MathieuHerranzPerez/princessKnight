using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    [SerializeField]
    private Transform bottomLeftPoint = default;
    [SerializeField]
    private Transform topRighPoint = default;
    [SerializeField]
    private Transform target = default;


    void LateUpdate()
    {
        Vector3 newPos = target.position;
        bool hasPosChanged = false;
        if(target.position.x < bottomLeftPoint.position.x)
        {
            newPos.x = bottomLeftPoint.position.x;
            hasPosChanged = true;
        }
        if(target.position.x > topRighPoint.position.x)
        {
            newPos.x = topRighPoint.position.x;
            hasPosChanged = true;
        }

        if (target.position.z < bottomLeftPoint.position.z)
        {
            newPos.z = bottomLeftPoint.position.z;
            hasPosChanged = true;
        }
        if (target.position.z > topRighPoint.position.z)
        {
            newPos.z = topRighPoint.position.z;
            hasPosChanged = true;
        }

        if(hasPosChanged)
        {
            target.transform.position = newPos;
        }
    }
}
