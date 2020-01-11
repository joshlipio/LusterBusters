using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnAmplitude : MonoBehaviour
{
    Visualizer vis;

    // Start is called before the first frame update
    void Start()
    {
        vis = FindObjectOfType<Visualizer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vis.amplitude * 0.01f < 5f)
            GetComponent<Light>().intensity = 5f;
        else if (vis.amplitude * 0.01f > 20f)
            GetComponent<Light>().intensity = 20f;
        else
            GetComponent<Light>().intensity = vis.amplitude * 0.01f;
    }
}
