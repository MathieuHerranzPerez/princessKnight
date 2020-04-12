using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlotUI : SlotUser
{
    public GameObject cardGO
    {
        get
        {
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        // if the slot is empty
        if (!cardGO)
        {
            if (DraggableUIObject.itemBeingDragged != null)
            {
                DraggableUIObject.itemBeingDragged.transform.SetParent(transform);

                // place it at the same deep
                FitObjectWell(DraggableUIObject.itemBeingDragged);

                NotifyDeckUI();
            }
        }
        else
        {/*
            GameObject itemgo = cardGO;
            // swap the items
            itemgo.transform.SetParent(DragHandlerUI.itemBeingDragged.GetComponent<DragHandlerUI>().startParent);
            itemgo.transform.position = DragHandlerUI.itemBeingDragged.GetComponent<DragHandlerUI>().startPosition;
            DragHandlerUI.itemBeingDragged.transform.SetParent(transform);*/
        }
    }

    public override void NotifyDeckUI()
    {
        if (deckUI != null)
        {
            deckUI.NotifyFromCardSlot();
        }
    }
}
