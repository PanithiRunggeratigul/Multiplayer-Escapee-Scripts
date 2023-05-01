using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PlayerSettingsManager : MonoBehaviour
{
    [SerializeField] Slider sensitivity_slider;
    [SerializeField] Slider audio_slider;
    [SerializeField] TMP_Dropdown graphics_dropdown;
    [SerializeField] AudioMixer mixer;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreen;
    float sound;
    float sensitivity;
    int graphics;
    int screen_resolution;
    int fullscreen_resolution;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i=0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();

        sensitivity = PlayerPrefs.GetFloat("currentSensitivity");
        sensitivity_slider.value = sensitivity;

        sound = PlayerPrefs.GetFloat("VolumeLevel");
        audio_slider.value = sound;

        graphics = PlayerPrefs.GetInt("Graphics");
        graphics_dropdown.value = graphics;

        screen_resolution = PlayerPrefs.GetInt("Resolution");
        resolutionDropdown.value = screen_resolution;

        fullscreen_resolution = PlayerPrefs.GetInt("IsFullscreen");
        if (fullscreen_resolution == 1)
        {
            fullscreen.GetComponent<Toggle>().isOn = true;
        }
        else if (fullscreen_resolution == 0)
        {
            fullscreen.GetComponent<Toggle>().isOn = false;
        }
    }

    public void ChangeSensitivity(float newMouseSens)
    {
        sensitivity = newMouseSens;
        PlayerPrefs.SetFloat("currentSensitivity", sensitivity);
    }

    public void SetVolume(float volume)
    {
        sound = volume;
        PlayerPrefs.SetFloat("VolumeLevel", sound);
        mixer.SetFloat("volume", sound);
    }

    public void SetGraphics(int graphicsindex)
    {
        graphics = graphicsindex;
        PlayerPrefs.SetInt("Graphics", graphics);
        QualitySettings.SetQualityLevel(graphicsindex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        if (isFullScreen)
        {
            fullscreen_resolution = 1;
        }
        else
        {
            fullscreen_resolution = 0;
        }
        
        PlayerPrefs.SetInt("IsFullscreen", fullscreen_resolution);

        if (fullscreen_resolution == 1)
        {
            Screen.fullScreen = true;
        }
        else if (fullscreen_resolution == 0)
        {
            Screen.fullScreen = false;
        }
        
    }

    public void SetScreenResolution(int resolutionIndex)
    {
        screen_resolution = resolutionIndex;
        PlayerPrefs.SetInt("Resolution", screen_resolution);
        Resolution resolution = resolutions[screen_resolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
