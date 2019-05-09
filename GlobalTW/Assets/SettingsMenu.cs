using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public Dropdown resolutionDD;

    public Slider Vslider;

    private void Start()
    {
        float temp;
        if(audioMixer.GetFloat("MasterVolume",out temp))
        {
            Vslider.value = temp;
        }

        resolutions = Screen.resolutions;

        resolutionDD.ClearOptions();

        List<string> options = new List<string>();

        int current = 0;

        for(int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width / 16 == resolutions[i].height / 9)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;

                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    current = i;
                }
            }
        }

        resolutionDD.AddOptions(options);
        resolutionDD.value = current;
        resolutionDD.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        if (volume < -19)
            volume = -80;
        audioMixer.SetFloat("MasterVolume", volume);
    }


    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
