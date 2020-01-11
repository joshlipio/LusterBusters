using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles1 : MonoBehaviour
{

    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject effectToSpawn;


    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SpawnVFX();
        }
    }

    void SpawnVFX()
    {
        GameObject vfx;
        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }
}
