using UnityEngine;
using UnityEngine.EventSystems;

public class DeckSlotUI : SlotUser
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (DraggableUIObject.itemBeingDragged == null)
            return;

        DraggableUIObject.itemBeingDragged.transform.SetParent(transform);

        // place it at the same deep
        FitObjectWell(DraggableUIObject.itemBeingDragged);

        NotifyDeckUI(DraggableUIObject.itemBeingDragged);
    }

    public override void NotifyDeckUI(GameObject go)
    {
        if (deckUI != null)
        {
            deckUI.NotifyFromCardContainer(go);
        }
    }
}
