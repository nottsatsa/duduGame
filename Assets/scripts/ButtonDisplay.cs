using UnityEngine;
using TMPro; // TextMeshPro-ийн namespace

public class ButtonDisplay : MonoBehaviour
{
    public TMP_Text displayText; // TextMeshPro текст
    public Animator textAnimator;
    public float displayTime = 2f;

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
    }

    void HideText()
    {
        textAnimator.ResetTrigger("Show");
        textAnimator.SetTrigger("Hide");
    }
}