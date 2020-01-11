using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Timer script. There should only be one of these in a scene!
/// </summary>
public class Timer : MonoBehaviour
{
    /// <summary>
    /// Timer for the round
    /// </summary>
    public float startTime;
    public GameObject timeUI;
    public static Timer instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InvokeRepeating("TimerDecrement", 0, 1);
        timeUI = transform.Find("Timecount").gameObject;
    }

    void TimerDecrement()
    {
        if (startTime > 0)
        {
            startTime--;

            if (startTime <= 10)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Buzzer", gameObject);
            }

            //Format into 0:00
            string minutes = ((int)startTime / 60).ToString();
            string seconds = (startTime % 60).ToString("00");

            timeUI.GetComponent<Text>().text = minutes + ":" + seconds;
        }
    }
}
