using TMPro;
using UnityEngine;

public class BestScoreUpdater : MonoBehaviour
{
    
    TextMeshProUGUI _text;
    Color _defaultColour;
    private void Awake()
    {
        _text= GetComponent<TextMeshProUGUI>();
        _defaultColour = _text.color;
    }
    private void OnEnable()
    {
        GameManager.Instance.OnBestScoreChanged += HandleOnBestScoreChange;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnBestScoreChanged -= HandleOnBestScoreChange;
    }
    void HandleOnBestScoreChange()
    {
        if(GameManager.Instance.LastBestScore != GameManager.Instance.BestScore)
        {
            _text.color = Color.green;
        }
        else
        {
            _text.color= _defaultColour;
        }
        _text.SetText("Best: " + GameManager.Instance.BestScore);
    }
}
