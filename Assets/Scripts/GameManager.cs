using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    private const int firstGamingLevel = 1;
    private const int lastGamingLevel = 3;
    private const int maxLives = 3;

    private int actualLevel = 0;

    private int score = 0;
    private string playerName = "Player1";
    private int lives = maxLives;
    private int bonus = 0;

    bool scenesAreInTransition = false;

    private bool textsNotLinked = true;

    Text playerNameText;
    Text playerScoreText;
    Text playerLivesText;
    Text playerBonusText;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        actualLevel = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) Application.Quit();

        linkTexts();
    }

    private void linkTexts()
    {
        if (textsNotLinked)
        {
            textsNotLinked = false;
            if (actualLevel == 0) return;  //pas utilisé sur l'écran de titre

            playerNameText = GameObject.FindGameObjectWithTag("TextName").GetComponent<Text>();
            playerNameText.text = playerName;

            playerLivesText = GameObject.FindGameObjectWithTag("TextLives").GetComponent<Text>();
            playerLivesText.text = lives.ToString();

            playerScoreText = GameObject.FindGameObjectWithTag("TextScore").GetComponent<Text>();
            playerScoreText.text = score.ToString();

            playerBonusText = GameObject.FindGameObjectWithTag("TextBonus").GetComponent<Text>();
            playerBonusText.text = bonus.ToString();
        }
    }

    public void RestartLevel(float delay)
    {
        if (scenesAreInTransition) return;  //Pour éviter plusieurs transitions lancées en bloc

        scenesAreInTransition = true;

        StartCoroutine(RestartLevelDelay(delay, actualLevel));
    }


    public void StartNextlevel(float delay)
    {
        if (scenesAreInTransition) return;  //Pour éviter plusieurs transitions lancées en bloc

        scenesAreInTransition = true;

        StartCoroutine(RestartLevelDelay(delay, GetNextLevel()));
    }

    private IEnumerator RestartLevelDelay(float delay, int level)
    {
        yield return new WaitForSeconds(delay);
        textsNotLinked = true;

        if (lives == 0)
        {
            bonus = 0;
            SceneManager.LoadScene("SceneGameOver");
        }
        else if (level == 2)
            SceneManager.LoadScene("Scene2");
        else if (level == 3)
            SceneManager.LoadScene("Scene3");
        else
            SceneManager.LoadScene("Scene1");

        scenesAreInTransition = false;
    }

    public void ResetGame()
    {
        lives = maxLives;
        actualLevel = 0;
        score = 0;
        SceneManager.LoadScene("Scene0");
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    private int GetNextLevel()
    {
        if (++actualLevel == lastGamingLevel + 1)
            actualLevel = firstGamingLevel;

        return actualLevel;
    }

    //---------------------------------------------------------------
    //Role "traditionnel" du Game Manager: petit donc on le garde ici
    //---------------------------------------------------------------
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        playerScoreText.text = score.ToString();
    }

    public void PlayerDie()
    {
        lives--;
        playerLivesText.text = lives.ToString();
    }

    public void SetBonus(int bonus)
    {
        this.bonus = bonus;

        if (playerBonusText == null) return;

        playerBonusText.text = this.bonus.ToString();
    }
}