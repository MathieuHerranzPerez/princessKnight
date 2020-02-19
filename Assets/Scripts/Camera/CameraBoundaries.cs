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
        Vector3 targetCurrentPos = target.position;
        Vector3 newPos = targetCurrentPos;
        bool hasPosChanged = false;
        if(targetCurrentPos.x < bottomLeftPoint.position.x)
        {
            newPos.x = bottomLeftPoint.position.x;
            hasPosChanged = true;
        }
        if(targetCurrentPos.x > topRighPoint.position.x)
        {
            newPos.x = topRighPoint.position.x;
            hasPosChanged = true;
        }

        if (targetCurrentPos.z < bottomLeftPoint.position.z)
        {
            newPos.z = bottomLeftPoint.position.z;
            hasPosChanged = true;
        }
        if (targetCurrentPos.z > topRighPoint.position.z)
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
