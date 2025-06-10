using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 10f;    
    private float currentTime;
    private bool hasEnded = false;
    // public CameraExample cameraExampl; // Камерын скрипт холбох
public cameraExampl cameraExampl;
    void Start()
    {
        currentTime = countdownTime;
        UpdateCountdownDisplay();
        Debug.Log("CountdownTimer started");
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
                Debug.Log("Countdown finished, opening camera...");
                OpenCameraDirectly(); // Шууд камер нээх функц дуудах
            }
        }
    }

    void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OpenCameraDirectly()
    {
        if (cameraExampl != null)
        {
            Debug.Log("Directly opening camera...");
            cameraExampl.TakePicture(); // Камерын скриптийн TakePicture() функц дуудах
        }
        else
        {
            Debug.LogError("CameraExample script not assigned!");
        }
    }
}