// using UnityEngine;
// using TMPro; // TextMeshPro-ийн namespace
// using System.Collections.Generic; // Энийг нэмнэ


// public class ButtonDisplay : MonoBehaviour
// {
//     public TMP_Text displayText; // TextMeshPro текст
//     public Animator textAnimator;
//     public float displayTime = 2f;
//     public PlanetInfoManager infoManager; // PlanetInfoManager-г холбоно


//     void Start()
//     {
//         // Автоматаар олох тохиргоо
//         if (displayText == null) displayText = GetComponent<TMP_Text>();
//         if (textAnimator == null) textAnimator = GetComponent<Animator>();
//     }

//     public void ShowButtonName(string buttonName)
// {
//     // Товчны нэрийг харгалзах текстээр солих
//     string displayName = buttonName switch
//     {
//         "moonInfo" => "Сар",
//         "earthInfo" => "Дэлхий",
//         "marsInfo" => "Ангараг",
//         _ => buttonName // Бусад тохиолдолд анхны нэрийг ашиглана
//     };

//     displayText.text = displayName;
//     textAnimator.ResetTrigger("Hide");
//     textAnimator.SetTrigger("Show");
//     CancelInvoke("HideText");
//     Invoke("HideText", displayTime);

//     // PlanetInfoManager-ийг ашиглан info харуулах
//     if (infoManager != null)
//     {
//         // Гаригийн нэрээр индексийг олох
//         for (int i = 0; i < infoManager.planetInfoObjects.Length; i++)
//         {
//             if (infoManager.planetInfoObjects[i] != null && 
//                 infoManager.planetInfoObjects[i].name.ToLower().Contains(buttonName.ToLower()))
//             {
//                 infoManager.ShowPlanetInfo(i);
//                 break;
//             }
//         }
//     }
// }
//     void HideText()
//     {
//         textAnimator.ResetTrigger("Show");
//         textAnimator.SetTrigger("Hide");
//     }
// }


using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ButtonDisplay : MonoBehaviour
{
    public TMP_Text displayText;
    public float displayTime = 2f;
    public PlanetInfoManager infoManager;

    // Монгол нэршилтэй толь бичиг
    private readonly Dictionary<string, string> mongolNames = new()
    {
        {"moon", "Сар"},
        {"earth", "Дэлхий"},
        {"mars", "Ангараг"},
        {"jupiter", "Бархасбадь"},
        {"mercury", "Буд"},
        {"saturn", "Санчир"},
        {"uranus", "Тэнгэрийн ван"},
        {"neptune", "Далай ван"},
        {"venus", "Сугар"}
    };

    void Start()
    {
        if (displayText == null) displayText = GetComponent<TMP_Text>();
    }

    public void ShowButtonName(string buttonName)
    {
        string lowerName = buttonName.ToLower();
        string matchedKey = mongolNames.Keys.FirstOrDefault(key => lowerName.Contains(key));
        string displayName = matchedKey != null ? mongolNames[matchedKey] : buttonName;

        displayText.text = displayName;

        if (infoManager != null)
        {
            for (int i = 0; i < infoManager.planetInfoObjects.Length; i++)
            {
                if (infoManager.planetInfoObjects[i] != null && 
                    infoManager.planetInfoObjects[i].name.ToLower().Contains(lowerName))
                {
                    infoManager.ShowPlanetInfo(i);
                    break;
                }
            }
        }

        // Зөвхөн текст харуулах, анимейшнгүй
        CancelInvoke("HideText");
        Invoke("HideText", displayTime);
    }

    void HideText()
    {
        displayText.text = ""; // Эсвэл null үсгүүдийг арилгах
    }
}