using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Weapon pickup script. Attach to object with a collider. Object's child should be model of the weapon.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    public enum Weapons
    {
        Shotgun,
        MachineGun,
        RocketLauncher
    }

    public Weapons weaponPickUp;

    //Whenever a collision with the weapon pickup occurs
    private void OnCollisionEnter(Collision collision)
    {
        //Check if collided with is player
        if (collision.collider.gameObject.tag == "Player")
        {
            //Get a reference to the player that collided
            PlayerController player = collision.collider.gameObject.GetComponent<PlayerController>();
            Weapon newWeapon;

            //If the player already has a picked up weapon, delete it
            if (player.weaponInventory[1] != null)
            {
                Destroy(player.weaponInventory[1]);
            }

            //Add the weapon pickup to the player
            switch (weaponPickUp)
            {           
                case Weapons.Shotgun:     
                    newWeapon = player.gameObject.AddComponent<Shotgun>();          
                    break;

                case Weapons.MachineGun:
                    newWeapon = player.gameObject.AddComponent<MachineGun>();
                    break;

                case Weapons.RocketLauncher:
                    newWeapon = player.gameObject.AddComponent<RocketLauncher>();
                    break;
                default:
                    newWeapon = player.gameObject.AddComponent<Weapon>();
                    break;
            }

            //Change player's equipped weapon
            player.weaponInventory[1] = newWeapon;
            player.ChangeWeapon(newWeapon);

            //Disable collider and child weapon object
            StartCoroutine(Reenable(10));
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }


    /// <summary>
    /// Reenable after given time
    /// </summary>
    /// <returns></returns>
    IEnumerator Reenable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
