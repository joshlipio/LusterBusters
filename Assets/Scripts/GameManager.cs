using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for player score UI, game win condition, and changing objective color 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// Player number of current person in the lead
    /// </summary>
    public int currentLeader = 0;

    /// <summary>
    /// List of transforms that are the potential spawn points
    /// </summary>
    public Transform[] spawnPoints;

    //References to all playerUIs
    public PlayerPanel player1UI;
    public PlayerPanel player2UI;
    public PlayerPanel player3UI;
    public PlayerPanel player4UI;

    //Reference to the objective game object
    GameObject objective;

    //All the player scores
    int[] playerScores;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerScores = new int[5];
        objective = FindObjectOfType<ObjectiveScript>().gameObject; //Find where the objective is
        InvokeRepeating("CheckObjective", 0f, 1f); //Calls CheckObjective() every second
    }

    /// <summary>
    /// Increase the score of a player by an amount. Called when a projectile kills a player to increment the killer's score.
    /// </summary>
    /// <param name="playerNumber"></param>
    /// <param name="amount"></param>
    public void IncreaseScore(int playerNumber, int amount)
    {
        switch (playerNumber)
        {
            case 1:
                playerScores[1] += amount;
                player1UI.ChangeScoreUI(playerScores[1].ToString());
                break;
            case 2:
                playerScores[2] += amount;
                player2UI.ChangeScoreUI(playerScores[2].ToString());
                break;
            case 3:
                playerScores[3] += amount;
                player3UI.ChangeScoreUI(playerScores[3].ToString());
                break;
            case 4:
                playerScores[4] += amount;
                player4UI.ChangeScoreUI(playerScores[4].ToString());
                break;

        }
    }

    /// <summary>
    /// Checks which player has object with ObjectiveScript() attached, and increments score
    /// </summary>
    void CheckObjective()
    {
        //If objective has a parent (a player has the objective)
        if (objective.transform.parent != null)
        {
            //Switch based on who has the objective
            switch (objective.transform.parent.name)
            {
                //increment their score, and update UI
                case "Player1":      
                    playerScores[1]++;
                    player1UI.ChangeScoreUI(playerScores[1].ToString());
                    break;
                case "Player2":
                    playerScores[2]++;
                    player2UI.ChangeScoreUI(playerScores[2].ToString());
                    break;
                case "Player3":
                    playerScores[3]++;
                    player3UI.ChangeScoreUI(playerScores[3].ToString());
                    break;
                case "Player4":
                    playerScores[4]++;
                    player4UI.ChangeScoreUI(playerScores[4].ToString());
                    break;
            }
        }

        //Update current leader if there's a new person in the lead
        for (int i = 0; i < playerScores.Length; i++)
        {
            if (playerScores[currentLeader] < playerScores[i])
            {
                //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Buzzer2", gameObject);
                currentLeader = i;
            }
        }

        //If there's no time left, open victory screen by passing in the current leader as the winner
        if (Timer.instance.startTime == 0)
        {
            PauseMenu.instance.Victory(currentLeader);
        }

    }
}
