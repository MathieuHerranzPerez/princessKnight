using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private SceneFader sceneFader = default;

    [SerializeField] private string gameSceneName = "Mathieu";


    void Start()
    {
        Instance = this;
        InitData();
    }

    public void InitData()
    {
        // load all the cards saved
        MasterDeck.Instance.LoadData();

        DataBetweenScene.isInitiated = true;
    }

    public void StartGame()
    {
        // --- store data in DataBetweenScene

        // -- store cards
        // -- store cards
        // - card selected
        List<Card> listSelectedCards = new List<Card>();
        listSelectedCards.AddRange(MasterDeck.Instance.GetDeck().GetSelectedCards());
        List<Card> listAllCards = MasterDeck.Instance.GetListAllCards();
        DataBetweenScene.listIndexCardSelected = new List<int>();         // clear the list
        for (int i = 0; i < listAllCards.Count; ++i)
        {
            if (listSelectedCards.Contains(listAllCards[i]))
            {
                DataBetweenScene.listIndexCardSelected.Add(i);
            }
        }

        // save selected card in file
        SaveSystem.SaveDeck(MasterDeck.Instance.GetDeck(), MasterDeck.Instance);

        // load scene
        sceneFader.FadeTo(gameSceneName);
    }
}
