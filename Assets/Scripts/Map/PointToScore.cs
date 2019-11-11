using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToScore : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Prince prince = other.gameObject.GetComponent<Prince>();
        if (prince)
        {
            ScoreManager.Instance.NotifyPrinceSaved();
            prince.NotifySaved();
        }
    }
}
