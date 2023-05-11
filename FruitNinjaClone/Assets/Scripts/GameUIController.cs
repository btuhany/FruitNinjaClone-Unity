using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _readyText;
    [SerializeField] GameObject _goText;
    [SerializeField] Image _bombEffectImage;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
        _readyText.SetActive(false);
        _goText.SetActive(false);
        _bombEffectImage.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;

        GameManager.Instance.OnGameOver += HandleOnGameOver;
        GameManager.Instance.OnGameStart += HandleOnGameStart;
    }
    void HandleOnGameOver()
    {

        StartCoroutine(BombEffect());
 
    }
    void HandleOnGameStart()
    {
        //_gameOverPanel.SetActive(false);
        //_readyText.SetActive(false);
        //_goText.SetActive(false);
        //_bombEffectImage.gameObject.SetActive(false);
           
    }
    public void RestartButtonClick()
    {
        _readyText.SetActive(false);
        _gameOverPanel.SetActive(false);
        _goText.SetActive(false);
        _bombEffectImage.gameObject.SetActive(false);
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
    IEnumerator BombEffect()
    {
        float elapsed = 0f;
        float duration = 1.5f;
        _bombEffectImage.gameObject.SetActive(true);
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            _bombEffectImage.color = Color.Lerp(Color.clear,Color.white, t);
            Time.timeScale = 1f - t;

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        
        _gameOverPanel.SetActive(true);
        duration = 0.7f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            _bombEffectImage.color = Color.Lerp(Color.white, Color.clear, t);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        _bombEffectImage.gameObject.SetActive(false);
        yield return null;
    }
}
