using System;
using System.Collections.Generic;
using UnityEngine;

public class MasterDeck : MonoBehaviour
{
    public static MasterDeck Instance { get; private set; }

    [SerializeField] private CardCollection cardCollection = default;

    [SerializeField] private Deck deck = default;


    private List<int> listIndexCardNotFound = new List<int>();

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("More than 1 MasterDeck in the scene");
            Destroy(gameObject);
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public List<Card> GetListAllCards()
    {
        return cardCollection.listAllCard;
    }

    public List<int> GetListIndexCardNotFound()
    {
        return listIndexCardNotFound;
    }

    public void SetListIndexCardNotFound(List<int> listIndexCardNotFound)
    {
        this.listIndexCardNotFound = listIndexCardNotFound;
    }

    public void SaveData()
    {
        // to add all card if the deck (temporary)
        //foreach (Card c in cardCollection.listAllCard)
        //{
        //    if (!deck.GetListCard().Contains(c) && !FindCardInArray(c, deck.GetSelectedCards()))
        //    {
        //        deck.AddDeckCard(c);
        //    }
        //}

        SaveSystem.SaveDeck(deck, this);
    }
    private bool FindCardInArray(Card c, Card[] listCard)
    {
        int i = 0;
        bool found = false;
        while (i < listCard.Length && !found)
        {
            if (listCard[i] == c)
                found = true;
            ++i;
        }
        return found;
    }

    public void LoadData()
    {
        SaveDeckData saveDeckData = SaveSystem.LoadDeck();

        // card found
        List<Card> listCardFound = new List<Card>();
        if (saveDeckData != null)
        {
            foreach (int i in saveDeckData.listIndexCardFound)
            {
                listCardFound.Add(cardCollection.listAllCard[i]);
            }

            // index card not found
            for (int i = 0; i < cardCollection.listAllCard.Count; ++i)
            {
                if (!saveDeckData.listIndexCardFound.Contains(i) && !saveDeckData.listIndexCardSelected.Contains(i))
                {
                    listIndexCardNotFound.Add(i);
                }
            }
        }
        else
        {
            // add all the cards to the list
            for (int i = 0; i < cardCollection.listAllCard.Count; ++i)
            {
                listIndexCardNotFound.Add(i);
            }
        }

        // card selected
        //Card[] tabCardSelected = new Card[8];
        Card[] tabCardSelected = new Card[deck.GetSelectedCardLength()];
        if (saveDeckData != null)
        {
            int ind = 0;
            foreach (int i in saveDeckData.listIndexCardSelected)
            {
                tabCardSelected[ind] = cardCollection.listAllCard[i];
                ++ind;
            }
        }

        deck.SetListCardFound(listCardFound);
        deck.SetSelectedCards(tabCardSelected);
    }

    public Deck GetDeck()
    {
        return this.deck;
    }

    public void AddCardJustFoundToDeck(Card card)
    {
        // remove the card from listIndexCardNotFound
        for (int i = 0; i < cardCollection.listAllCard.Count; ++i)
        {
            if (cardCollection.listAllCard[i] == card)
            {
                listIndexCardNotFound.Remove(i);
            }

        }

        // add it to the deck
        deck.AddDeckCard(card);
        SaveData(); // todo new
    }

    public bool HasCardNotFound()
    {
        return listIndexCardNotFound.Count > 0;
    }

    public Card GetCardFromAllCardsWithIndex(int index)
    {
        return cardCollection.listAllCard[index];
    }
}
