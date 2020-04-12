using System;
using System.Collections.Generic;

[Serializable]
public class SaveDeckData
{
    // Card / Deck
    public List<int> listIndexCardFound = new List<int>();      // found but not selected
    public List<int> listIndexCardSelected = new List<int>();   // selected but not in found list

    public SaveDeckData(Deck deck, MasterDeck masterDeck)
    {
        List<Card> listCardFound = deck.GetListCard();     // found but not selected
        Card[] tabSelectedCards = deck.GetSelectedCards(); // selected but not in found list
        List<Card> listSelectedCards = new List<Card>();
        List<Card> listAllCards = masterDeck.GetListAllCards();

        listSelectedCards.AddRange(tabSelectedCards);

        for (int i = 0; i < listAllCards.Count; ++i)
        {
            if (listCardFound.Contains(listAllCards[i]))
            {
                listIndexCardFound.Add(i);
            }
            else if (listSelectedCards.Contains(listAllCards[i]))
            {
                listIndexCardSelected.Add(i);
            }
        }
    }
}
