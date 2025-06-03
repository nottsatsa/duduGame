using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections; // ← Үүнийг нэмэх
using System.Collections.Generic;

public class WeightManager : MonoBehaviour
{
    [Header("ESP32 Тохиргоо")]
    public string serverAddress = "http://192.168.4.1";
    public float updateInterval = 0.5f;

    [Header("Гаригийн UI элементүүд")]
    public List<PlanetUI> planetUIs;

    private Dictionary<string, int> planetWeights = new Dictionary<string, int>();
    private bool isConnected = false;

    [System.Serializable]
    public class PlanetUI
    {
        public string planetName;
        public TMP_Text weightDisplay;
        public TMP_Text statusDisplay;
        public GameObject errorPanel;
        public float weightMultiplier;
    }

    void Start()
    {
        foreach (var planet in planetUIs)
        {
            planetWeights[planet.planetName] = 0;
        }
        StartCoroutine(UpdateAllWeights());
    }

    IEnumerator UpdateAllWeights() // ← Тийм ээ, зөвхөн IEnumerator
    {
        while (true)
        {
            yield return StartCoroutine(FetchWeightData());
            UpdateAllUI();
            yield return new WaitForSeconds(updateInterval);
        }
    }

   IEnumerator FetchWeightData()
{
    using (UnityWebRequest webRequest = UnityWebRequest.Get(serverAddress + "/weight"))
    {
        yield return webRequest.SendWebRequest();

        isConnected = !(webRequest.result == UnityWebRequest.Result.ConnectionError || 
                        webRequest.result == UnityWebRequest.Result.ProtocolError);

        if (isConnected)
        {
            ParseWeightData(webRequest.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Request error: " + webRequest.error);
        }
    }
}


    void ParseWeightData(string jsonResponse)
    {
        try
        {
            WeightData data = JsonUtility.FromJson<WeightData>(jsonResponse);
            foreach (var planet in planetUIs)
            {
                planetWeights[planet.planetName] = data.weight;
            }
        }
        catch
        {
            Debug.LogError("JSON parsing error!");
        }
    }

    void UpdateAllUI()
    {
        foreach (var planet in planetUIs)
        {
            planet.weightDisplay.text = $"{planet.planetName} дээр {planetWeights[planet.planetName] * planet.weightMultiplier}кг";
            planet.statusDisplay.text = isConnected ? "Идэвхтэй" : "Тасарсан";
            planet.errorPanel.SetActive(!isConnected);
        }
    }

    [System.Serializable]
    private class WeightData
    {
        public int weight;
    }
}
