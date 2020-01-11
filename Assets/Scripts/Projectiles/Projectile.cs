
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon types can shoot different projectiles
/// </summary>
public class Projectile : MonoBehaviour
{
    public float despawnTime;
    public float speed;
    public int damage;

    private void Start()
    {
        Invoke("DestroyProjectile", despawnTime);
        Physics.IgnoreLayerCollision(8, 8);
    }

    /// <summary>
    /// Virtual because we might want to change behavior on how it destroys itself
    /// </summary>
    public virtual void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Player")
        {
            collision.collider.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
        }
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
}
