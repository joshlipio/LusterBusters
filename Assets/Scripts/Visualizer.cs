using System;
using UnityEngine;
using System.Runtime.InteropServices;

class Visualizer : MonoBehaviour
{
    FMOD.Studio.EventInstance musicInstance;
    FMOD.DSP fft;

    //Number of sameples to get from sound instance
    const int numSamples = 1024;

    //Value of each frequency band level
    float[] freqBands = new float[64];

    //Smoothed value of each corresponding frequency band
    float[] bandBuffer = new float[64];

    //Keeps track of how rapidly each band decreases
    float[] bufferDecrease = new float[64];

    public float amplitude;

    public LineRenderer currentLine;

    public string songString;


    void Start()
    {
        FMODUnity.RuntimeManager.CoreSystem.createDSPByType(FMOD.DSP_TYPE.FFT, out fft);
        fft.setParameterInt((int)FMOD.DSP_FFT.WINDOWTYPE, (int)FMOD.DSP_FFT_WINDOW.HANNING);
        fft.setParameterInt((int)FMOD.DSP_FFT.WINDOWSIZE, numSamples); //number of samples

        //Get master channel group
        FMOD.ChannelGroup channelGroup;
        FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out channelGroup);
        channelGroup.addDSP(FMOD.CHANNELCONTROL_DSP_INDEX.HEAD, fft);

        //Used for visualizer testing
        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/" + songString);
        musicInstance.start();
    }

    void Update()
    {
        SpectrumData();      
        Buffer();
        VisualizeBands();
        Amplitude();

        if (Time.timeScale == 0)
        {
            musicInstance.setPaused(true);
        }
        else
        {
            musicInstance.setPaused(false);
        }
    }

    /// <summary>
    /// Get spectrum data from master channel
    /// </summary>
    void SpectrumData()
    {
        IntPtr unmanagedData;
        uint length;
        fft.getParameterData((int)FMOD.DSP_FFT.SPECTRUMDATA, out unmanagedData, out length);
        FMOD.DSP_PARAMETER_FFT fftData = (FMOD.DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(FMOD.DSP_PARAMETER_FFT));
        var spectrum = fftData.spectrum; //float[numChannels][numSamples]

        if (fftData.numchannels > 0)
        {
            FrequencyBands64(spectrum);
        }
    }

    /// <summary>
    /// Calculate values for each band based on spectrum data received
    /// </summary>
    /// <param name="spectrumData"></param>
    void FrequencyBands64(float[][] spectrumData)
    {
        //Count is the current sample being checked
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        //For each band
        for (int i = 1; i < 64; i++)
        {        
            float avg = 0;
            
            if (i==16||i==32||i==40||i==48||i==56)
            {
                power++;
                //Calculates # of samples in band i
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 3)
                    sampleCount -= 2;
            }

            //Calc this band value based on how many samples it should contain
            for (int j = 0; j < sampleCount; j++)
            {
                //Convert from linear to logarithmic
                avg += (spectrumData[0][count] + spectrumData[1][count]) * (count + 1);
                count++;
            }
            avg /= count;

            if (i < 4)
                freqBands[i] = avg * 20;
            else
                freqBands[i] = avg * 160;
        }
    }



    /// <summary>
    /// Smooths out values so visualizer isn't so jittery
    /// </summary>
    void Buffer()
    {
        for (int i = 0; i < 64; i++)
        {
            //If source sound band is higher, use source sound band
            if (freqBands[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBands[i];
                bufferDecrease[i] = 0.005f;
            }

            //If source sound band is lower, decrease band visualization exponentially
            if (freqBands[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    //Should probably take this function freqCubes out of this class in the future
    /// <summary>
    /// Visualize bands by modifying cube scale
    /// </summary>
    void VisualizeBands()
    {
        for (int i = 0; i < 64; i++)
        {
            currentLine.SetPosition(i, new Vector3(i*0.45f, bandBuffer[i],0));
        }
    }

    void Amplitude()
    {
        float currentAmplitude = 0;
        for (int i = 0; i < 64; i++)
        {
            currentAmplitude += bandBuffer[i];
        }
        amplitude = currentAmplitude;    
    }

    void OnDestroy()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();
    }

}

/* Some random band-sample calculations
 
    32 bands
    0-7     1 sample
    7-11    2 
    12-15	4
    16-19   8 
    20-23   16
    24-27	32
    28-32	64 +
            ---
            512

    8 bands
    0	2
    1	4
    2	8
    3	16
    4	32
    5	64
    6	128
    7	256 +
        ---
        510
 */ 
