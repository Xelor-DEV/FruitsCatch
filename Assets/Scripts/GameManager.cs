using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text scoreText;
    public Text timerText;
    public Text finalScore;
    public Text lastFinalScore;

    public GameObject gameOverPanel;
    public float timeLimit = 60f; // 1.5 minutos
    public int lives = 5; // Numero de vidas

    private float currentTime;
    private int score;
    private int lastScore;

    public ProgressData progress;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentTime = timeLimit;
        score = 0;
        UpdateScoreText();
        gameOverPanel.SetActive(false);
        LoadProgress();
        lastScore = progress.score;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Max(0, currentTime).ToString("F2");

        if (currentTime <= 0)
        {
            EndGame();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // Lógica para terminar el juego
        gameOverPanel.SetActive(true);
        finalScore.text = "Score: " + score;
        lastFinalScore.text = "Last Score: " + lastScore;
        SaveProgress();
    }

    public void SaveProgress()
    {
        progress.score = score;
        string json = JsonUtility.ToJson(progress);
        SaveData.Save("progress.json", json);
        print("Data saved: " + progress.score + " points.");
    }
    public void LoadProgress()
    {
        progress = JsonUtility.FromJson<ProgressData>(SaveData.Load("progress.json"));
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }
}

