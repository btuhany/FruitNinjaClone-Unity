using TMPro;
using UnityEngine;

public class HeartPanelUpdater : MonoBehaviour
{
    TextMeshProUGUI[] _text;
    private void Awake()
    {
        _text = GetComponentsInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        GameManager.Instance.OnLivesChanged += HandleOnLiveChange;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnLivesChanged -= HandleOnLiveChange;
    }
    void HandleOnLiveChange()
    {
        int CurrentLive = GameManager.Instance.Lives;
        switch (CurrentLive)
        {
            case 3:
                _text[1].SetText("");
                break;
            case 2:
                _text[1].SetText("X");
                break;
            case 1:
                _text[1].SetText("XX");
                break;
            case 0:
                _text[1].SetText("XXX");
                break;
            default:
                break;
        }

    }
}
