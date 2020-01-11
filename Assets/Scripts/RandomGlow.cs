using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gets all children and assigns random color glow
/// </summary>
public class RandomGlow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Material player1Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_blue 1");
        Material player2Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_red 1");
        Material player3Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_green 1");
        Material player4Mat = Resources.Load<Material>("Materials/PlayerGlow/glow_yellow 1");

        //Grab all child renderers in game object this is attached to
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (var renderer in renderers)
        {
            switch (Random.Range(1, 5))
            {
                case 1:
                    renderer.material = player1Mat;
                    break;
                case 2:
                    renderer.material = player2Mat;
                    break;
                case 3:
                    renderer.material = player3Mat;
                    break;
                case 4:
                    renderer.material = player4Mat;
                    break;

            }
        }
    }
}
