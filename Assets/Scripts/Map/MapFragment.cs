using System.Collections.Generic;
using UnityEngine;

public class MapFragment : MonoBehaviour
{
    public void Destroy()
    {
        // TODO remove all content


        Destroy(transform.gameObject);
    }
}