using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddableUnit2 : MonoBehaviour
{
    private static int lastId = 0;

    public int Id { get; private set; }

    [SerializeField]
    private Transform skinToHide = default;

    // ---- INTERN ----
    private LayerMask firstLayer;
    private bool isVisible = true;
    private bool newIsVisible = true;

    void Awake()
    {
        firstLayer = gameObject.layer;
        Id = lastId;
        ++lastId;
    }

    void Update()
    {
        if (isVisible != newIsVisible)
        {
            if (newIsVisible)
            {
                RevelUnit();
            }
            else
            {
                HideUnit();
            }
        }

        isVisible = newIsVisible;
        if (!isVisible)
        {
            newIsVisible = true;
        }
    }

    public void Hide()
    {
        newIsVisible = false;
    }

    public void Revel()
    {
        newIsVisible = true;
    }

    private void HideUnit()
    {
        GameObjectExtension.SetLayerRecursively(skinToHide.gameObject, LayerMask.NameToLayer("Invisible"));
    }

    private void RevelUnit()
    {
        GameObjectExtension.SetLayerRecursively(skinToHide.gameObject, firstLayer);
    }
}
