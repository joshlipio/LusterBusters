using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    public int speed;
    public int health;
    public PlayerPanel playerUI;
    public Material material;

    public GameObject playerRespawn;
    //public Transform SpawnPoint;
    public Weapon[] weaponInventory = new Weapon[2];
    Weapon currentWeapon;
    private string hAxis;
    private string vAxis;
    private string leftTrigger;
    private string rightTrigger;
    private int controllerNumber;
    private string xLook;
    private string yLook;
    
    private void Start()
    {
        currentWeapon = GetComponent<Weapon>();
        weaponInventory[0] = currentWeapon;
        SetControllerNumber(playerNumber);
        playerUI.ChangeHealthUI("Health: " + health);
        playerUI.ChangeAmmoUI(currentWeapon.weaponName + ": " + currentWeapon.ammoCount);
    }

    internal void SetControllerNumber(int number)
    {
        controllerNumber = number;
        hAxis = "J" + controllerNumber + "Horizontal";
        vAxis = "J" + controllerNumber + "Vertical";
        leftTrigger = "Fire" + controllerNumber;
        rightTrigger = "Shoot" + controllerNumber;
        xLook = "J" + controllerNumber + "HorizontalLook";
        yLook = "J" + controllerNumber + "VerticalLook";
    }

    void Update()
    {
        if (controllerNumber > 0)
        {
            Vector3 move = new Vector3(Input.GetAxis(hAxis), 0f, -Input.GetAxis(vAxis)) * Time.deltaTime * speed;
            transform.position += move;

            Vector3 look = new Vector3(Input.GetAxisRaw(xLook), 0f, -Input.GetAxisRaw(yLook)) * Time.deltaTime;
            if (look.magnitude > 0.01)
                transform.rotation = Quaternion.LookRotation(look);

            TriggerLight(Input.GetAxis(leftTrigger));

            if (Input.GetAxis(rightTrigger) > 0)
            {
                ShootWeapon();
            }

        }
    }

    void TriggerLight(float intensity)
    {
        GetComponentInChildren<Light>().intensity = 2*intensity;
        if (intensity > 0)
            material.EnableKeyword("_EMISSION");
        else
            material.DisableKeyword("_EMISSION");
    }

    void ShootWeapon()
    {   

        currentWeapon.controllerNumber = controllerNumber;

        currentWeapon.Shoot();

        if (currentWeapon.ammoCount == 0)
        {
            Destroy(currentWeapon);
            ChangeWeapon(weaponInventory[0]);
        }
        playerUI.ChangeAmmoUI(currentWeapon.weaponName + ": "+ currentWeapon.ammoCount);
  
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        playerUI.ChangeAmmoUI(currentWeapon.weaponName + ": " + currentWeapon.ammoCount);
    }

    public void ChangeHealth(int change)
    {
        health += change;
        playerUI.ChangeHealthUI("Health: " + health);
        if (health <= 0)
        {
            Transform childObjective = transform.Find("Objective");
            if (childObjective != null)
            {
                childObjective.parent = null;
            }


            transform.position = new Vector3(999, 999, 999);
            Invoke("Respawn", 1);
        }
    }

    private void Respawn()
    {
        Transform spawn = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)];
        transform.position = spawn.position;
        health = 10;
        ChangeHealth(0);
     
        GameObject respawn =  Instantiate(playerRespawn, spawn.position , playerRespawn.transform.rotation);
       
    }

}
