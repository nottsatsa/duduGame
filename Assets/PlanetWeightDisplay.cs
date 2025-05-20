// Жишээ нь: PlanetInfoObject доторх скрипт
using UnityEngine;
using TMPro;

public class PlanetWeightDisplay : MonoBehaviour
{
    public TMP_Text weightText; // Жинг харуулах текст
    
    void Update()
    {
        // ESP32-ийн жингийн мэдээллийг шууд авах
        float currentWeight = PlanetInfoManager.latestWeight;
        
        // Текстэнд харуулах
        if (weightText != null)
        {
            weightText.text = currentWeight.ToString("F1") + " kg";
        }
        
        // Эсвэл жингийн утгад үндэслэн өөр animation эхлүүлэх
        if (currentWeight > 100)
        {
            // Хүнд жинтэй үед тусгай эффект
        }
    }
}