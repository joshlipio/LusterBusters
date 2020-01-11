using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           other.GetComponent<PlayerController>().ChangeHealth(-100);
        }

        if (other.tag == "Objective")
        {
            other.transform.position = new Vector3(0f, 1f, 0f);
        }
    }
}
