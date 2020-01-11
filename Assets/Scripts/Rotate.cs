using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float speed;

    /// <summary>
    /// Bool for if the object rotates every so often (such as every 30 seconds)
    /// </summary>
    public bool rotateOccasionally;

    /// <summary>
    /// Only used if rotateOccsionally is enabled. The delay between each rotation.
    /// </summary>
    public float rotatePeriod;

    private void Start()
    {
        if (rotateOccasionally)
        {
            InvokeRepeating("Delay", rotatePeriod, rotatePeriod);
        }
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }

    void Delay()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }
}
