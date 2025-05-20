using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoonSceneController : MonoBehaviour
{
    public TMP_Text weightText; // Unity Editor дээр үүнийг Text UI элементтэй холбоно
 public enum Planet
    {
        Earth,
        Moon,
        Mars,
        Jupiter
    }
    public Planet planet = Planet.Moon;

    float GetMultiplier()
    {
        switch (planet)
        {
            case Planet.Moon: return 0.165f;
            case Planet.Mars: return 0.38f;
            case Planet.Jupiter: return 2.34f;
            default: return 1f; // Earth
        }
    }

    void Update()
    {
        float multiplier = GetMultiplier();
        float adjustedWeight = ESP32Reader.currentWeight * multiplier;
        weightText.text = planet.ToString() + " дээрх жин: " + adjustedWeight.ToString("F2") + " кг";
    }
}