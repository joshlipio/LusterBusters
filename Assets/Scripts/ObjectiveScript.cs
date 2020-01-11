using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objective script that attaches the object to a player that collides with it
/// </summary>
public class ObjectiveScript : MonoBehaviour
{
    //Player materials for changing objective color
    Material player1Mat;
    Material player2Mat;
    Material player3Mat;
    Material player4Mat;
    Material defaultObjectiveMat;

    //Start material for the object this is attached to
    Material material1;

    //Material this object is lerping to
    Material material2;

    //Duration of the lerp
    float duration = 1f;
    float lerp = 0;

    Renderer objectRenderer;

    private void Start()
    {
        //Load the player materials from Materials folder
        player1Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_blue 1");
        player2Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_red 1");
        player3Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_green 1");
        player4Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_yellow 1");
        defaultObjectiveMat = Resources.Load<Material>("Materials/objective");
        
        objectRenderer = gameObject.GetComponentInChildren<Renderer>();

        //Set both materials to the current material of the object at start of game
        material1 = gameObject.GetComponentInChildren<Renderer>().material;
        material2 = gameObject.GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        if (transform.parent == null)
        {
            material2 = defaultObjectiveMat;
        }

        if (lerp < 1)
        {
            lerp += Time.deltaTime / duration;
        }

        //Get that renderers current materials list
        var mats = objectRenderer.materials;

        //Lerp the material and set the objects materials
        objectRenderer.material.Lerp(material1, material2, lerp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {     
            transform.parent = other.gameObject.transform;
            gameObject.transform.localPosition = Vector3.zero;

            switch (other.gameObject.name)
            {
                case "Player1":
                    material2 = player1Mat; lerp = 0;
                    break;
                case "Player2":
                    material2 = player2Mat; lerp = 0;
                    break;
                case "Player3":
                    material2 = player3Mat; lerp = 0;
                    break;
                case "Player4":
                    material2 = player4Mat; lerp = 0;
                    break;
            }

        }
    }


}
