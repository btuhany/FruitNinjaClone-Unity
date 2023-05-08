using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event System.Action OnBestScoreChanged;
    public event System.Action OnScoreChanged;
    public event System.Action OnLivesChanged;

    public int CurrentScore=0;
    public int BestScore=0;
    public int Lives = 3;
    public int LastBestScore = 0;
    
    private void Awake()
    {
        Instance= this;
        
    }
    private void Start()
    {
        InvokeUIEvents();
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
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Restart()
    {

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
}
