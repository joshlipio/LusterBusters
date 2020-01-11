using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;

/// <summary>
///  In game pause menu UI manager class. There should only be one of these in a scene!
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Bool for if the game is paused or not
    /// </summary>
    public static bool isPaused = false;

    /// <summary>
    /// Bool for if the game is over or not
    /// </summary>
    public static bool isOver = false;

    public Button backButton;
    public Button resumeButton;
    public Button instructionsButton;
    public Button restartVictory;

    public GameObject pauseMenu;
    public GameObject instructions;
    public GameObject victoryScreen;

    public static PauseMenu instance; //Let's the script be called by other scripts

    void Start()
    {
        instance = this;
    }


    public void handleStart()
    {
        if (isPaused && !isOver)
            Resume();
        else if (!isOver)
            Pause();
    }

    public void Resume()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick2", gameObject);
        pauseMenu.SetActive(false);
        instructions.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        pauseMenu.SetActive(true);
        instructions.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;

        resumeButton.Select();
        resumeButton.OnSelect(null);
    }

    public void handleCancel()
    {
        if (isPaused && !isOver && instructions.activeSelf)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick2", gameObject);
            instructions.SetActive(false);
            pauseMenu.SetActive(true);

            instructionsButton.Select();
            instructionsButton.OnSelect(null);
        }

        else if (isPaused && !isOver)
        {
            Resume();
        }
    }

    public void showInstructions()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        pauseMenu.SetActive(false);
        instructions.SetActive(true);

        backButton.Select();
        backButton.OnSelect(null);
    }

    public void Restart()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isOver = false;
        Resume();
    }

    public void QuitToMenu()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        SceneManager.LoadScene("mainTitle");
        isOver = false;
        Resume();
        StartToJoin.instance.UnassignPlayers();
    }

    /// <summary>
    /// Called by GameManager, displays victory screen
    /// </summary>
    /// <param name="playerNumber">Player winner number</param>
    public void Victory(int playerNumber)
    {   
        victoryScreen.SetActive(true);
        Time.timeScale = 0f;
        isOver = true;

        Text VictoryUI = victoryScreen.transform.Find("Winner").gameObject.GetComponent<Text>();

        switch (playerNumber)
        {   
            case 1:
                VictoryUI.text = "Blue Player Wins";
                break;
            case 2:
                VictoryUI.text = "Red Player Wins";
                break;
            case 3:
                VictoryUI.text = "Green Player Wins";
                break;
            case 4:
                VictoryUI.text = "Yellow Player Wins";
                break;
        }

        restartVictory.Select();
        restartVictory.OnSelect(null);
    }
}
