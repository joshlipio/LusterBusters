using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAdapt : MonoBehaviour
{
    //Player materials for changing objective color
    Material player1Mat;
    Material player2Mat;
    Material player3Mat;
    Material player4Mat;

    //Start material for the object this is attached to
    Material material1;

    //Material this object is lerping to
    Material material2;

    //Duration of the lerp
    float duration = 5f;
    float lerp = 1;

    Renderer[] objectRenderers;

    /// <summary>
    /// Index of the material being changed
    /// </summary>
    public int materialNumber;

    private int currentLeader = 0;

    void Awake()
    {
        InvokeRepeating("CheckWinner", 0f, 1f); //Calls every second

        //Load the player materials from Materials folder
        player1Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_blue 1");
        player2Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_red 1");
        player3Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_green 1");
        player4Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_yellow 1");

        //Grabs all child renderers in game object this is attached to
        objectRenderers = this.gameObject.GetComponentsInChildren<Renderer>();

        //Set both materials to the current material of the object at start of game
        material1 = this.gameObject.GetComponentInChildren<Renderer>().materials[materialNumber];
        material2 = this.gameObject.GetComponentInChildren<Renderer>().materials[materialNumber];
    }

    // Update is called once per frame
    void Update()
    {
        if (lerp < 1)
        {
            lerp += Time.deltaTime / duration;
        }

        if (lerp >= 1)
        {
            material1 = material2;
        }

        //Loop through each renderer this object is a prent to
        foreach (var objectRenderer in objectRenderers)
        {
            //Get that renderers current materials list
            var mats = objectRenderer.materials;

            //Lerp the material and set the objects materials
            mats[materialNumber].Lerp(material1, material2, lerp);
            objectRenderer.materials = mats;
        }
        
    }

    //Check current leader so it can change the color if its different
    void CheckWinner()
    {
        if (GameManager.instance.currentLeader != currentLeader)
        {
            switch (GameManager.instance.currentLeader)
            {
                case 1:
                    currentLeader = 1;
                    material2 = player1Mat; lerp = 0;
                    break;
                case 2:
                    currentLeader = 2;
                    material2 = player2Mat; lerp = 0;
                    break;
                case 3:
                    currentLeader = 3;
                    material2 = player3Mat; lerp = 0;
                    break;
                case 4:
                    currentLeader = 4;
                    material2 = player4Mat; lerp = 0;
                    break;
            }
        }
    }
}
