using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ACountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 10f;    
    private float currentTime;
    private bool hasEnded = false;
    public FaceDetectionController faceDetector;

    void Start()
    {
        currentTime = countdownTime;
        UpdateCountdownDisplay();
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateCountdownDisplay();
            
            if (currentTime <= 0 && !hasEnded)
            {
                currentTime = 0;
                countdownText.text = "00:00";
                hasEnded = true;
                faceDetector.StartFaceDetection();
            }
        }
    }

    void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}