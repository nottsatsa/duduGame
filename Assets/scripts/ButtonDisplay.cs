using UnityEngine;
using TMPro; // TextMeshPro-ийн namespace

public class ButtonDisplay : MonoBehaviour
{
    public TMP_Text displayText; // TextMeshPro текст
    public Animator textAnimator;
    public float displayTime = 2f;
    public PlanetInfoManager infoManager; // PlanetInfoManager-г холбоно


    void Start()
    {
        // Автоматаар олох тохиргоо
        if (displayText == null) displayText = GetComponent<TMP_Text>();
        if (textAnimator == null) textAnimator = GetComponent<Animator>();
    }

    public void ShowButtonName(string buttonName)
{
    displayText.text = buttonName;
    textAnimator.ResetTrigger("Hide");
    textAnimator.SetTrigger("Show");
    CancelInvoke("HideText");
    Invoke("HideText", displayTime);

    // PlanetInfoManager-ийг ашиглан info харуулах
    if (infoManager != null)
    {
        // Гаригийн нэрээр индексийг олох
        for (int i = 0; i < infoManager.planetInfoObjects.Length; i++)
        {
           if (infoManager.planetInfoObjects[i] != null && infoManager.planetInfoObjects[i].name.ToLower().Contains(buttonName.ToLower()))
            {
                infoManager.ShowPlanetInfo(i);
                break;
            }
        }
    }
}


    void HideText()
    {
        textAnimator.ResetTrigger("Show");
        textAnimator.SetTrigger("Hide");
    }
}