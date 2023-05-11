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
    public event System.Action OnGameStart;

    bool _isGameOn;
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
        
        //   BladeController.Instance.CanReadInput = false;
        GameSetupAndStart();
    }
    public void Restart()
    {
        // BladeController.Instance.CanReadInput = false;
        GameSetupAndStart();
    }
    void GameSetupAndStart()
    {
        CurrentScore = 0;
        Lives = 3;
        InvokeUIEvents();
        SoundManager.Instance.PlaySound(4);
        SoundManager.Instance.PlaySoundDelayed(0);
        StartCoroutine(StartGameWithCountdownDelay(_startDelay));
    }
    public void StartTheGame()
    {
        OnGameStart?.Invoke();
        _isGameOn = true;
        
       // BladeController.Instance.CanReadInput = true; //Obsrv pattern? event
    }
    public void GameOver()
    {
        if (!_isGameOn) return;
        _isGameOn = false;
        OnGameOver?.Invoke();
        SoundManager.Instance.StopAllSounds();
        SoundManager.Instance.PlaySoundDelayed(6);
        if(BestScore>=LastBestScore)
        {
            LastBestScore=BestScore;
            OnBestScoreChanged?.Invoke();
        }
       // BladeController.Instance.CanReadInput = false;  //Obsrv pattern? event
 
        
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
