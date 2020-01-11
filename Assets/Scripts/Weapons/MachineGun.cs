using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon
{
    public override void Awake()
    {
        weaponName = "MG";
        lastShoot = Time.time;
        shake = FindObjectOfType<CameraShake>();
        fireRate = .1f;
        ammoCount = 30;

    }

    public override void Shoot(Player player)
    {
        if ((Time.time - lastShoot > fireRate) && ammoCount > 0)
        {
            player.SetVibration(0, 0.25f, 0.1f);

            shake.ShakeCaller(.02f, .1f);
            lastShoot = Time.time;
            Projectile bullet = Instantiate(projectile, transform.position + transform.forward, transform.rotation);           
            bullet.transform.Rotate(new Vector3(0, Random.Range(0, 10f),0)); //Add random spread to the bullets
            ammoCount--;
        }
    }
}
