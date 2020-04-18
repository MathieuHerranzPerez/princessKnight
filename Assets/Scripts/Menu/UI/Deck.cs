using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> listCard = new List<Card>();
    private Card[] selectedCards = new Card[3];

    private int nbCardSelected = 0;

    // to notify the UI if changed
    private List<Observer> listObserver = new List<Observer>();

    /* ----- GETTERS ----- */
    public Card[] GetSelectedCards()
    {
        return selectedCards;
    }

    public List<Card> GetListCard()
    {
        return listCard;
    }
    /* ----- ------- ----- */
    /* ----- SETTERS ----- */

    public void SetListCardFound(List<Card> listCard) // (not selected)
    {
        this.listCard = listCard;
        NotifyObservers();
    }
    public void SetSelectedCards(Card[] selectedCard)
    {
        this.selectedCards = selectedCard;
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
        listCard.Add(card);
        //Debug.Log("added : " + card); // affD
    }

    public void SelectCard(Card card)
    {
        if (!isSelectedCardListFull())
        {
            bool isPlaced = false;
            int i = 0;
            while (!isPlaced)
            {
                if (!selectedCards[i])
                {
                    selectedCards[i] = card;
                    isPlaced = true;
                }
                ++i;
            }
            listCard.Remove(card);
            ++nbCardSelected;
        }
    }

    public void SelectCardInPos(Card card, int i)
    {
        if (!isSelectedCardListFull() && i >= 0 && i < selectedCards.Length && selectedCards[i] == null)
        {
            selectedCards[i] = card;
            listCard.Remove(card);
            ++nbCardSelected;
        }
    }

    public void SelectCardIfNotSelected(Card card, int i)
    {
        if (listCard.Contains(card))
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
        int ind = 0;
        bool found = false;
        while (!found && ind < selectedCards.Length)
        {
            if (selectedCards[ind] == card)
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
            Card tmpCard = selectedCards[i];
            selectedCards[i] = card;
            selectedCards[ind] = tmpCard;
        }
    }

    public void DeselectCard(Card card)
    {
        // find it in the list
        int indice = -1;
        int i = 0;
        while (i < selectedCards.Length && indice < 0)
        {
            if (selectedCards[i] == card)
            {
                indice = i;
            }
        }

        DeselectCard(indice);
    }

    public void DeselectCard(int i)
    {
        // place it in the deck
        listCard.Add(selectedCards[i]);
        // remove it from the selected cards
        selectedCards[i] = null;
        --nbCardSelected;
    }

    public Card GetCardFromSelected(int i)
    {
        return selectedCards[i];
    }

    private bool isSelectedCardListFull()
    {
        return nbCardSelected == selectedCards.Length;
    }

    private bool isSelectedCardListEmpty()
    {
        return nbCardSelected == 0;
    }

    public void DisplayDeckSelectedInConsole() // affD
    {
        string str = "";
        for (int i = 0; i < selectedCards.Length; ++i)
        {
            str += selectedCards[i] + "; ";
        }
        Debug.Log("Selected : [ " + str + " ]");

        str = "";
        for (int i = 0; i < listCard.Count; ++i)
        {
            str += listCard[i] + ";";
        }
        Debug.Log("Deck : [ " + str + " ]");
    }

    public int GetSelectedCardLength()
    {
        return selectedCards.Length;
    }
}
