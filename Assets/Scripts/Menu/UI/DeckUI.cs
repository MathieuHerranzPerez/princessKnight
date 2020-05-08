using UnityEngine;

public class DeckUI : MonoBehaviour, Observer
{
    private CardSlotUI[] slotList;  // list of the slots of selected cards

    [SerializeField] private GameObject selectedCardsGOUI = default;


    [SerializeField] private DeckSlotUI cardsListGOUI = default;

    private MenuWeaponManager menuWeaponManager;
    private Deck deck;

    void Start()
    {
        deck = MasterDeck.Instance.GetDeck();
        menuWeaponManager = MenuWeaponManager.Instance;
        Init();
    }

    private void Init()
    {
        slotList = new CardSlotUI[deck.GetSelectedCardLength()];
        cardsListGOUI.SetDeckUI(this);
        deck.AddObserver(this);
        Notify();
    }

    public void NotifyFromCardSlot(GameObject go)
    {
        menuWeaponManager.GiveNewWeapon(go.GetComponent<Card>().WeaponPrefab);

        // for all the slots, add the card if not already added
        for (int i = 0; i < slotList.Length; ++i)
        {
            if (slotList[i].cardGO)
            {
                deck.SelectCardIfNotSelected(slotList[i].cardGO.GetComponent<Card>(), i);
            }
        }

        deck.DisplayDeckSelectedInConsole(); //affD
    }

    public void NotifyFromCardContainer(GameObject go)
    {
        menuWeaponManager.RemoveWeapon(go.GetComponent<Card>().WeaponPrefab);

        // for all the slots, add the card if not already added or remove it if null
        for (int i = 0; i < slotList.Length; ++i)
        {
            if (slotList[i].cardGO)
            {
                deck.SelectCardIfNotSelected(slotList[i].cardGO.GetComponent<Card>(), i);
            }
            else
            {
                deck.DeselectCard(i);
            }
        }
        deck.DisplayDeckSelectedInConsole(); //affD
    }

    public void NotifyClicOnCardFromSlot(GameObject go)
    {
        // put it in the container
        go.transform.SetParent(cardsListGOUI.transform);
        // change the clic behaviour
        go.GetComponent<CardUI>().ChangeListener(delegate { NotifyClicOnCardFromContainer(go); });

        menuWeaponManager.RemoveWeapon(go.GetComponent<Card>().WeaponPrefab);

        // for all the slots, add the card if not already added or remove it if null
        for (int i = 0; i < slotList.Length; ++i)
        {
            if (slotList[i].cardGO)
            {
                deck.SelectCardIfNotSelected(slotList[i].cardGO.GetComponent<Card>(), i);
            }
            else
            {
                deck.DeselectCard(i);
            }
        }

        deck.DisplayDeckSelectedInConsole(); //affD
    }

    public void NotifyClicOnCardFromContainer(GameObject go)
    {
        // check if there is space in selected
        bool isFull = true;
        int i = 0;
        while(i < selectedCardsGOUI.transform.childCount && isFull)
        {
            if(selectedCardsGOUI.transform.GetChild(i).childCount == 0)
                isFull = false;
            else
                ++i;
        }

        if (!isFull)
        {
            // put it in the slot
            go.transform.SetParent(selectedCardsGOUI.transform.GetChild(i).transform);
            // change the clic behaviour
            go.GetComponent<CardUI>().ChangeListener(delegate { NotifyClicOnCardFromSlot(go); });

            menuWeaponManager.GiveNewWeapon(go.GetComponent<Card>().WeaponPrefab);

            // for all the slots, add the card if not already added
            for (int j = 0; j < slotList.Length; ++j)
            {
                if (slotList[j].cardGO)
                {
                    deck.SelectCardIfNotSelected(slotList[j].cardGO.GetComponent<Card>(), j);
                }
            }

            deck.DisplayDeckSelectedInConsole(); //affD
        }

    }

    public void Notify()
    {
        Clear();
        for (int i = 0; i < selectedCardsGOUI.transform.childCount; ++i)
        {
            CardSlotUI cardSlot = selectedCardsGOUI.transform.GetChild(i).GetComponent<CardSlotUI>();
            slotList[i] = cardSlot;
            slotList[i].SetDeckUI(this);

            Card card = deck.GetCardFromSelected(i);
            if (card)
            {
                Card cardClone = (Card)Instantiate(card, cardSlot.transform);  // instantiate the card in the UI
                cardClone.gameObject.AddComponent(typeof(CardUI));
                CardUI cardUI = cardClone.GetComponent<CardUI>();
                cardUI.ChangeListener(delegate { NotifyClicOnCardFromSlot(cardClone.gameObject); });

                cardClone.gameObject.AddComponent(typeof(DraggableUIObject));
                cardClone.gameObject.AddComponent(typeof(CanvasGroup));
            }
        }

        int j = 0;
        foreach (Card c in deck.GetListCard())
        {
            ++j;
            Card cardClone = (Card)Instantiate(c, cardsListGOUI.transform);  // instantiate the card in the UI
            cardClone.gameObject.AddComponent(typeof(CardUI));
            CardUI cardUI = cardClone.GetComponent<CardUI>();
            cardUI.ChangeListener(delegate { NotifyClicOnCardFromContainer(cardClone.gameObject); });
            cardClone.gameObject.AddComponent(typeof(DraggableUIObject));
            cardClone.gameObject.AddComponent(typeof(CanvasGroup));
        }
    }

    private void Clear()
    {
        // destroy the card GO (if exists)
        for (int i = 0; i < selectedCardsGOUI.transform.childCount; ++i)
        {
            foreach (Transform child in selectedCardsGOUI.transform.GetChild(i))
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (Transform child in cardsListGOUI.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}