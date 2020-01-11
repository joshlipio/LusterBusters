using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerNumber;
    public int speed;
    public int health;
    public PlayerPanel playerUI;
    public Material material;

    public GameObject playerRespawnColor;
    private GameObject respawn;
    //public Transform SpawnPoint;
    public Weapon[] weaponInventory = new Weapon[2];
    Weapon currentWeapon;

    public Rewired.Player player { get { return StartToJoin.GetRewiredPlayer(playerNumber); } }

    private void Start()
    {
        currentWeapon = GetComponent<Weapon>();
        weaponInventory[0] = currentWeapon;
        currentWeapon.Color(playerNumber);
        playerUI.ChangeHealthUI(health.ToString());
        respawn = Instantiate(playerRespawnColor, transform.position, playerRespawnColor.transform.rotation);
        respawn.transform.parent = this.gameObject.transform;
        if (currentWeapon.ammoCount == -1)
            playerUI.ChangeAmmoUI("\u221E");
        else
            playerUI.ChangeAmmoUI(currentWeapon.ammoCount.ToString());

        if (player == null)
        {
            playerUI.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Walls")
        {
            player.SetVibration(0, 0.5f, 0.15f);
        }
    }

    void Update()
    {

        if (!ReInput.isReady) return; // Exit if Rewired isn't ready. This would only happen during a script recompile in the editor.
        if (player == null) return;

        if (Time.timeScale > 0)
        {
            Vector3 move = new Vector3(player.GetAxis("MoveHorizontal"), 0f, player.GetAxis("MoveVertical")) * Time.deltaTime * speed;
            transform.position += move;

            Vector3 look = new Vector3(player.GetAxis("LookHorizontal"), 0f, player.GetAxis("LookVertical")) * Time.deltaTime;
            if (look.magnitude > 0.01)
                transform.rotation = Quaternion.LookRotation(look);

            TriggerLight(player.GetAxis("Reveal"));

            if (player.GetAxis("Fire") > 0)
                ShootWeapon();
        }

        if (player.GetButtonDown("Start"))
        {
            PauseMenu.instance.handleStart();
        }

        if (player.GetButtonDown("UICancel"))
        {
            PauseMenu.instance.handleCancel();
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
        currentWeapon.Shoot(player);

        if (currentWeapon.ammoCount == 0)
        {
            Destroy(currentWeapon);
            ChangeWeapon(weaponInventory[0]);
        }
        if (currentWeapon.ammoCount == -1)
            playerUI.ChangeAmmoUI("\u221E");
        else
            playerUI.ChangeAmmoUI(currentWeapon.ammoCount.ToString());
  
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        currentWeapon.Color(playerNumber);
        playerUI.ChangeGunUI(currentWeapon.weaponName);
        currentWeapon.lastShoot = Time.time;
        if (currentWeapon.ammoCount == -1)
            playerUI.ChangeAmmoUI("\u221E");
        else
            playerUI.ChangeAmmoUI(currentWeapon.ammoCount.ToString());
    }

    public void ChangeHealth(int change)
    {
        health += change;   
        if (health <= 0)
        {
            health = 0; //if less than 0, set to 0 so it doesn't display negative health
            Transform childObjective = transform.Find("Objective");
            if (childObjective != null)
            {
                childObjective.parent = null;
                childObjective.transform.position = new Vector3(childObjective.transform.position.x,
                    1f, childObjective.transform.position.z); 
            }
            ChangeWeapon(weaponInventory[0]); //change to pistol
            transform.position = new Vector3(999, 999, 999);
            Invoke("Respawn", 1);
        }
        playerUI.ChangeHealthUI(health.ToString());
    }

    private void Respawn()
    {
        Transform spawn = GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)];

        //Attempts at better spawning
        //spawn = GameManager.instance.spawnPoints[0].transform;
        //Transform i;
        //Transform furthestPlayer = GameObject.FindGameObjectsWithTag("Player")
        //    .OrderByDescending(o => (o.transform.position - GameManager.instance.spawnPoints[0].
        //    transform.position).sqrMagnitude)
        //    .ElementAtOrDefault(1).transform;

        ////Loop through each spawn points
        //foreach (var item in GameManager.instance.spawnPoints)
        //{
        //    //Find closest player
        //    i = GameObject.FindGameObjectsWithTag("Player")
        //    .OrderByDescending(o => (o.transform.position - item.transform.position).sqrMagnitude)
        //    .ElementAtOrDefault(1).transform;

        //    //If closest player in this spawn point is further, set it as spawn point
        //    if (furthestPlayer.position.sqrMagnitude < i.position.sqrMagnitude)
        //    {
        //        furthestPlayer = i;
        //        spawn = item;
        //    }
        //}

        health = 10;
        ChangeHealth(0);

        transform.position = spawn.position;    
        respawn.GetComponent<ParticleSystem>().Play();
       
    }

}
