using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    //Mixers for Music, SFX, and Master
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus UI;
    FMOD.Studio.Bus Master;

    /// <summary>
    /// Awake will find the mixer channels in FMOD, get their current values, 
    /// and make sure the setting sliders are in the their correct position
    /// </summary>
    void Awake()
    {
        UI = FMODUnity.RuntimeManager.GetBus("bus:/Master/UI");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");

        float trash, value;

        Master.getVolume(out value, out trash);
        GameObject.Find("MasterSlider").GetComponent<Slider>().value = value;

        SFX.getVolume(out value, out trash);
        GameObject.Find("SFXSlider").GetComponent<Slider>().value = value;

        Music.getVolume(out value, out trash);
        GameObject.Find("MusicSlider").GetComponent<Slider>().value = value;

        UI.getVolume(out value, out trash);
        GameObject.Find("UISlider").GetComponent<Slider>().value = value;
    }

    /// <summary>
    /// Change the mixer volume for UI volume
    /// </summary>
    /// <param name="newVolume"></param>
    public void ChangeUIVolume(Slider newVolume)
    {
        UI.setVolume(newVolume.value);
    }

    /// <summary>
    /// Change the mixer volume for music volume
    /// </summary>
    /// <param name="newVolume"></param>
    public void ChangeMusicVolume(Slider newVolume)
    {
        Music.setVolume(newVolume.value);
    }

    /// <summary>
    /// Change the mixer volume for SFX volume
    /// </summary>
    /// <param name="newVolume"></param>
    public void ChangeSFXVolume(Slider newVolume)
    {
        SFX.setVolume(newVolume.value);
    }

    /// <summary>
    /// Change the mixer volume for all sounds in game
    /// </summary>
    /// <param name="newVolume"></param>
    public void ChangeMasterVolume(Slider newVolume)
    {
        Master.setVolume(newVolume.value);
    }


    /// <summary>
    /// Change the mixer volume for UI volume
    /// </summary>
    /// <param name="newVolume"></param>
    public float GetUIVolume()
    {
        float volume;
        UI.getVolume(out volume);
        return volume;
    }

    /// <summary>
    /// Change the mixer volume for music volume
    /// </summary>
    /// <param name="newVolume"></param>
    public float GetMusicVolume(float newVolume)
    {
        float volume;
        Music.getVolume(out volume);
        return volume;
    }

    /// <summary>
    /// Change the mixer volume for SFX volume
    /// </summary>
    /// <param name="newVolume"></param>
    public float GetSFXVolume(float newVolume)
    {
        float volume;
        SFX.getVolume(out volume);
        return volume;
    }

    /// <summary>
    /// Change the mixer volume for all sounds in game
    /// </summary>
    /// <param name="newVolume"></param>
    public float GetMasterVolume(float newVolume)
    {
        float volume;
        Master.getVolume(out volume);
        return volume;
    }


}
