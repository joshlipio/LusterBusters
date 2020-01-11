using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public float speed;
    public float fireRate;
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    // Start is called before the first frame update
    void Start()
    {
           if(muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            transform.position = transform.position + transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed");
        }
    }
    private void OnCollisionEnter(Collision co)
    {
        speed = 0;
        if (hitPrefab!= null)
        {
            ContactPoint contact = co.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            var hitVFX = Instantiate(hitPrefab, pos, rot);
        }
        Destroy(gameObject);
    }
}
