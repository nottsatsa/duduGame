using UnityEngine;
using TMPro;

public class garagInfo : MonoBehaviour
{
    public float gravityMultiplier = 1.0f;
    public TMP_Text weightText;

    void Update()
    {
        if (PlanetInfoManager.latestWeight > 0 && weightText != null)
        {
            float calculatedWeight = PlanetInfoManager.latestWeight * gravityMultiplier;
            weightText.text = $"{calculatedWeight:F2} kg";
        }
    }
}
