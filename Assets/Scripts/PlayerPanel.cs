using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public GameObject HealthUI;
    public GameObject AmmoUI;
    public GameObject ScoreUI;
    public GameObject GunUI;

    // Start is called before the first frame update
    void Start()
    {
        //Health, Ammo, and Score should be children objects to the object this script is attached to
        HealthUI = transform.Find("Health").gameObject;
        AmmoUI = transform.Find("Ammo").gameObject;
        ScoreUI = transform.Find("Score").gameObject;
        GunUI = transform.Find("Gun").gameObject;
    }

    /// <summary>
    /// Change the health UI
    /// </summary>
    /// <param name="newHealth">String the the text to be displayed</param>
    public void ChangeHealthUI(string newHealth)
    {
        HealthUI.GetComponent<Text>().text = newHealth;
    }

    /// <summary>
    /// Change the ammo UI
    /// </summary>
    /// <param name="newAmmo">String the the text to be displayed</param>
    public void ChangeAmmoUI(string newAmmo)
    {
        AmmoUI.GetComponent<Text>().text = newAmmo;
    }

    public void ChangeGunUI(string newGun)
    {
        GunUI.GetComponent<RawImage>().texture = Resources.Load<Texture>("GunUI/" + newGun);
    }


    /// <summary>
    /// Change the score UI
    /// </summary>
    /// <param name="newScore">String the the text to be displayed</param>
    public void ChangeScoreUI(string newScore)
    {
        ScoreUI.GetComponent<Text>().text = newScore;
    }
}
