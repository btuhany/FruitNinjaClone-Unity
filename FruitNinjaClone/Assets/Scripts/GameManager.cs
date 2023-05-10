using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameUIController _ui;
    [SerializeField] float _startDelay;
    public static GameManager Instance;
    public event System.Action OnBestScoreChanged;
    public event System.Action OnScoreChanged;
    public event System.Action OnLivesChanged;
    public event System.Action OnGameOver;

    public bool IsGameOn;
    [HideInInspector] public int CurrentScore=0;
    [HideInInspector] public int BestScore=0;
    [HideInInspector] public int Lives = 3;
    [HideInInspector] public int LastBestScore = 0;
    
    private void Awake()
    {
        Instance= this;
        
    }
    private void Start()
    {
        InvokeUIEvents();
        BladeController.Instance.CanReadInput = false;
        IsGameOn = false;
        StartCoroutine(StartGameWithCountdownDelay(_startDelay));
    }
    public void Restart()
    {
        Debug.Log("Restart");
    }
    public void StartTheGame()
    {
        IsGameOn = true;
        BladeController.Instance.CanReadInput = true;
    }
    public void GameOver()
    {
        if(BestScore>=LastBestScore)
        {
            LastBestScore=BestScore;
            OnBestScoreChanged?.Invoke();
        }
        BladeController.Instance.CanReadInput = false;
        Time.timeScale = 0f;
        
        OnGameOver?.Invoke();
    }
    public void IncreaseScore()
    {
        CurrentScore++;
        OnScoreChanged?.Invoke();
        if(CurrentScore > LastBestScore) 
        {
            BestScore = CurrentScore;
            OnBestScoreChanged?.Invoke();
        }
    }
    public void DecreaseLive()
    {
        Lives--;
        OnLivesChanged?.Invoke(); 
        if(Lives <= 0) 
            GameOver();
    }
    void InvokeUIEvents()
    {
        OnScoreChanged?.Invoke();
        OnBestScoreChanged?.Invoke();
        OnLivesChanged?.Invoke();
    }
    IEnumerator StartGameWithCountdownDelay(float delay)
    {
        _ui.StartCountDown(delay);
        yield return new WaitForSeconds(delay);
        StartTheGame();
        yield return null;
    }
}
