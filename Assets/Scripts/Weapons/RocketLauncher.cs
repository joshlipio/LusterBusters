using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    public override void Awake()
    {
        weaponName = "Rocket";
        lastShoot = Time.time;
        shake = FindObjectOfType<CameraShake>();
        fireRate = 1;
        ammoCount = 3;
    }

    public override void Shoot(Player player)
    {
        if ((Time.time - lastShoot > fireRate) && ammoCount > 0)
        {
            player.SetVibration(0, .75f, 0.3f);

            lastShoot = Time.time;
            shake.ShakeCaller(.075f, 0.3f);
            Projectile bullet = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            ammoCount--;
        }
    }
}
