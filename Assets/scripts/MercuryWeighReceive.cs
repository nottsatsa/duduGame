using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;
using TMPro;

public class MercuryWeighReceive : MonoBehaviour
{
    [Header("ESP32 Settings")]
    public string ipAddress = "192.168.4.1"; // ESP32-ын Access Point IP
    public int port = 80;

    [Header("Display Settings")]
    public float updateInterval = 2f; // 2 секунд тутамд шинэчлэх
    public TMP_Text weightDisplay; // UI Text (TextMeshPro)

    private TcpClient client;
    private NetworkStream stream;
    private float lastUpdateTime;
    private int currentWeight;

    void Start()
    {
        ConnectToESP32();
        lastUpdateTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            RequestWeightData();
            lastUpdateTime = Time.time;
        }
    }

    void ConnectToESP32()
    {
        try
        {
            client = new TcpClient(ipAddress, port);
            stream = client.GetStream();
            Debug.Log("Connected to ESP32");
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e.Message);
        }
    }

    void RequestWeightData()
    {
        try
        {
            // HTTP GET request илгээх
            string request = "GET /weight HTTP/1.1\r\nHost: " + ipAddress + "\r\n\r\n";
            byte[] data = Encoding.ASCII.GetBytes(request);
            stream.Write(data, 0, data.Length);

            // Хариу хүлээж авах
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // JSON парсер хийх
            ParseWeightData(response);
        }
        catch (Exception e)
        {
            Debug.LogError("Communication error: " + e.Message);
            Reconnect();
        }
    }

    void ParseWeightData(string jsonResponse)
    {
        try
        {
            // JSON-г задлах (энгийн version)
            int startIndex = jsonResponse.IndexOf('{');
            int endIndex = jsonResponse.LastIndexOf('}');
            if (startIndex >= 0 && endIndex > startIndex)
            {
                string jsonStr = jsonResponse.Substring(startIndex, endIndex - startIndex + 1);
                var jsonData = JsonUtility.FromJson<WeightData>(jsonStr);
                currentWeight = jsonData.weight_In_g;

                // UI дээр харуулах
                if (weightDisplay != null)
                    weightDisplay.text = $"Жин: {currentWeight}g";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Parsing error: " + e.Message);
        }
    }

    void Reconnect()
    {
        if (client != null) client.Close();
        ConnectToESP32();
    }

    void OnDestroy()
    {
        if (stream != null) stream.Close();
        if (client != null) client.Close();
    }

    [System.Serializable]
    private class WeightData
    {
        public int weight_In_g;
    }
}
