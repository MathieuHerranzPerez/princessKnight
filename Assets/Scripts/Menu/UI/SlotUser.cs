using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SlotUser : MonoBehaviour, IDropHandler
{
    public abstract void OnDrop(PointerEventData eventData);

    protected void FitObjectWell(GameObject go)
    {
        Vector3 itemDraggedPos = DraggableUIObject.itemBeingDragged.transform.position;
        itemDraggedPos.y = transform.position.y;
        DraggableUIObject.itemBeingDragged.transform.position = itemDraggedPos;
    }

    protected DeckUI deckUI;

    public void SetDeckUI(DeckUI deck)
    {
        deckUI = deck;
    }

    public abstract void NotifyDeckUI(GameObject go);
}
