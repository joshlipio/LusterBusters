using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapons can have different behaviors (faster fire rate, different projectiles)
/// </summary>
public class Weapon : MonoBehaviour
{
    /// <summary>
    /// The projectile to be fired
    /// </summary>
    protected Projectile projectile;

    /// <summary>
    /// Cooldown time of the gun, in seconds
    /// </summary>
    protected float fireRate;

    /// <summary>
    /// Speed of the projectile that's being fired
    /// </summary>
    protected float projectileSpeed;

    /// <summary>
    /// Float for checking gun cooldown
    /// </summary>
    public float lastShoot;

    /// <summary>
    /// Amount of ammo left in the gun
    /// </summary>
    public int ammoCount;

    /// <summary>
    /// Name of the weapon
    /// </summary>
    public string weaponName;

    /// <summary>
    /// Lets the weapon shake the camera for shooting
    /// </summary>
    public CameraShake shake;

    /// <summary>
    /// Overridable Awake() function, because weapon attributes can change
    /// </summary>
    public virtual void Awake()
    {
        weaponName = "Pistol";
        lastShoot = Time.time;
        shake = FindObjectOfType<CameraShake>();
        fireRate = .5f;
        projectileSpeed = 25;
        ammoCount = -1;
    }

    public void Color(int controllerNumber)
    {
        switch (controllerNumber)
        {
            case 0:
                projectile = Resources.Load<Projectile>("Prefabs/WeaponProjectileEffects/" + weaponName + "Blue");
                break;
            case 1:
                projectile = Resources.Load<Projectile>("Prefabs/WeaponProjectileEffects/" + weaponName + "Red");
                break;
            case 2:
                projectile = Resources.Load<Projectile>("Prefabs/WeaponProjectileEffects/" + weaponName + "Green");
                break;
            case 3:
                projectile = Resources.Load<Projectile>("Prefabs/WeaponProjectileEffects/" + weaponName + "Yellow");
                break;
        }
    }

    /// <summary>
    /// Overridable Shoot() function, in case maybe it shoots multiple projectiles
    /// </summary>
    public virtual void Shoot(Player player)
    {
        //Checks fire rate against time last shot
        if (Time.time - lastShoot > fireRate)
        {
            player.SetVibration(0, 0.75f, 0.1f);

            lastShoot = Time.time;
            shake.ShakeCaller(.02f, .25f); //Shake the screen
            Projectile bullet = Instantiate(projectile, transform.position + transform.forward, transform.rotation); //Instantiate the bullet
            bullet.speed = projectileSpeed; //Set its speed
        }
    }
}
