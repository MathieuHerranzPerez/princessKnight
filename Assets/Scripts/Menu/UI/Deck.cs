using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    //private List<Card> listCard = new List<Card>();
    //private Card[] selectedCards = new Card[3];
    private List<int> listCardIndex = new List<int>();
    private int[] selectedCardIndexes = new int[3] { -1, -1, -1 };

    private int nbCardSelected = 0;

    // to notify the UI if changed
    private List<Observer> listObserver = new List<Observer>();

    /* ----- GETTERS ----- */
    public Card[] GetSelectedCards()
    {
        Card[] selectedCards = new Card[3];
        int i = 0;
        foreach(int index in selectedCardIndexes)
        {
            if(index >= 0)
            {
                selectedCards[i] = MasterDeck.Instance.GetListAllCards()[index];
                ++i;
            }
        }

        return selectedCards;
    }

    public List<Card> GetListCard()
    {
        List<Card> listCard = new List<Card>();

        foreach(int index in listCardIndex)
        {
            if (index >= 0)
            {
                listCard.Add(MasterDeck.Instance.GetListAllCards()[index]);
            }
        }

        return listCard;
    }
    /* ----- ------- ----- */
    /* ----- SETTERS ----- */

    public void SetListCardFound(List<Card> listCard) // (not selected)
    {
        // this.listCard = listCard;

        listCardIndex = new List<int>();
        foreach (Card c in listCard)
        {
            listCardIndex.Add(MasterDeck.Instance.GetCardIndexByTitle(c.GetTitle()));
        }
        NotifyObservers();
    }
    public void SetSelectedCards(Card[] selectedCard)
    {
        //this.selectedCards = selectedCard;
        int i = 0;
        foreach (Card c in selectedCard)
        {
            if (c)
            {
                selectedCardIndexes[i] = MasterDeck.Instance.GetCardIndexByTitle(c.GetTitle());
            }
        }
        NotifyObservers();
    }
    /* ----- ------- ----- */

    private void NotifyObservers()
    {
        foreach (Observer obs in listObserver)
        {
            obs.Notify();
        }
    }

    public void AddObserver(Observer obs)
    {
        listObserver.Add(obs);
    }


    public void AddDeckCard(Card card)
    {
        //listCard.Add(card);
        listCardIndex.Add(MasterDeck.Instance.GetCardIndexByTitle(card.GetTitle()));
        //Debug.Log("added : " + card); // affD
    }

    public void SelectCard(Card card)
    {
        int cardIndex = MasterDeck.Instance.GetCardIndexByTitle(card.GetTitle());
        if (!isSelectedCardListFull())
        {
            bool isPlaced = false;
            int i = 0;
            while (!isPlaced)
            {
                if (selectedCardIndexes[i] == -1)
                {
                    selectedCardIndexes[i] = cardIndex;
                    isPlaced = true;
                }
                ++i;
            }
            listCardIndex.Remove(cardIndex);
            ++nbCardSelected;
        }
    }

    public void SelectCardInPos(Card card, int i)
    {
        if (!isSelectedCardListFull() && i >= 0 && i < selectedCardIndexes.Length && selectedCardIndexes[i] == -1)
        {
            int cardIndex = MasterDeck.Instance.GetCardIndexByTitle(card.GetTitle());
            selectedCardIndexes[i] = cardIndex;
            listCardIndex.Remove(cardIndex);
            ++nbCardSelected;
        }
    }

    public void SelectCardIfNotSelected(Card card, int i)
    {
        int cardIndex = MasterDeck.Instance.GetCardIndexByTitle(card.GetTitle());
        if (listCardIndex.Contains(cardIndex))
        {
            SelectCardInPos(card, i);
        }
        else
        {
            ChangeCardPlace(card, i);
        }
    }

    public void ChangeCardPlace(Card card, int i)
    {
        int cardIndex = MasterDeck.Instance.GetCardIndexByTitle(card.GetTitle());
        int ind = 0;
        bool found = false;
        while (!found && ind < selectedCardIndexes.Length)
        {
            if (selectedCardIndexes[ind] == cardIndex)
            {
                found = true;
            }
            else
            {
                ++ind;
            }
        }

        if (found)
        {
            // swap
            int tmpIndexCard = selectedCardIndexes[i];
            selectedCardIndexes[i] = cardIndex;
            selectedCardIndexes[ind] = tmpIndexCard;
        }
    }

    public void DeselectCard(Card card)
    {
        // find it in the list
        int indice = -1;
        int i = 0;
        while (i < selectedCardIndexes.Length && indice < 0)
        {
            if (selectedCardIndexes[i] == MasterDeck.Instance.GetCardIndexByTitle(card.GetTitle()))
            {
                indice = i;
            }
        }

        DeselectCard(indice);
    }

    public void DeselectCard(int i)
    {
        if (selectedCardIndexes[i] != -1 && !listCardIndex.Contains(selectedCardIndexes[i]))
        {
            // place it in the deck
            listCardIndex.Add(selectedCardIndexes[i]);
            // remove it from the selected cards
            selectedCardIndexes[i] = -1;
            --nbCardSelected;
        }
    }

    public Card GetCardFromSelected(int i)
    {
        
        Card card = selectedCardIndexes[i] > -1 ? MasterDeck.Instance.GetListAllCards()[selectedCardIndexes[i]] : null;
        return card;
    }

    private bool isSelectedCardListFull()
    {
        return nbCardSelected == selectedCardIndexes.Length;
    }

    private bool isSelectedCardListEmpty()
    {
        return nbCardSelected == 0;
    }

    public void DisplayDeckSelectedInConsole() // affD
    {
        string str = "";
        for (int i = 0; i < selectedCardIndexes.Length; ++i)
        {
            str += selectedCardIndexes[i] + "; ";
        }
        Debug.Log("Selected : [ " + str + " ]");

        str = "";
        for (int i = 0; i < listCardIndex.Count; ++i)
        {
            str += listCardIndex[i] + ";";
        }
        Debug.Log("Deck : [ " + str + " ]");
    }

    public int GetSelectedCardLength()
    {
        return selectedCardIndexes.Length;
    }
}
