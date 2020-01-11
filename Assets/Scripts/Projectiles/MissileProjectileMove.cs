using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissileProjectileMove : Projectile
{
    public bool bounce = false;
    public float bounceForce = 10;
	public GameObject muzzlePrefab;
	public GameObject hitPrefab;
	public string shotSFX;
	public string hitSFX;
	public List<GameObject> trails;
    public int playerNumber;
    public float vibrationIntensity;
    public float vibrationDuration;

    private Vector3 startPos;
	private bool collided;
	private Rigidbody rb;
    private Transform target;
    CameraShake shake;

	void Start ()
    {    
        rb = GetComponent <Rigidbody> ();
        startPos = transform.position;

        try
        {
            target = GameObject.FindGameObjectsWithTag("Player")
            .OrderBy(o => (o.transform.position - startPos).sqrMagnitude)
            .ElementAtOrDefault(1).transform;
        }
        catch (System.Exception){}
        
        shake = FindObjectOfType<CameraShake>();

        Invoke("DestroyProjectile", despawnTime);
        Physics.IgnoreLayerCollision(8, 8);
			
		if (muzzlePrefab != null) {
			var muzzleVFX = Instantiate (muzzlePrefab, transform.position, Quaternion.identity);
			muzzleVFX.transform.forward = gameObject.transform.forward;
			var ps = muzzleVFX.GetComponent<ParticleSystem>();
			if (ps != null)
				Destroy (muzzleVFX, ps.main.duration);
			else {
				var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
				Destroy (muzzleVFX, psChild.main.duration);
			}
		}

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/" + shotSFX, gameObject);

    }

    public override void FixedUpdate ()
    {
        if (speed != 0 && rb != null)
        {
            rb.position += (transform.forward) * (speed * Time.deltaTime);
            if (target != null)
            {
                var rotate = Quaternion.LookRotation(target.position - transform.position);
                rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotate, 10f));
            }
        }
    }

	void OnCollisionEnter (Collision co)
    {
        if (!bounce)
        {
            if (co.gameObject.tag != "Bullet" && !collided)
            {
                collided = true;
                shake.ShakeCaller(0.1f, 1f);

                if (hitSFX != null)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached("event:/" + hitSFX, gameObject);
                }

                if (co.collider.gameObject.tag == "Player")
                {
                    PlayerController hitPlayer = co.collider.gameObject.GetComponent<PlayerController>();
                    if (hitPlayer.health > 0)
                    {
                       hitPlayer.player.SetVibration(0, vibrationIntensity, vibrationDuration);
                        hitPlayer.ChangeHealth(-damage);
                        if (hitPlayer.health <= 0)
                        {
                            switch (playerNumber)
                            {
                                case 1:
                                    GameManager.instance.IncreaseScore(1, 5);
                                    break;
                                case 2:
                                    GameManager.instance.IncreaseScore(2, 5);
                                    break;
                                case 3:
                                    GameManager.instance.IncreaseScore(3, 5);
                                    break;
                                case 4:
                                    GameManager.instance.IncreaseScore(4, 5);
                                    break;

                            }
                        }
                    }
                }

                if (trails.Count > 0)
                {
                    for (int i = 0; i < trails.Count; i++)
                    {
                        trails[i].transform.parent = null;
                        var ps = trails[i].GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            ps.Stop();
                            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                        }
                    }
                }

                speed = 0;
                GetComponent<Rigidbody>().isKinematic = true;

                ContactPoint contact = co.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                if (hitPrefab != null)
                {
                    var hitVFX = Instantiate(hitPrefab, pos, rot) as GameObject;

                    var ps = hitVFX.GetComponent<ParticleSystem>();
                    if (ps == null)
                    {
                        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVFX, psChild.main.duration);
                    }
                    else
                        Destroy(hitVFX, ps.main.duration);
                }

                StartCoroutine(DestroyParticle(0f));
            }
        }
        else
        {
            rb.useGravity = true;
            rb.drag = 0.5f;
            ContactPoint contact = co.contacts[0];
            rb.AddForce (Vector3.Reflect((contact.point - startPos).normalized, contact.normal) * bounceForce, ForceMode.Impulse);
            Destroy ( this );
        }
	}

	public IEnumerator DestroyParticle (float waitTime)
    {

		if (transform.childCount > 0 && waitTime != 0) {
			List<Transform> tList = new List<Transform> ();

			foreach (Transform t in transform.GetChild(0).transform) {
				tList.Add (t);
			}		

			while (transform.GetChild(0).localScale.x > 0) {
				yield return new WaitForSeconds (0.01f);
				transform.GetChild(0).localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				for (int i = 0; i < tList.Count; i++) {
					tList[i].localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
				}
			}
		}
		
		yield return new WaitForSeconds (waitTime);
		Destroy (gameObject);
	}
}
