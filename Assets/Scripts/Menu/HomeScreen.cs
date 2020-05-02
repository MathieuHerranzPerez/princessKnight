using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : BaseScreen
{
    [SerializeField] private Button playButton = default;

    protected override void OnAwake()
    {
        playButton.onClick.AddListener(delegate () { MenuManager.Instance.StartGame(); });
    }
}
