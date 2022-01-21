using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    // Score variables
    public static int score; // Acces from other scripts by writing UIcontoller.score
    public static int highscore;

    // UI variables
    public Image gameOverMenu;
    public Image finishMenu;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public Slider powerUpSlider;


    private Player player;

    // Use this for initialization
    void Start()
    {
        gameOverMenu.gameObject.SetActive(false);
        finishMenu.gameObject.SetActive(false);
        powerUpSlider.gameObject.SetActive(false);
        highscore = PlayerPrefs.GetInt("highscore", highscore); // Loading the high score from playerprefs
        highScoreText.text = "High Score: " + highscore.ToString();

        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        // Update the UI
        scoreText.text = "Score: " + score.ToString();

        // Display the UI elements
        // UI slider
        if (player.powerUpActivated)
        {
            powerUpSlider.gameObject.SetActive(true);
            powerUpSlider.value = -player.powerUpTimer; // Move the slider bar according to the poweruptimer
        }
        else
        {
            powerUpSlider.gameObject.SetActive(false);
        }

        if (player.isDead)
        {
            Invoke("GameOverMenu", 2f); // Calls the menu with delay
        }

        if (player.levelFinished && (SceneManager.GetActiveScene().buildIndex) != 3) // Check if the finished level is not the last level (level 3 in this game)
        {
            Invoke("FinishMenu", 2f); // Calls the menu with delay
        }

        if (player.levelFinished && (SceneManager.GetActiveScene().buildIndex) == 3) // Check if the finished level is  the last level (level 3 in this game)
        {
            Invoke("LastMenu", 2f); // Calls the menu with delay
        }
    }

    void GameOverMenu()
    {
        gameOverMenu.gameObject.SetActive(true);
    }

    void FinishMenu()
    {
        finishMenu.gameObject.SetActive(true);
    }

    // Change the level
    // Restart
    public void RestartGame()
    {
        // Before changing scene, check whether the current score is higher than the high score
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore); // Save the new highscore
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        score = 0;
    }

    // Main menu
    public void MainMenu()
    {
        // Before changing scene, check whether the current score is higher than the high score
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore); // Save the new highscore
        }
        SceneManager.LoadScene("MainMenu");
        score = 0;
    }

    // Next level
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Last Menu
    public void LastMenu()
    {

        // Before endingv the game, check whether the current score is higher than the high score
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore); // Save the new highscore
        }
               
        SceneManager.LoadScene("LastMenu");
        score = 0;
    }
}
