using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using UnityEngine.UI;

/// <summary>
/// Main menu UI manager class
/// </summary>
public class MainMenu : MonoBehaviour
{
    public Button backInstructions;
    public Button backCredits;
    public Button backPlay;
    public Button backSettings;
    public Button play;
    public Button settings;
    public Button credits;
    public Button instructions;

    private Button lastSelected;
    public GameObject rotationPoint;

    private Vector3 currentAngle;
    private Vector3 targetAngle;

    private void Start()
    {
        currentAngle = rotationPoint.transform.eulerAngles;
    }

    void Update()
    {
        // Watch for JoinGame action in each Player
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            if (ReInput.players.GetPlayer(i).GetButtonDown("UICancel"))
            {
                showTitleScreen();
            }
        }

        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime * 10),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime * 10),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * 10));

        rotationPoint.transform.eulerAngles = currentAngle;
    }

    public void showTitleScreen()
    {
        StartToJoin.instance.UnassignPlayers();

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick2", gameObject);

        targetAngle = new Vector3(0f, 0f, 0f);

        lastSelected.Select();
        lastSelected.OnSelect(null);
    }

    public void showInstructions()
    {
        lastSelected = instructions;

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        targetAngle = new Vector3(0f, -120f, 0f);

        backInstructions.Select();
        backInstructions.OnSelect(null);

    }

    public void showSettings()
    {
        lastSelected = settings;

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        targetAngle = new Vector3(90f, 0f, 0f);

        backSettings.Select();
        backSettings.OnSelect(null);
    }

    public void showCredits()
    {
        lastSelected = credits;

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        targetAngle = new Vector3(0f, 120f, 0f);

        backCredits.Select();
        backCredits.OnSelect(null);
    }

    public void showPlayerSelect()
    {
        StartToJoin.instance.UnassignPlayers();

        lastSelected = play;

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        targetAngle = new Vector3(-90f, 0f, 0f);

        backPlay.Select();
        backPlay.OnSelect(null);
    }

    public void startGame()
    {
        if (StartToJoin.instance.playerMap.Count >= 2)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
            SceneManager.LoadScene("NewMap");
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick3", gameObject);
        }
    }

    public void Quit()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick", gameObject);
        Application.Quit();
    }

}
