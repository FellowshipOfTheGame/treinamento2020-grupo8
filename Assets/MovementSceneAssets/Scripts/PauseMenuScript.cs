using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class PauseMenuScript : MonoBehaviour
{
	public AudioMixer audioMixer;
	Resolution[] resolutions;
	public Dropdown resolutionDropdown;


	[SerializeField] private GameObject PauseMenuScreen;


	void Start()
	{
		resolutions = Screen.resolutions;
		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}


		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}


	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("volume", volume);
	}


	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}


	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}


	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}


	public void CloseMenu()
    {
		Time.timeScale = 1f;
		PauseMenuScreen.SetActive(false);
    }


	public void ExitGame()
    {
		Debug.Log("Exit");
		Application.Quit();
    }
}
