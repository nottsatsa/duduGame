using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class moonWR : MonoBehaviour{
    [Header("ESP32 Settings")]
    public string serverAddress = "http://192.168.4.1"; // ESP32-ын Access Point хаяг
    public float updateInterval = 0.5f; // Шинэчлэх хугацаа

    [Header("UI Settings")]
    public TMP_Text moonweightDisplay; // Жин харуулах текст
    public TMP_Text mercuryweightDisplay;
    public TMP_Text venusweightDisplay;
    public TMP_Text weightDisplay;
    public TMP_Text marsweightDisplay;
    public TMP_Text jupiterweightDisplay;
    public TMP_Text saturnweightDisplay;
    public TMP_Text uranusweightDisplay;
    public TMP_Text neptuneweightDisplay;
    public TMP_Text statusDisplay; // Статус харуулах текст
    public GameObject connectionErrorPanel; // Холболтын алдааны панел

    private int currentWeight;
    private bool isConnected = false;

    void Start()
    {
        StartCoroutine(UpdateWeightData());
    }

    IEnumerator UpdateWeightData()
    {
        while (true)
        {
            yield return StartCoroutine(GetRequest("/weight"));
            yield return new WaitForSeconds(updateInterval);
        }
    }

    IEnumerator GetRequest(string endpoint)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(serverAddress + endpoint))
        {
            // Request илгээх
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                // Алдаа гарсан тохиолдолд
                Debug.LogError("Error: " + webRequest.error);
                UpdateConnectionStatus(false);
            }
            else
            {
                // Амжилттай холбогдсон
                UpdateConnectionStatus(true);
                
                // JSON өгөгдлийг задлах
                string jsonResponse = webRequest.downloadHandler.text;
                ParseWeightData(jsonResponse);
            }
        }
    }

    void ParseWeightData(string jsonResponse)
    {
        try
        {
            // JSON-г задлах
            WeightData data = JsonUtility.FromJson<WeightData>(jsonResponse);
            currentWeight = data.weight;

            // UI дээр харуулах
            if (weightDisplay != null)
                moonweightDisplay.text = $"Саран дээр {currentWeight * 0.17}кг";
            mercuryweightDisplay.text = $"Буд дээр {currentWeight * 0.38}кг";
            venusweightDisplay.text = $"Сугар дээр {currentWeight * 0.91}кг";
            weightDisplay.text = $"Дэлхий дээр {currentWeight}кг";
            marsweightDisplay.text = $"Ангараг дээр {currentWeight * 0.38}кг";
            jupiterweightDisplay.text = $"Вөөөө! Бархасбадь {currentWeight * 2.34}кг";
            saturnweightDisplay.text = $"Санчир дээр {currentWeight * 1.06}кг";
            uranusweightDisplay.text = $"Тэнгэрийн ван дээр {currentWeight * 0.92}кг";
            neptuneweightDisplay.text = $"Далайн ван дээр {currentWeight*1.19}кг";
                  
        }
        catch (System.Exception e)
        {
            Debug.LogError("JSON parsing error: " + e.Message);
        }
    }

    void UpdateConnectionStatus(bool connected)
    {
        isConnected = connected;
        
        if (statusDisplay != null)
            statusDisplay.text = connected ? "Холболт: Идэвхтэй" : "Холболт: Тасарсан";
            
        if (connectionErrorPanel != null)
            connectionErrorPanel.SetActive(!connected);
    }

    [System.Serializable]
    private class WeightData
    {
        public int weight;
    }
}
