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

        NotifyDeckUI();
    }

    public override void NotifyDeckUI()
    {
        if (deckUI != null)
        {
            deckUI.NotifyFromCardContainer();
        }
    }
}
