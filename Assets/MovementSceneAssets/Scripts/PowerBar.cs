using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxPower(float maxPower)
    {
        slider.maxValue = maxPower;
    }

    public void SetPower(float power)
	{
        slider.value = power;
	}
}
