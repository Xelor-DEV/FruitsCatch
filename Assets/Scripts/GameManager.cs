using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text scoreText;
    public Text timerText;
    public Text finalScore;
    //public Text lastFinalScore;

    public GameObject gameOverPanel;
    public float timeLimit = 60f; // 1.5 minutos
    public int lives = 5; // Numero de vidas

    private float currentTime;
    private int score;
    private int lastScore;

    public ProgressData progress;

    public DBConn dbConn;  // Referencia al script de conexión a la BD
    public UserSessionData sessionData;  // Referencia al ScriptableObject con el userId

    private bool isGamePaused = false;  // Estado de la pausa
    private bool isGameOver = false;   // Indica si el juego ha terminado

    public void ReceiveUserId(string userID)
    {
        sessionData.userId = int.Parse(userID);
        Debug.Log("user id: " + userID);
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ResumeGame();
        currentTime = timeLimit;
        score = 0;
        UpdateScoreText();
        gameOverPanel.SetActive(false);
        //LoadProgress();
        lastScore = progress.score;
        dbConn.SelectedOperation = OperationType.RecordScore;
        isGameOver = false;  // Asegurarse de que no esté terminado al iniciar
    }

    void Update()
    {
        if (!isGamePaused) // Solo actualiza el juego si no está en pausa
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Max(0, currentTime).ToString("F2");

            if (currentTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(int points)
    {
        if (!isGamePaused)  // Solo agregar puntos si el juego no está en pausa
        {
            score += points;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void LoseLife()
    {
        if (!isGamePaused)  // Solo restar vidas si el juego no está en pausa
        {
            lives--;
            if (lives <= 0)
            {
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        // Lógica para terminar el juego
        gameOverPanel.SetActive(true);
        finalScore.text = "Score: " + score;
        //lastFinalScore.text = "Last Score: " + lastScore;
        SaveProgress();
        PauseGame();
        Time.timeScale = 0;
        isGameOver = true; // Marcar el juego como terminado
    }

    public void SaveProgress()
    {
        progress.score = score;
        string json = JsonUtility.ToJson(progress);
        //SaveData.Save("progress.json", json);
        print("Data saved: " + progress.score + " points.");

        if (sessionData != null)
        {
            dbConn.SendScoreToDatabase(score, sessionData.userId);
        }

    }
    public void LoadProgress()
    {
        progress = JsonUtility.FromJson<ProgressData>(SaveData.Load("progress.json"));
    }
    public void ResetGame()
    {
        if (isGameOver == true)  // Solo permitir reiniciar si el juego ha terminado
        {
            SceneManager.LoadScene("Game");
            ResumeGame();
        }
    }


    private void PauseGame()
    {
        isGamePaused = true;    // Cambiar el estado a pausado
        Time.timeScale = 0;     // Detener el tiempo del juego
    }

    private void ResumeGame()
    {
        isGamePaused = false;   // Cambiar el estado a no pausado
        Time.timeScale = 1;     // Reanudar el tiempo del juego
    }
}

