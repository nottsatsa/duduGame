using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class WeighReceive : MonoBehaviour
{
    [Header("ESP32 Settings")]
    public string serverAddress = "http://192.168.4.1"; // ESP32 Access Point хаяг
    public float updateInterval = 1f; // Шинэчлэх хугацаа

    [Header("UI Settings")]
    public TMP_Text weightDisplay; // Жин харуулах текст
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
                Debug.Log("Received: " + jsonResponse); // Лог дээр хүлээн авсан өгөгдлийг харах
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
                weightDisplay.text = $"Дэлхий дээр {currentWeight}кг!";
        }
        catch (System.Exception e)
        {
            Debug.LogError("JSON parsing error: " + e.Message);
            Debug.LogError("Raw JSON: " + jsonResponse); // Алдаа гарсан JSON өгөгдлийг харах
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



// 5.19 hurtel bolj bsn kod ugn zgr ajilj bsn genet jin holbogdohoo boliv 
// using UnityEngine;
// using UnityEngine.Networking;
// using System.Collections;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class WeighReceive : MonoBehaviour{
//     [Header("ESP32 Settings")]
//     public string serverAddress = "http://192.168.4.1"; // ESP32-ын Access Point хаяг
//     public float updateInterval = 1f; // Шинэчлэх хугацаа

//     [Header("UI Settings")]
//     public TMP_Text weightDisplay; // Жин харуулах текст
//     public TMP_Text statusDisplay; // Статус харуулах текст
//     public GameObject connectionErrorPanel; // Холболтын алдааны панел

//     private int currentWeight;
//     private bool isConnected = false;

 
//     void Start()
//     {
//         StartCoroutine(UpdateWeightData());
//     }

//     IEnumerator UpdateWeightData()
//     {
//         while (true)
//         {
//             yield return StartCoroutine(GetRequest("/weight"));
//             yield return new WaitForSeconds(updateInterval);
//         }
//     }

//     IEnumerator GetRequest(string endpoint)
//     {
//         using (UnityWebRequest webRequest = UnityWebRequest.Get(serverAddress + endpoint))
//         {
//             // Request илгээх
//             yield return webRequest.SendWebRequest();

//             if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
//                 webRequest.result == UnityWebRequest.Result.ProtocolError)
//             {
//                 // Алдаа гарсан тохиолдолд
//                 Debug.LogError("Error: " + webRequest.error);
//                 UpdateConnectionStatus(false);
//             }
//             else
//             {
//                 // Амжилттай холбогдсон
//                 UpdateConnectionStatus(true);
                
//                 // JSON өгөгдлийг задлах
//                 string jsonResponse = webRequest.downloadHandler.text;
//                 ParseWeightData(jsonResponse);
//             }
//         }
//     }

//     void ParseWeightData(string jsonResponse)
//     {
//         try
//         {
//             // JSON-г задлах
//             WeightData data = JsonUtility.FromJson<WeightData>(jsonResponse);
//             currentWeight = data.weight;

//             // UI дээр харуулах
//             if (weightDisplay != null)
//                 weightDisplay.text = $"Дэлхий дээр {currentWeight}кг!";
//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError("JSON parsing error: " + e.Message);
//         }
//     }

//     void UpdateConnectionStatus(bool connected)
//     {
//         isConnected = connected;
        
//         if (statusDisplay != null)
//             statusDisplay.text = connected ? "Холболт: Идэвхтэй" : "Холболт: Тасарсан";
            
//         if (connectionErrorPanel != null)
//             connectionErrorPanel.SetActive(!connected);
//     }

//     [System.Serializable]
//     private class WeightData
//     {
//         public int weight;
//     }
// }
// // /////////////////////////////////////////
// // {
// //    [Header("ESP32 settings")]
// //    public string ipAddress = "192.168.4.1"; // ESP32 AP IP
// //    public int port=80;

// //    [Header("Display settings")]
// //    public float updateWeightPerSecond = 2f;
// //    public TMP_Text weightDisplay;

// // //esp32 holboltiin settings
// //    private TcpClient espConnection; //ESP32 төхөөрөмжтэй холбогдох холболтын объект
// //    private NetworkStream espRead;
// //    private float lastUpdateTime;
// //    private int currentWeight;
 
// //     void Start()
// //     {
// //         ConnectToESP32();
// //         lastUpdateTime=Time.time; // Time.time->Тоглоом эхлэлээс хойш өнгөрсөн секунд
// //     }
     
// //     void Update()
// //     {
// //         if(Time.time-lastUpdateTime>=updateWeightPerSecond)
// //         {
// //             RequestWeightData();
// //             lastUpdateTime=Time.time;
// //         }
// //     }

// //     void ConnectToESP32()
// //     {
// //         try
// //         {
// //             espConnection=new TcpClient(ipAddress, port); //IP (192.168.4.1) болон порт (80) руу TCP/IP холболт үүсгэдэг.
// //             espRead=espConnection.GetStream(); //Өгөгдөл дамжуулах суваг нээх
// //             Debug.Log("Connected to ESP32");
// //         }
// //          catch (Exception e) //buh aldaanii error
// //         {
// //             Debug.LogError("Connection error: " + e.Message);
// //         }
// //     }

// //     void RequestWeightData()
// //     {
// //         try
// //         {
// //             // HTTP GET request
// //             string request="GET/weight HTTP/1.1\r\nHost :"+ipAddress+"\r\n\r\n";
// //             byte[] data = Encoding.ASCII.GetBytes(request);
// //             espRead.Write(data,0,data.Length);
            
// //             // GET response
// //             byte[] buffer = new byte[1024];
// //             int bytesRead = espRead.Read(buffer, 0, buffer.Length);
// //             string response = Encoding.ASCII.GetString(buffer,0,bytesRead);

// //             ParseWeightData(response);//response uzemj saijruulah
// //         }
// //         catch (Exception e)
// //         {
// //             Debug.LogError("Communication error: "+e.Message);
// //             Reconnect();
// //         }
// //     }

// //     void ParseWeightData(string jsonResponse)
// //     {
// //         try{
// //             int startIndex = jsonResponse.IndexOf('{');
// //             int endIndex = jsonResponse.LastIndexOf('}');
// //             if (startIndex>=0 && endIndex>startIndex)
// //             {
// //                 string jsonStr= jsonResponse.Substring(startIndex, endIndex-startIndex+1);
// //                 var jsonData = JsonUtility.FromJson<WeightData>(jsonStr);
// //                 currentWeight = jsonData.weight_In_g;

// //                 // UI дээр харуулах
// //                 if(weightDisplay!=null)
// //                 {
// //                     weightDisplay.text=$"Жин : {currentWeight}гр";
// //                 }
// //             }
// //         }
// //         catch (Exception e)
// //         {
// //             Debug.LogError("Parsing error: " + e.Message);
// //         }
// //     }

// //     void Reconnect()
// //     {
// //         if(espConnection!=null) espConnection.Close();
// //         ConnectToESP32();
// //     }

// //     void OnDestroy()
// //     {
// //         if(espRead!=null) espRead.Close();
// //         if(espConnection!=null) espConnection.Close();
// //     }

// //     [System.Serializable]
// //     private class WeightData
// //     {
// //         public int weight_In_g;
// //     }
// // }
