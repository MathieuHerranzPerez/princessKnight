using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ScreenType
{
    HomeScreen,
    AchievementScreen,
    StatisticsScreen
}

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }

    [SerializeField] private Camera cam = default;

    [Header("Screens")]
    [SerializeField] private int screenLayerIndex = 0;
    [SerializeField] private ScreenType firstScreenType = default;
    [SerializeField] private bool initWithFirstScreen = true;
    [SerializeField] private List<BaseScreen> listScreen = new List<BaseScreen>();
    [SerializeField] private Transform screenContainer = default;

    [Header("Popups")]
    [SerializeField] private int popupsLayerIndex = 100;
    [SerializeField] private List<Popup> listPopup = new List<Popup>();
    [SerializeField] private Transform popupContainer = default;


    public event Action OnScreenChanged;

    // ---- INTERN ----
    private BaseScreen currentScreen;
    private List<Popup> listCurrentPopup = new List<Popup>();

    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("more than 1 ScreenManager in the scene");
        }
        Instance = this;
    }

    void Start()
    {
        if(initWithFirstScreen)
            ShowScreen(firstScreenType);
    }

    public void ShowScreen(string type)
    {
        Enum.TryParse(type, out ScreenType screenType);
        ShowScreen(screenType);
    }

    public BaseScreen ShowScreen(ScreenType screenType, Hashtable parameters = null)
    {
        if(currentScreen != null)
        {
            currentScreen.Exit();
        }

        BaseScreen newScreen = CreateScreen(screenType);
        if(newScreen != null)
        {
            newScreen.SetParams(parameters);
            currentScreen = newScreen;

            NotifyScreenChanged();
        }

        return newScreen;
    }

    public T CreatePopup<T>() where T : Popup
    {
        T popupPrefab = (T)GetPopupPrefab(typeof(T));
        if (popupPrefab != null)
        {
            T popup = Instantiate(popupPrefab, popupContainer, true);
            popup.GetComponent<Canvas>().worldCamera = cam;
            SetSortingOrderPopup(popup);
            listCurrentPopup.Add(popup);
            popup.OnOpened += OnPopupOpened;
            popup.OnClosed += OnPopupClosed;

            return popup;
        }

        return null;
    }

    private BaseScreen CreateScreen(ScreenType screenType)
    {
        BaseScreen baseScreen = listScreen.Find(screen => screen.Type == screenType);
        if(baseScreen != null)
        {
            BaseScreen screen = Instantiate(baseScreen, screenContainer, true);
            Canvas screenCanvas = screen.GetComponent<Canvas>();
            screenCanvas.worldCamera = cam;
            screenCanvas.sortingLayerID = screenLayerIndex;
            return screen;
        }

        return null;
    }

    private void NotifyScreenChanged()
    {
        OnScreenChanged?.Invoke();
    }

    private Popup GetPopupPrefab(Type type)
    {
        return listPopup.Find(popup => popup.GetType() == type);
    }

    private void OnPopupOpened(Popup popup)
    {
        if (!listCurrentPopup.Contains(popup))
        {
            SetSortingOrderPopup(popup);
            listCurrentPopup.Add(popup);
        }
    }

    private void SetSortingOrderPopup(Popup popup)
    {
        if (listCurrentPopup.Count == 0)
            popup.GetComponent<Canvas>().sortingOrder = popupsLayerIndex;
        else
            popup.GetComponent<Canvas>().sortingOrder = listCurrentPopup.Max(p => p.GetComponent<Canvas>().sortingOrder) + 1;
    }

    private void OnPopupClosed(Popup popup)
    {
        if (!popup.DontDestroyOnClose && listCurrentPopup.Contains(popup))
        {
            listCurrentPopup.Remove(popup);
        }
    }
}
