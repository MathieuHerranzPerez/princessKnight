using UnityEngine;

public class HiddableUnit : MonoBehaviour
{
    private static int lastId = 0;

    public int Id { get; private set; }

    [SerializeField]
    private Transform skinToHide = default;

    // ---- INTERN ----
    private LayerMask firstLayer;
    private bool isVisible = true;

    void Awake()
    {
        firstLayer = gameObject.layer;
        Id = lastId;
        ++lastId;
    }

    void Update()
    {
        if (!isVisible)
        {
            Revel();
        }
    }

    public void Hide()
    {
        isVisible = false;
        GameObjectExtension.SetLayerRecursively(skinToHide.gameObject, LayerMask.NameToLayer("Invisible"));
    }

    public void Revel()
    {
        isVisible = true;
        GameObjectExtension.SetLayerRecursively(skinToHide.gameObject, firstLayer);
    }
}
