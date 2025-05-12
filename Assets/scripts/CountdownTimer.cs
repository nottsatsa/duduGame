using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdownTime = 10f;    
    private float currentTime;
    private bool hasEnded = false;  // Шинэ хувьсагч: таймер дууссан эсэхийг хянах

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
        }
        else if (!hasEnded)  // Зөвхөн анхны удаа дууссан тохиолдолд
        {
            currentTime = 0;  // Сөрөг утгаас сэргийлнэ
            countdownText.text = "00:00";
            hasEnded = true;  // Таймер дууссан гэж тэмдэглэнэ
            Invoke("LoadNextScene", 0.5f); 
        }
    }

    void UpdateCountdownDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("cameraScene");
    }
}