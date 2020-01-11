using UnityEngine;
using System.Collections.Generic;
using Rewired;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired.ControllerExtensions;

[AddComponentMenu("")]
public class StartToJoin : MonoBehaviour
{
    public static StartToJoin instance;

    public int maxPlayers = 4;
    public List<PlayerMap> playerMap; // Maps Rewired Player ids to game player ids
    
    public Text player1Text;
    public Text player2Text;
    public Text player3Text;
    public Text player4Text;

    public GameObject player1Bot;
    public GameObject player2Bot;
    public GameObject player3Bot;
    public GameObject player4Bot;

    private int gamePlayerIdCounter = 0;

    void Start()
    {
        playerMap = new List<PlayerMap>();
        instance = this; // set up the singleton
    }

    //Find "Press Start to Join" text
    private void OnLevelWasLoaded(int level)
    {
        try
        {
            if (player1Text == null)
                player1Text = GameObject.Find("blueJoin").GetComponent<Text>();
            if (player2Text == null)
                player2Text = GameObject.Find("redJoin").GetComponent<Text>();
            if (player3Text == null)
                player3Text = GameObject.Find("greenJoin").GetComponent<Text>();
            if (player4Text == null)
                player4Text = GameObject.Find("yellowJoin").GetComponent<Text>();

            if (player1Bot == null)
                player1Bot = GameObject.Find("blueJoined");
            if (player2Bot == null)
                player2Bot = GameObject.Find("redJoined");
            if (player3Bot == null)
                player3Bot = GameObject.Find("greenJoined");
            if (player4Bot == null)
                player4Bot = GameObject.Find("yellowJoined");
        }
        catch (System.Exception){}       
    }

    //Watch for JoinGame action in each Player
    void Update()
    {
        for(int i = 0; i < ReInput.players.playerCount; i++)
        {
            if(ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
            {
                AssignNextPlayer(i);
            }
        }
    }

    void AssignNextPlayer(int rewiredPlayerId)
    {
        if(playerMap.Count >= maxPlayers)
        {
            Debug.LogError("Max player limit already reached!");
            return;
        }

        int gamePlayerId = GetNextGamePlayerId();

        //Add the Rewired Player as the next open game player slot
        playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

        //DualShock4 Set color
        foreach (Joystick joystick in rewiredPlayer.controllers.Joysticks)
        {
            var ds4 = joystick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;

            switch (gamePlayerId)
            {
                case 0: ds4.SetLightColor(Color.blue);break;
                case 1: ds4.SetLightColor(Color.red); break;
                case 2: ds4.SetLightColor(Color.green); break;
                case 3: ds4.SetLightColor(Color.yellow); break;
            }
        }

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/MenuClick4", gameObject);


        //Set text to Joined
        switch (gamePlayerId)
        {
            case 0: player1Text.text = ""; player1Bot.SetActive(true); break;
            case 1: player2Text.text = ""; player2Bot.SetActive(true); break;
            case 2: player3Text.text = ""; player3Bot.SetActive(true); break;
            case 3: player4Text.text = ""; player4Bot.SetActive(true); break;
        }
    }

    private int GetNextGamePlayerId()
    {
        return gamePlayerIdCounter++;
    }

    //Reset everything
    public void UnassignPlayers()
    {
        playerMap.Clear();

        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player rewiredPlayer = ReInput.players.GetPlayer(i);

            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Assignment");
            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Default");

            //DualShock4 color back to normal
            foreach (Joystick joystick in rewiredPlayer.controllers.Joysticks)
            {
                var ds4 = joystick.GetExtension<IDualShock4Extension>();
                if (ds4 == null) continue;
                ds4.SetLightColor(0.043f, 0.094f, 0.110f, 1f);
            }
        }

        if (player1Text)
            player1Text.text = "Press Start To Join"; player1Bot.SetActive(false);
        if (player2Text)
            player2Text.text = "Press Start To Join"; player2Bot.SetActive(false);
        if (player3Text)
            player3Text.text = "Press Start To Join"; player3Bot.SetActive(false);
        if (player4Text)
            player4Text.text = "Press Start To Join"; player4Bot.SetActive(false);

        gamePlayerIdCounter = 0;
    }

    //When the application finishes, all DS4 lights set to normal
    private void OnApplicationQuit()
    {  
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            Player rewiredPlayer = ReInput.players.GetPlayer(i);
            foreach (Joystick joystick in rewiredPlayer.controllers.Joysticks)
            {
                var ds4 = joystick.GetExtension<IDualShock4Extension>();
                if (ds4 == null) continue;
                ds4.SetLightColor(0.043f, 0.094f, 0.110f, 1f);
            }
        }
    }


    //Called by PlayerController, gets controller of player
    public static Rewired.Player GetRewiredPlayer(int gamePlayerId)
    {
        if (!Rewired.ReInput.isReady) return null;

        if (instance == null)
        {
            Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scene?");
            return null;
        }

        for (int i = 0; i < instance.playerMap.Count; i++)
        {
            if (instance.playerMap[i].gamePlayerId == gamePlayerId) return ReInput.players.GetPlayer(instance.playerMap[i].rewiredPlayerId);
        }
        return null;

    }

    //This class is used to map the Rewired Player Id to your game player id
    public class PlayerMap
    {
        public int rewiredPlayerId;
        public int gamePlayerId;

        public PlayerMap(int rewiredPlayerId, int gamePlayerId)
        {
            this.rewiredPlayerId = rewiredPlayerId;
            this.gamePlayerId = gamePlayerId;
        }
    }
}