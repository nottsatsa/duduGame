using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ESP32Reader : MonoBehaviour
{
    public static float currentWeight = 0f; // Бусад скриптүүд эндээс жинг авна
    public string serverAddress = "http://192.168.4.1";
    public float updateInterval = 1f;

    void Start()
    {
        StartCoroutine(UpdateWeight());
    }

    IEnumerator UpdateWeight()
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(serverAddress);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                WeightData data = JsonUtility.FromJson<WeightData>(json);
                currentWeight = data.weight;
                Debug.Log("ESP32 weight: " + currentWeight);
            }
            else
            {
                Debug.LogWarning("ESP32 Error: " + www.error);
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }

    [System.Serializable]
    public class WeightData
    {
        public float weight;
    }
}
