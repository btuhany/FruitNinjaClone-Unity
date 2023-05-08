using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        GameManager.Instance.OnScoreChanged += HandleOnScoreChange;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnScoreChanged -= HandleOnScoreChange;
    }
    void HandleOnScoreChange()
    {
        _text.SetText(GameManager.Instance.CurrentScore.ToString());
    }
}
