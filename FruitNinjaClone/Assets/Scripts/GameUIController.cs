using System.Collections;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _readyText;
    [SerializeField] GameObject _goText;
    private void Awake()
    {
        _gameOverPanel.SetActive(false);
        _readyText.SetActive(false);
        _goText.SetActive(false);
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;

        GameManager.Instance.OnGameOver += HandleOnGameOver;
    }
    void HandleOnGameOver()
    {
        _gameOverPanel.SetActive(true);
 
    }
    public void RestartButtonClick()
    {
        GameManager.Instance.Restart();
    }
    public void StartCountDown(float delay)
    {
        StartCoroutine(ReadyGoCountdown(delay));
    }
    IEnumerator ReadyGoCountdown(float delay)
    {
        WaitForSeconds waitTime = new WaitForSeconds(delay/2);
        _readyText.SetActive(true);
        yield return waitTime;
        _readyText.SetActive(false);
        _goText.SetActive(true);
        yield return waitTime;
        _goText.SetActive(false);
        yield return null;
    }

}
