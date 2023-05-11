using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] GameObject _readyText;
    [SerializeField] GameObject _goText;
    [SerializeField] Image _bombEffectImage;
    [SerializeField] TextMeshProUGUI _comboText;

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
        BladeController.Instance.OnComboEvent+= HandleOnComboEvent;
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
    void HandleOnComboEvent()
    {
        _comboText.SetText("+" + BladeController.Instance.ComboCounter.ToString() + " FRUIT COMBO");
        _comboText.transform.position = BladeController.Instance.TransformPos + Vector3.up * 0.5f;
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        float elapsed = 0f;
        float duration = 3f;
        _comboText.gameObject.SetActive(true);
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            _comboText.color = Color.Lerp(Color.white, Color.clear, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _comboText.gameObject.SetActive(false);
        yield return null;
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
