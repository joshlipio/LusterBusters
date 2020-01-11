using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public override void Awake()
    {
        weaponName = "Shotgun";
        lastShoot = Time.time;
        shake = FindObjectOfType<CameraShake>();
        fireRate = .5f;
        ammoCount = 5;
    }

    public override void Shoot(Player player)
    {
        if ((Time.time - lastShoot > fireRate) && ammoCount > 0)
        {
            player.SetVibration(0, 0.75f, 0.25f);
            shake.ShakeCaller(.05f, 0.35f);
            lastShoot = Time.time;

            Projectile bullet1 = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet1.transform.Rotate(new Vector3(0, -19.5f, 0));

            Projectile bullet2 = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet2.transform.Rotate(new Vector3(0, -6.5f, 0));

            Projectile bullet3 = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet3.transform.Rotate(new Vector3(0, 6.5f, 0));

            Projectile bullet4 = Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet4.transform.Rotate(new Vector3(0, 19.5f, 0));

            ammoCount--;
        }
    }
}
