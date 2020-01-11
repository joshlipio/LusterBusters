using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test script
/// </summary>
public class EmissionControl : MonoBehaviour
{

    public Material material;
    // Start is called before the first frame update
    void Awake ()
    {
        material.DisableKeyword ("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= 3.0f)
        {
            material.EnableKeyword("_EMISSION");
        }
    }
}
