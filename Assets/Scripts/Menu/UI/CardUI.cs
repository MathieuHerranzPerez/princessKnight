using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CardUI : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void ChangeListener(Action<GameObject> func)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { func(gameObject); });
    }
}
