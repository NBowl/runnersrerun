using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

	public AudioMixer audioMixer;
	public Slider volumeSlider;
	float currentVolume;
    public void OnSliderValueChanged(float value)
{
	audioMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
}

public void SetFullscreen(bool isFullscreen)
{
	Screen.fullScreen = isFullscreen;
}

}
